namespace JoygameInventory.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime ProductAddDate { get; set; }
        public string img { get; set; } = string.Empty;

        public ICollection<InventoryAssigment> InventoryAssigments { get; set; }


    }
}
