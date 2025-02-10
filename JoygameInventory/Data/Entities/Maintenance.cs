using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; } // Maintenance ID
        public string MaintenanceDescription { get; set; } // Servis açıklaması
        public DateTime CreatedAt { get; set; } // Servis oluşturulma tarihi

        public string? ServiceTitle { get; set; }

        public string? ServiceAdress { get; set; }

        // Hangi ürüne ait olduğunu belirtiyoruz
        public string ProductBarkod { get; set; }

        // Servisin ilişkilendirildiği ürün
        public Product Product { get; set; }
    }
}
