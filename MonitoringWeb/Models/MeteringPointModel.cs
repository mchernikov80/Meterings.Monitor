using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringPointModel
    {
        public long PointId { get; set; }

        [Required]
        [Display(Name = "ФИО Владельца")]
        public string Owner { get; set; }

        [Required]
        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Дом")]
        public string House { get; set; }

        [Required]
        [Display(Name = "Квартира")]
        public string Flat { get; set; }

        [Display(Name = "Адрес точки учета")]
        public string Address => $"улица {Street}, дом {House}, кв.{Flat}";

        [Required]
        [Display(Name = "Тип счетчика")]
        public string DeviceType { get; set; }

        [Required]
        [Display(Name = "Серийный номер счетчика")]
        public string DeviceSerialNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата следующей поверки счетчика")]
        public DateTime? DeviceCheckDate { get; set; }

        [Display(Name = "Показание")]
        public float? DeviceValue { get; set; }
    }
}