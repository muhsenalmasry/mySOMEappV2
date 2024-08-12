namespace mySOMEappV2.Authenticators
{
    public interface IJwtTokenManager
    {
        string Authenticate(string email, string password);
    }
}
