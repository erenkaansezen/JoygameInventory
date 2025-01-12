using Microsoft.AspNetCore.Identity;

namespace JoygameInventory.Data.Entities
{
    public class JoyUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
         
        public ICollection<InventoryAssigment> InventoryAssigments { get; set; }

    }
}
