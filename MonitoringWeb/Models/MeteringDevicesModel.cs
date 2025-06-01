using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringDevicesModel
    {
        [Display(Name = "Фильтр")]
        public MeteringDevicesForCheckFilterModel DevicesForCheckFilter { get; set; }

        [Display(Name = "Список счетчиков")]
        public IList<MeteringDeviceModel> MeteringDevices { get; set; }
    }
}