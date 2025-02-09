namespace JoygameInventory.Models.Model
{
    public interface ITokenService
    {
        Task<RelatedDigitalTokenResponse> GetToken(RelatedDigitalTokenRequest tokenRequest);
    }


}
