using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringDataRegistrationModel
    {
        #region MeteringPoint
        public long? PointId { get; set; }

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

        #region MeteringData
        [Display(Name = "Дата-время снятия показания")]
        public DateTime? MeteringDataCheckedAt { get; set; } = DateTime.Now;

        [Display(Name = "Показание счетчика")]
        [Required(ErrorMessage = "Укажите показание счетчика")]
        public float? MeteringDataValue { get; set; }
        #endregion
    }
}