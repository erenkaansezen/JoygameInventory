using Microsoft.AspNetCore.Identity;

namespace JoygameInventory.Data.Entities
{
    public class JoyStaff 
    {
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string UserName { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Document { get; set; }

        public ICollection<InventoryAssigment> InventoryAssigments { get; set; }
    }
}
