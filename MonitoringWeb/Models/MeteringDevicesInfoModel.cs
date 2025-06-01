using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MonitoringWeb.Models
{
    public class MeteringDevicesInfoModel
    {
        #region MeteringDevice
        [Required(ErrorMessage = "Укажите тип счетчика")]
        [Display(Name = "Тип счетчика")]
        public string DeviceType { get; set; }

        [Required(ErrorMessage = "Укажите серийный номер счетчика")]
        [Display(Name = "Серийный номер счетчика")]
        public string SerialNo { get; set; }

        [Display(Name = "Дата следующей поверки счетчика")]
        public DateTime? CheckDate { get; set; }

        #endregion

        #region MeteringDeviceInfo
        [Display(Name = "Дата-время монтажа")]
        public DateTime? MountedAt { get; set; } = DateTime.Now;

        [Display(Name = "Дата-время демонтажа")]
        public DateTime? DemountedAt { get; set; }
        #endregion
    }
}