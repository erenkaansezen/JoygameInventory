using Flurl.Http;
using JoygameInventory.Models.Model;
using Microsoft.Extensions.Options;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IOptionsMonitor<RelatedDigitalEmailSettings> _emailSettings;
    private readonly ITokenService _tokenService;
    private readonly TokenStorage _tokenStorage; // TokenStorage sınıfını ekleyin

    public EmailService(ILogger<EmailService> logger,
                        IOptionsMonitor<RelatedDigitalEmailSettings> emailSettings,
                        ITokenService tokenService,
                        TokenStorage tokenStorage) // TokenStorage dependency injection ile alınıyor
    {
        _logger = logger;
        _emailSettings = emailSettings;
        _tokenService = tokenService;
        _tokenStorage = tokenStorage;
    }

    public string ConvertFileToBase64(string filePath)
    {
        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        return Convert.ToBase64String(fileBytes);
    }
    // E-posta gönderme metodu
    public async Task<bool> SendEmailAsync(string to, string body, string subject, List<string> attachmentPaths)
    {
        try
        {
            var token = _tokenStorage.AccessToken;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Geçerli bir token alınamadı.");
                return false;
            }

            // Ekleri oluştur
            var attachments = new List<Attachment>();

            if (attachmentPaths != null && attachmentPaths.Count > 0)
            {
                foreach (var path in attachmentPaths)
                {
                    // Dosyayı Base64 formatına çevir
                    var fileContentBase64 = ConvertFileToBase64(path);

                    // Ek bilgilerini oluştur
                    var attachment = new Attachment
                    {
                        Name = Path.GetFileName(path),  // Dosya adı
                        Type = "pdf",  // Dosya türünü değiştirebilirsiniz (örneğin, "pdf", "docx")
                        Content = fileContentBase64
                    };

                    attachments.Add(attachment);
                }
            }

            // E-posta gönderim isteği oluştur
            var emailRequest = new RelatedDigitalEmailRequest
            {
                Subject = subject,
                HtmlBody = body,
                ToName = to,
                ToEmailAddress = to,
            };

            // E-posta gönderimi için API'yi çağır
            return await SendRelatedDigitalEmailAsync(emailRequest, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "E-posta gönderme hatası.");
            return false;
        }
    }


    // E-posta gönderimi için ilgili API'yi çağıran metot
    private async Task<bool> SendRelatedDigitalEmailAsync(RelatedDigitalEmailRequest emailRequest, string token)
    {
        try
        {
            var response = await _emailSettings.CurrentValue.PostHtmlURL
                .AllowAnyHttpStatus()
                .WithHeader("Authorization", token)
                .PostJsonAsync(emailRequest)
                .ReceiveJson<RelatedDigitalEmailResponse>();

            if (response.Success)
            {
                _logger.LogInformation($"E-posta başarıyla gönderildi: {emailRequest.ToEmailAddress}");
                return true;
            }
            else
            {
                _logger.LogError($"E-posta gönderimi başarısız: {response.Message}");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "E-posta gönderme hatası.");
            return false;
        }
    }


    // E-posta gönderimi için API'yi çağıran metod
    private async Task<bool> PostEmailWithRelatedDigitalAsync(RelatedDigitalEmailRequest emailRequest, string token)
    {
        try
        {
            // API'ye e-posta gönderme isteği gönder
            var response = await _emailSettings.CurrentValue.PostHtmlURL
                .AllowAnyHttpStatus()
                .WithHeader("Authorization", token)
                .PostJsonAsync(emailRequest)
                .ReceiveJson<RelatedDigitalEmailResponse>();

            // Eğer e-posta gönderimi başarılı ise
            if (response.Success)
            {
                _logger.LogInformation($"E-posta başarıyla gönderildi: {emailRequest.ToEmailAddress}");
                return false;
            }
            else
            {
                // Eğer gönderim başarısız olduysa, hatayı logla
                _logger.LogError($"E-posta gönderimi başarısız: {response.Message}");
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API ile e-posta gönderme hatası.");
            return false;
        }
    }
}
