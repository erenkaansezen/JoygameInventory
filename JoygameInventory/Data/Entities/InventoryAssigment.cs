namespace JoygameInventory.Data.Entities
{
    public class InventoryAssigment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public JoyStaff User { get; set; }
        public int? PreviusAssigmenId { get; set; }
        public JoyStaff PreviusAssigmentUserNavigation { get; set; } // Bu kullanıcıyı temsil edecek navigasyon özelliği

        public DateTime AssignmentDate { get; set; }

        public ICollection<AssigmentHistory> AssigmentHistorys { get; set; }


    }
}
