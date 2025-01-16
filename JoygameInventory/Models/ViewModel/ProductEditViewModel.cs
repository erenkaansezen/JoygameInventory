using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string ProductBarkod { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime ProductAddDate { get; set; }
        public string Status { get; set; } = null!;
        public int SelectedUserId { get; set; }

        
        public List<JoyStaff> JoyStaffs { get; set; } // Önceki atamalar
        public IEnumerable<InventoryAssigment>? InventoryAssigments { get; set; }


    }




}
