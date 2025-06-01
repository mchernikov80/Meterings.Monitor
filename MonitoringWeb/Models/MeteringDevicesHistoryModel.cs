using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringDevicesHistoryModel
    {
        [Display(Name = "Точка учета")]
        public string MeteringPoint { get; set; }

        [Display(Name = "История замен счетчиков")]
        public IList<MeteringDevicesInfoModel> MeteringDevicesHistory { get; set; }
    }
}