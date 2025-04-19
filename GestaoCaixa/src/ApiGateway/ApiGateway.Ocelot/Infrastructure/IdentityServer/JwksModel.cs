using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Ocelot.Infrastructure.IdentityServer
{
    public class JwksModel
    {
        public JsonWebKey[] Keys { get; set; }
    }
}