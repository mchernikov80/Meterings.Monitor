using MonitoringDB.Model.Sql;
using MonitoringDB.Model.Sql.Extentions;
using MonitoringWeb.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MonitoringWeb.Controllers
{
    public class MeteringDataController : Controller
    {
        public async Task<ActionResult> Registration(long? pointId)
        {
            var model = new MeteringDataRegistrationModel();
            if (pointId.HasValue)
            {
                var meteringPoint = await MonitoringDbRepository.GetMeteringPointAsync(pointId.Value);
                model.PointId = pointId;
                model.MeteringPointOwner = meteringPoint.Owner;
                model.MeteringPointStreet = meteringPoint.Street;
                model.MeteringPointHouse = meteringPoint.House;
                model.MeteringPointFlat = meteringPoint.Flat;
            }
            return View("MeteringDataRegistration", model);
        }

        [HttpPost]
        public async Task<ActionResult> Registration(MeteringDataRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                MeteringPoint meteringPoint = null;
                if (!model.PointId.HasValue) 
                {
                    meteringPoint = new MeteringPoint
                    {
                        Owner = model.MeteringPointOwner,
                        Street = model.MeteringPointStreet,
                        House = model.MeteringPointHouse,
                        Flat = model.MeteringPointFlat
                    }; 
                }

                if (model.MeteringDataValue.HasValue)
                {
                    var meteringData = new MeteringData
                    {
                        PointId = model.PointId.GetValueOrDefault(),
                        MeteringPoint = meteringPoint,
                        CheckedAt = model.MeteringDataCheckedAt ?? DateTime.Now,
                        Value = model.MeteringDataValue.Value
                    };

                    await MonitoringDbRepository.SaveAsync(meteringPoint, meteringData, null, null);
                    ViewBag.ResultMessage = "Показания счетчика успешно зарегистрированы";
                    return View("MeteringDataRegistration");
                }
            }
            return View("MeteringDataRegistration", model);
        }
    }
}