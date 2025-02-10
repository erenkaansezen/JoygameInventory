using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class Product
    {
        [Key]

        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string ProductBarkod { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime ProductAddDate { get; set; }

        public string? Status { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<Category> Categories { get; set; }

        public ICollection<InventoryAssigment> InventoryAssigments { get; set; }
        public ICollection<AssigmentHistory> AssigmentHistorys { get; set; }
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>(); // Servisler koleksiyonu
        public ICollection<MaintenanceHistory> MaintenanceHistory { get; set; } = new List<MaintenanceHistory>(); // Servisler koleksiyonu




    }
}
