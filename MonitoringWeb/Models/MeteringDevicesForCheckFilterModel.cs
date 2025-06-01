using System.ComponentModel.DataAnnotations;

namespace MonitoringWeb.Models
{
    public class MeteringDevicesForCheckFilterModel
    {
        [Display(Name = "Дом для проверки")]
        public string House { get; set; }

        [Display(Name = "В течение ближайших дней")]
        public int Days { get; set; }
    }
}