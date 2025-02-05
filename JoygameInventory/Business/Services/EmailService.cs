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

    public EmailService(HttpClient httpClient, IOptions<RelatedDigitalEmailSettings> emailSettings)
    {
        _httpClient = httpClient;
        _emailSettings = emailSettings.Value;
    }

    // Token'ı alır
    public async Task<string> AuthenticateAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken))
        {
            return _accessToken; // Eğer token geçerliyse, hemen geri döner.
        }

        var authData = new
        {
            // Burada gerekli kimlik doğrulama verilerinizi doldurun
            UserName = "zula_live_wsuser",
            Password = "4CC92744"
        };

        var content = new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json");

        // Authentication API URL'si
        var authRequest = new HttpRequestMessage(HttpMethod.Post, _emailSettings.AuthServiceURL)
        {
            Content = content
        };

        // Eğer varsa başlıkları da ekleyebilirsiniz

        var response = await _httpClient.SendAsync(authRequest);

        

        var responseContent = await response.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

        if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
        {
            throw new Exception("Authentication failed, no access token returned.");
        }

        _accessToken = authResponse.AccessToken;
        return _accessToken;
    }

    // E-posta gönderme işlemi
    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        var token = await AuthenticateAsync(); // Geçerli token'ı al

        var emailData = new
        {
            FromName = _emailSettings.FromName,
            FromAddress = _emailSettings.FromAddress,
            ReplyAddress = _emailSettings.ReplyAddress,
            ToAddress = toAddress,
            Subject = subject,
            Body = body,
            IsHtml = true
        };

        var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, _emailSettings.PostHtmlURL)
        {
            Content = content
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); // Token'ı başlığa ekle

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to send email. Status code: {response.StatusCode}. Response: {errorContent}");
        }
    }
}

public class AuthResponse
{
    public string AccessToken { get; set; }
}
