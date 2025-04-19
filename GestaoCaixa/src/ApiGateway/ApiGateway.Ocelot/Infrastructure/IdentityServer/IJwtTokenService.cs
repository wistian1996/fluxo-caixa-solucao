using ApiGateway.Ocelot.Models;

namespace ApiGateway.Ocelot.Infrastructure.IdentityServer
{
    public interface IJwtTokenService
    {
        JwtResponse GenerateTokenUsingRsa(LoginRequest loginRequest);
    }
}
