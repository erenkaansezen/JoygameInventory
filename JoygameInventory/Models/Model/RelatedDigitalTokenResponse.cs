namespace JoygameInventory.Models.Model
{
    public class RelatedDigitalTokenResponse
    {
        public bool Success { get; set; }
        public string ServiceTicket { get; set; }
        public DateTime ExpireDate { get; set; }  // Token'ın geçerlilik süresi
    }

}
