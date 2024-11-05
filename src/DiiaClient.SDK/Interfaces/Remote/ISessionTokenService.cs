namespace DiiaClient.SDK.Interfaces.Remote
{
    public interface ISessionTokenService
    {
        Task<string> GetSessionToken();
    }
}
