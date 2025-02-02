namespace JoygameInventory.Data.Entities
{
    public class Licence
    {
        public int Id { get; set; }
        public string LicenceName { get; set; }
        public DateTime LicenceActiveDate { get; set; }
        public DateTime LicenceEndDate { get; set; }
        public ICollection<LicenceUser> LicenceUser { get; set; }

    }
}
