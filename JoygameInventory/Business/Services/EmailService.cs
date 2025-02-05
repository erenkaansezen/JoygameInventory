using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JoygameInventory.Models.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class EmailService
{
    private readonly HttpClient _httpClient;
    private readonly RelatedDigitalEmailSettings _emailSettings;
    private string _accessToken;
    private DateTime _tokenExpirationTime;

    public EmailService(HttpClient httpClient, IOptions<RelatedDigitalEmailSettings> emailSettings)
    {
        _httpClient = httpClient;
        _emailSettings = emailSettings.Value;
    }

    // Kimlik doğrulama işlemi
    public async Task<string> AuthenticateAsync()
    {
        var authData = new
        {
            UserName = _emailSettings.ApiUserName,
            Password = _emailSettings.ApiPassword
        };

        var content = new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_emailSettings.AuthServiceURL, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Authentication failed. Status code: {response.StatusCode}. Response: {errorContent}");
        }

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());

        if (authResponse.Success && !string.IsNullOrEmpty(authResponse.ServiceTicket))
        {
            // Token'ı saklıyoruz ve süresini belirliyoruz
            _accessToken = authResponse.ServiceTicket;
            _tokenExpirationTime = DateTime.UtcNow.AddMinutes(30); // Örneğin 30 dakika geçerli
            return _accessToken;
        }

        throw new Exception("Authentication failed. No valid service ticket returned.");
    }

    // Token geçerliliğini kontrol et
    private bool IsTokenValid()
    {
        // Eğer token yoksa veya süresi dolmuşsa, yeni bir token alıyoruz
        return !string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpirationTime;
    }

    // E-posta gönderme işlemi
    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        if (!IsTokenValid())
        {
            // Token geçersizse veya yoksa, kimlik doğrulama yapıyoruz
            await AuthenticateAsync();
        }

        var emailData = new
        {
            FromName = _emailSettings.FromName,
            FromAddress = _emailSettings.FromAddress,
            ReplyAddress = _emailSettings.ReplyAddress,
            ToAddress = toAddress,
            Subject = subject,
            Body = body,
            AuthToken = _accessToken,


            IsHtml = true
        };

        var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, _emailSettings.PostHtmlURL)
        {
            Content = content
        };

        // Token'ı başlığa ekle

        var response = await _httpClient.SendAsync(request);

        // Hata kontrolü
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorMessage = $"{response.StatusCode}{errorContent}";
            Console.WriteLine(errorMessage);  // Detaylı hata loglaması
            throw new Exception(errorMessage);
        }

        // Başarı durumunda ek bilgi yazdırma (isteğe bağlı)
        Console.WriteLine($"Email sent successfully to {toAddress}. Subject: {subject}");
    }
}

// Kimlik doğrulama yanıtını tutan sınıf
public class AuthResponse
{
    public string ServiceTicket { get; set; }
    public bool Success { get; set; }
    public string DetailedMessage { get; set; }
    public string TransactionId { get; set; }
}
