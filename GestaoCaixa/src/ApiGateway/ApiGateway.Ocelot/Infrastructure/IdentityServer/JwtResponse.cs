namespace ApiGateway.Ocelot.Infrastructure.IdentityServer
{
    public class JwtResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
    }
}
