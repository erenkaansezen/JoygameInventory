using JoygameInventory.Models.Model;
using Microsoft.Extensions.Options;

public class TokenStorage
{
    private RelatedDigitalTokenResponse _token;
    private readonly IOptionsMonitor<RelatedDigitalEmailSettings> _emailSettings;
    private readonly ITokenService _tokenService;
    private const int RelatedDigitalDefaultTokenExpireHour = 2;

    public TokenStorage(IOptionsMonitor<RelatedDigitalEmailSettings> emailSettings, ITokenService tokenService)
    {
        _emailSettings = emailSettings;
        _tokenService = tokenService;
    }

    public string AccessToken
    {
        get
        {
            if (_token == null || DateTime.UtcNow.AddSeconds(60) > _token.ExpireDate) // Token süresi bitmişse, yenile
            {
                _token = GetTokenAsync().Result; // Yeni token al
            }
            return _token?.ServiceTicket;
        }
    }

    public async Task<RelatedDigitalTokenResponse> GetTokenAsync()
    {
        var tokenRequest = new RelatedDigitalTokenRequest
        {
            UserName = _emailSettings.CurrentValue.ApiUserName,
            Password = _emailSettings.CurrentValue.ApiPassword
        };

        var tokenResponse = await _tokenService.GetToken(tokenRequest);
        tokenResponse.ExpireDate = DateTime.Now.AddHours(RelatedDigitalDefaultTokenExpireHour); // Token'ı 2 saat geçerli olarak belirle
        return tokenResponse;
    }
}
