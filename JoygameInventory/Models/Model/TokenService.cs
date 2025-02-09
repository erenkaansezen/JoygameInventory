using Flurl.Http;
using JoygameInventory.Models.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class TokenService : ITokenService
{
    private readonly IOptionsMonitor<RelatedDigitalEmailSettings> _emailSettings;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptionsMonitor<RelatedDigitalEmailSettings> emailSettings, ILogger<TokenService> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
    }

    // Token almak için kullanılan metod
    public async Task<RelatedDigitalTokenResponse> GetToken(RelatedDigitalTokenRequest tokenRequest)
    {
        try
        {
            // API'ye token almak için istek gönder
            var response = await _emailSettings.CurrentValue.AuthServiceURL
                .AllowAnyHttpStatus()
                .PostJsonAsync(tokenRequest)
                .ReceiveJson<RelatedDigitalTokenResponse>();

            // Eğer token alındıysa, başarıyla döndür
            if (response != null && response.Success)
            {
                return response;
            }

            _logger.LogError("Token alınamadı: " + JsonConvert.SerializeObject(response));
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token alırken hata oluştu.");
            return null;
        }
    }
}
