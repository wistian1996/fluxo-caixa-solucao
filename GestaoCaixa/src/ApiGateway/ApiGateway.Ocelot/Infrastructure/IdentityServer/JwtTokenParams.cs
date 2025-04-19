namespace ApiGateway.Ocelot.Infrastructure.IdentityServer
{
    public class JwtTokenParams
    {
        public const int TimeExpirationTokenInHours = 1;
        public const string Audience = "aud-identity-server-debug";
        public const string Issuer = "https://identity-server-debug.com";
    }
}
