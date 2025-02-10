using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class MaintenanceHistory
    {
        [Key]
        public int Id { get; set; } // Maintenance ID
        public string MaintenanceDescription { get; set; } // Servis açıklaması
        public DateTime CreatedAt { get; set; } // Servis oluşturulma tarihi
        public DateTime EndDate { get; set; } // Servisin bitiş tarihi

        // Hangi ürüne ait olduğunu belirtiyoruz
        public string ProductBarkod { get; set; }

        // Servisin ilişkilendirildiği ürün
        public Product Product { get; set; }
    }
}
