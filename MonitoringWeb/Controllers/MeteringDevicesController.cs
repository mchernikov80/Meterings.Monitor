using MonitoringDB.Model.Sql;
using MonitoringDB.Model.Sql.Extentions;
using MonitoringWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MonitoringWeb.Controllers
{
    public class MeteringDevicesController : Controller
    {
        public async Task<ActionResult> Index(string house, int days = 5)
        {
            var meteringDevices = await MonitoringDbRepository.GetMeteringDevicesForCheckAsync(days, house);
            var devicesModel = meteringDevices
                .Select(mp => new MeteringDeviceModel {
                                                        DeviceType = mp.DeviceType,
                                                        SerialNo = mp.SerialNo,
                                                        CheckDate = mp.CheckDate
                                                    }).ToList();

            var model = new MeteringDevicesModel { DevicesForCheckFilter = new MeteringDevicesForCheckFilterModel { Days = days, House = house }, MeteringDevices = devicesModel };
            return View("Index", model);
        }

        public async Task<ActionResult> DevicesHistory(long pointId)
        {
            var meteringPoint = await MonitoringDbRepository.GetMeteringPointAsync(pointId);
            var model = new MeteringDevicesHistoryModel { 
                MeteringPoint = $"Адрес: улица {meteringPoint.Street} дом {meteringPoint.House} кв.{meteringPoint.Flat}; Владелец:{meteringPoint.Owner}",
                MeteringDevicesHistory = meteringPoint.MeteringDeviceHistory?.Select(h => new MeteringDevicesInfoModel {
                                                                                                                DeviceType = h.MeteringDevice.DeviceType,
                                                                                                                SerialNo = h.MeteringDevice.SerialNo,
                                                                                                                CheckDate = h.MeteringDevice.CheckDate,
                                                                                                                MountedAt = h.MountedAt,
                                                                                                                DemountedAt = h.DemountedAt
                                                                                                            })
                                                                                                                .OrderBy(h => h.MountedAt)
                                                                                                                .ToList()
            };
            return View("MeteringDevicesHistory", model);
        }

        public ActionResult Mount()
        {
            return View("MeteringDeviceMounting", new MeteringDeviceMountingModel());
        }

        [HttpPost]
        public async Task<ActionResult> Mount(MeteringDeviceMountingModel model)
        {
            if (ModelState.IsValid)
            {
                MeteringData meteringData = null;

                var meteringPoint = new MeteringPoint {
                    Owner = model.MeteringPointOwner,
                    Street = model.MeteringPointStreet,
                    House = model.MeteringPointHouse,
                    Flat = model.MeteringPointFlat
                };

                if (model.MeteringDataValue.HasValue)
                {
                    meteringData = new MeteringData {
                        MeteringPoint = meteringPoint,
                        CheckedAt = model.MeteringDataCheckedAt ?? DateTime.Now,
                        Value = model.MeteringDataValue.Value
                    };
                }

                var meteringDevice = new MeteringDevice { 
                    DeviceType = model.DeviceType, 
                    SerialNo = model.DeviceSerialNo, 
                    CheckDate = model.DeviceCheckDate 
                };

                var meteringDeviceInfo = new MeteringDeviceInfo
                {
                    MountedAt = model.MountedAt ?? DateTime.Now,
                    DemountedAt = model.DemountedAt,
                    MeteringDevice = meteringDevice,
                    MeteringPoint = meteringPoint
                };


                await MonitoringDbRepository.SaveAsync(meteringPoint, meteringData, meteringDevice, meteringDeviceInfo);

                ViewBag.ResultMessage = "Данные о счетчике успешно сохранены";
                return View("MeteringDeviceMounting");
            }
            return View("MeteringDeviceMounting", model);
        }
    }
}