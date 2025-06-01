using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringDeviceMountingModel
    {
        #region MeteringPoint
        [Required(ErrorMessage = "Укажите данные о владельце точки учета")]
        [Display(Name = "Владелец")]
        public string MeteringPointOwner { get; set; }

        [Required(ErrorMessage = "Укажите улицу точки учета")]
        [Display(Name = "Улица")]
        public string MeteringPointStreet { get; set; }

        [Required(ErrorMessage = "Укажите улицу точки учета")]
        [Display(Name = "Дом")]
        public string MeteringPointHouse { get; set; }

        [Required(ErrorMessage = "Укажите квартиру точки учета")]
        [Display(Name = "Квартира")]
        public string MeteringPointFlat { get; set; }
        #endregion

        #region MeteringDevice
        [Required(ErrorMessage = "Укажите тип счетчика")]
        [Display(Name = "Тип счетчика")]
        public string DeviceType { get; set; }

        [Required(ErrorMessage = "Укажите серийный номер счетчика")]
        [Display(Name = "Серийный номер счетчика")]
        public string DeviceSerialNo { get; set; }

        [Display(Name = "Дата следующей поверки счетчика")]
        public DateTime? DeviceCheckDate { get; set; }

        #endregion

        #region MeteringDeviceInfo
        [Display(Name = "Дата-время монтажа")]
        public DateTime? MountedAt { get; set; } = DateTime.Now;

        [Display(Name = "Дата-время демонтажа")]
        public DateTime? DemountedAt { get; set; }
        #endregion

        #region MeteringData
        [Display(Name = "Дата-время снятия показания")]
        public DateTime? MeteringDataCheckedAt { get; set; } = DateTime.Now;

        [Display(Name = "Показание счетчика")]
        public float? MeteringDataValue { get; set; }
        #endregion
    }
}