using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MonitoringDB.Model.Sql.Extentions
{
    internal static class MonitoringDbModelExtention
    {
        internal static async Task AddOrUpdateAsync(this MonitoringDbModelContainer dbModel, MeteringPoint meteringPoint)
        {
            if (meteringPoint == null) 
                throw new ArgumentNullException(nameof(meteringPoint));

            if (meteringPoint.Id > 0)
            {
                MeteringPoint meteringPointDb = await dbModel.MeteringPoints.SingleAsync(p => p.Id == meteringPoint.Id);

                meteringPointDb.Owner = meteringPoint.Owner?.Trim();
                meteringPointDb.Street = meteringPoint.Street?.Trim();
                meteringPointDb.House = meteringPoint.House?.Trim();
                meteringPointDb.Flat = meteringPoint.Flat?.Trim();
            }
            else dbModel.MeteringPoints.Add(meteringPoint);
        }

        internal static async Task AddOrUpdateAsync(this MonitoringDbModelContainer dbModel, MeteringDevice meteringDevice)
        {
            if (meteringDevice == null)
                throw new ArgumentNullException(nameof(meteringDevice));

            if (meteringDevice.Id > 0)
            {
                MeteringDevice meteringDeviceDb = await dbModel.MeteringDevices.SingleAsync(p => p.Id == meteringDevice.Id);
                meteringDeviceDb.CheckDate = meteringDevice.CheckDate;
                meteringDeviceDb.DeviceType = meteringDevice.DeviceType?.Trim();
                meteringDeviceDb.SerialNo = meteringDevice.SerialNo?.Trim();
            }
            else dbModel.MeteringDevices.Add(meteringDevice);
        }
    }
}
