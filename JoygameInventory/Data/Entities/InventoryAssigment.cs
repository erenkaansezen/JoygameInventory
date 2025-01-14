namespace JoygameInventory.Data.Entities
{
    public class InventoryAssigment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public JoyUser User { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Status { get; set; }

    }
}
