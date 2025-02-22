using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class MaintenanceViewModel
    {
        public int MaintenanceId { get; set; } // Maintenance ID
        public string MaintenanceDescription { get; set; } // Servis açıklaması
        public DateTime MaintenanceCreated { get; set; } // Servis oluşturulma tarihi
        public DateTime? MaintenanceEndDate { get; set; } // Servisin bitiş tarihi

        public string ProductBarkod { get; set; }

        // Hangi ürüne ait olduğunu belirtiyoruz
        public string SelectedProductBarkod { get; set; }

        // Servisin ilişkilendirildiği ürün
        public ICollection<Product> Products { get; set; }


    }




}
