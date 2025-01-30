using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class Servers
    {
        [Key]
        public int Id { get; set; }
        public string? ServerName { get; set; }
        public string? IPAddress { get; set; }
        public string? MACAddress { get; set; }
        public string? OperatingSystem { get; set; }
        public string? CPU { get; set; }
        public int RAM { get; set; }
        public int Storage { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public DateTime DateInstalled { get; set; }
        public string? HostName { get; set; }
        public string? SerialNumber { get; set; }
        public string? NetworkInterface { get; set; }
        public string? PowerStatus { get; set; }
        public string? BackupStatus { get; set; }






    }
}
