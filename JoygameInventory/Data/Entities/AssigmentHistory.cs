namespace JoygameInventory.Data.Entities
{
    public class AssigmentHistory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public JoyStaff? User { get; set; }

        public DateTime AssignmentDate { get; set; }


    }
}
