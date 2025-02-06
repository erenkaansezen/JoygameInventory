using JoygameInventory.Models.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

public class EmailService
{
    private readonly HttpClient _httpClient;
    private readonly RelatedDigitalEmailSettings _emailSettings;
    private string _jwtToken;
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

        // Başarısız yanıtı logla
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Authentication failed. Status code: {response.StatusCode}. Response: {errorContent}");
            throw new Exception($"Authentication failed. Status code: {response.StatusCode}. Response: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Authentication response: {responseContent}");

        try
        {
            // Yanıtı deserialize et
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

            // Yanıt başarılı ise token al
            if (authResponse != null && authResponse.Success && !string.IsNullOrEmpty(authResponse.ServiceTicket))
            {
                _jwtToken = authResponse.ServiceTicket;  // ServiceTicket'ı JWT token olarak alıyoruz
                _tokenExpirationTime = DateTime.Now.AddMinutes(30); // Token 30 dakika geçerli
                return _jwtToken;
            }
            else
            {
                Console.WriteLine($"Authentication failed: {authResponse?.DetailedMessage}");
                throw new Exception($"Authentication failed: {authResponse?.DetailedMessage}");
            }
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Error parsing JSON response: {jsonEx.Message}");
            throw new Exception($"Error parsing JSON response: {jsonEx.Message}");
        }
    }

    // Token geçerliliğini kontrol et
    private bool IsTokenValid()
    {
        // Eğer token yoksa veya süresi dolmuşsa, yeni bir token alıyoruz
        return !string.IsNullOrEmpty(_jwtToken) && DateTime.Now < _tokenExpirationTime;
    }

    // Token'ı doğrula veya yenile
    public async Task<string> GetValidTokenAsync()
    {
        // Eğer token geçerli değilse, kimlik doğrulama işlemi yapıyoruz
        if (!IsTokenValid())
        {
            await AuthenticateAsync();
        }
        return _jwtToken;
    }

    // E-posta gönderme işlemi
    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        // Geçerli bir token al
        var token = await GetValidTokenAsync();

        var emailData = new
        {
            FromName = _emailSettings.FromName,
            FromAddress = _emailSettings.FromAddress,
            ReplyAddress = _emailSettings.ReplyAddress,
            ToAddress = toAddress,
            Subject = subject,
            Body = body,
            AuthToken = token, // JWT token'ı burada kullanıyoruz
            IsHtml = true
        };

        var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, _emailSettings.PostHtmlURL)
        {
            Content = content
        };

        var response = await _httpClient.SendAsync(request);



        // Başarı durumunda ek bilgi yazdırma (isteğe bağlı)
        Console.WriteLine($"Email sent successfully to {toAddress}. Subject: {subject}");
    }
}

// Kimlik doğrulama yanıtını tutan sınıf
public class AuthResponse
{
    public string ServiceTicket { get; set; }  // Token burada alınıyor
    public bool Success { get; set; }
    public string DetailedMessage { get; set; }
    public string TransactionId { get; set; }
}
