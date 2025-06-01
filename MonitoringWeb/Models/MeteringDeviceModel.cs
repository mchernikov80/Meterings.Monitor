using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringDeviceModel
    {
        [Required]
        [Display(Name = "Тип счетчика")]
        public string DeviceType { get; set; }

        [Required]
        [Display(Name = "Серийный номер счетчика")]
        public string SerialNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата следующей поверки счетчика")]
        public DateTime? CheckDate { get; set; }
    }
}