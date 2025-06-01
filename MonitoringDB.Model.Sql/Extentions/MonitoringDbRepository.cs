using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDB.Model.Sql.Extentions
{
    public class MonitoringDbRepository
    {
        private readonly static MonitoringDbModelContainer _monitoringDb = new MonitoringDbModelContainer();

        /// <summary>
        /// Запрос среза данных истории показаний приборов учета
        /// </summary>
        /// <param name="period">временная точка среза</param>
        /// <returns></returns>
        private static IQueryable<MeteringData> MeteringDataSliceQuery(DateTime? period = null)
        {
            var checkDate = period;
            var query = _monitoringDb.MeteringDataHistory;
            if (!checkDate.HasValue) 
            {
                checkDate = DateTime.Today.AddDays(1);
                query.Where(d => d.CheckedAt < checkDate);
            }
            else
                query.Where(d => d.CheckedAt <= checkDate);

            return query.GroupBy(d => d.PointId, (key, gr) => gr.OrderByDescending(item => item.CheckedAt).FirstOrDefault());
        }

        /// <summary>
        /// Запрос среза данных истории показаний приборов учета с информацией о счетчиках
        /// </summary>
        /// <returns></returns>
        public static IQueryable<dynamic> MeteringPointsInfoQuery()
        {
            IQueryable<dynamic> query = MeteringDataSliceQuery()
                .GroupJoin(_monitoringDb.MeteringDeviceHistory,
                            data => data.PointId, deviceInfo => deviceInfo.PointId,
                            (data, deviceHistory) => new { data, deviceHistory })
                .SelectMany(gr => gr.deviceHistory.DefaultIfEmpty(), (gr, deviceInfo) => new { MeteringData = gr.data, MeteringDeviceInfo = deviceInfo })
                .Where(e => e.MeteringDeviceInfo == null || e.MeteringDeviceInfo.DemountedAt == null);
            return query;
        }

        /// <summary>
        /// Срез данных истории показаний приборов учета с информацией о счетчиках
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Tuple<MeteringData, MeteringDevice>>> GetMeteringPointsInfoAsync(int skip = 0, int take = 0)
        {
            IQueryable<dynamic> query = MeteringDataSliceQuery()
                .GroupJoin(_monitoringDb.MeteringDeviceHistory,
                            data => data.PointId, deviceInfo => deviceInfo.PointId,
                            (data, deviceHistory) => new { data, deviceHistory })
                .SelectMany(gr => gr.deviceHistory.DefaultIfEmpty(), (gr, deviceInfo) => new { MeteringData = gr.data, MeteringDeviceInfo = deviceInfo })
                .Where(e => e.MeteringDeviceInfo == null || e.MeteringDeviceInfo.DemountedAt == null)
                .OrderBy(p => p.MeteringData.PointId);

            if (skip > 0)
                query = query.Skip(skip);
            if (take > 0)
            query = query.Take(take);

            var items = await query.ToListAsync<dynamic>();
            return items
                .Select(result => new Tuple<MeteringData, MeteringDevice>(result.MeteringData, result.MeteringDeviceInfo != null ? result.MeteringDeviceInfo.MeteringDevice : null))
                .AsEnumerable();
        }

        public static async Task<MeteringPoint> GetMeteringPointAsync(long id)
        {
            if (id < 0) throw new ArgumentException(nameof(id));

            return await _monitoringDb.MeteringPoints
                .Include(p => p.MeteringDeviceHistory)
                .SingleAsync(p => p.Id == id);
        }

        /// <summary>
        /// Получить счетчики для проверки в течение ближайшего кол-ва дней для указанного дома
        /// </summary>
        /// <param name="days"></param>
        /// <param name="house"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<MeteringDevice>> GetMeteringDevicesForCheckAsync(int days, string house)
        {
            if (days < 0) throw new ArgumentException(nameof(days));

            var today = DateTime.Today;
            var checkDate = today.AddDays(days + 1);

            IQueryable<MeteringDevice> query = null;
            if (string.IsNullOrWhiteSpace(house))
            {
                query = _monitoringDb.MeteringDevices.Where(d => d.CheckDate != null && d.CheckDate >= today && d.CheckDate < checkDate);
                return await query.ToListAsync();
            }

            query = _monitoringDb.MeteringDeviceHistory
                .Join(_monitoringDb.MeteringPoints, deviceInfo => deviceInfo.PointId, point => point.Id,
                    (deviceInfo, point) => new { deviceInfo.MeteringDevice, MeteringPoint = point })
                .Where(d => d.MeteringDevice.CheckDate != null && d.MeteringDevice.CheckDate >= today && d.MeteringDevice.CheckDate < checkDate
                            && string.Compare(d.MeteringPoint.House, house, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(d => d.MeteringDevice);

            return await query.ToListAsync();
        }

        public static async Task SaveAsync(MeteringPoint meteringPoint, MeteringData meteringData, MeteringDevice meteringDevice, MeteringDeviceInfo meteringDeviceInfo)
        {
            if (meteringPoint != null || meteringData != null || meteringDevice != null || meteringDeviceInfo != null)
            {
                using (var dbContextTransaction = _monitoringDb.Database.BeginTransaction())
                {
                    try
                    {
                        if (meteringPoint != null)
                            await _monitoringDb.AddOrUpdateAsync(meteringPoint);

                        if (meteringData != null)
                            _monitoringDb.MeteringDataHistory.Add(meteringData);

                        if (meteringDevice != null)
                            await _monitoringDb.AddOrUpdateAsync(meteringDevice);

                        if (meteringDeviceInfo != null)
                            _monitoringDb.MeteringDeviceHistory.Add(meteringDeviceInfo);

                        await _monitoringDb.SaveChangesAsync();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}
