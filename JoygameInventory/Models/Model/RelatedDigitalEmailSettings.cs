namespace JoygameInventory.Models.Model
{
    public class RelatedDigitalEmailSettings
    {
        public string AuthServiceURL { get; set; } // API kimlik doğrulama URL'si
        public string PostHtmlURL { get; set; } // E-posta gönderimi için URL
        public string FromName { get; set; } // Gönderenin adı
        public string FromAddress { get; set; } // Gönderen e-posta adresi
        public string ReplyAddress { get; set; } // Yanıtlanacak e-posta adresi
        public string ApiUserName { get; set; } // API kullanıcı adı
        public string ApiPassword { get; set; } // API şifre
    }

}
