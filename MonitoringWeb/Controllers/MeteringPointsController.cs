using MonitoringDB.Model.Sql;
using MonitoringDB.Model.Sql.Extentions;
using MonitoringWeb.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MonitoringWeb.Controllers
{
    public class MeteringPointsController : Controller
    {
        const int rowsPerPage = 5;

        public async Task<ActionResult> Index(int page=0)
        {
            var query = MonitoringDbRepository.MeteringPointsInfoQuery();

            var count = query.Count();
            var pages = count / rowsPerPage;
            if (count % rowsPerPage > 0)
                pages++;

            ViewBag.Pages = pages;
            ViewBag.CurrentPage = page;

            int skip = 0;
            if (page > 0) skip = rowsPerPage * page;
            var take = rowsPerPage;

            var meteringPointsInfo = await MonitoringDbRepository.GetMeteringPointsInfoAsync(skip, take);
            var model = meteringPointsInfo.Select(mp => new MeteringPointModel
            {
                PointId = mp.Item1.PointId,
                Owner = mp.Item1.MeteringPoint.Owner,
                Street = mp.Item1.MeteringPoint.Street,
                House = mp.Item1.MeteringPoint.House,
                Flat = mp.Item1.MeteringPoint.Flat,
                DeviceValue = mp.Item1.Value,
                DeviceType = mp.Item2?.DeviceType,
                DeviceSerialNo = mp.Item2?.SerialNo,
                DeviceCheckDate = mp.Item2?.CheckDate
            }).ToList();

            return View("Index", model);
        }
    }
}