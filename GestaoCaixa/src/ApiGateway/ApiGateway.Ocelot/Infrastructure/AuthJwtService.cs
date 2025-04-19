using System.Security.Claims;
using System.Text;

namespace ApiGateway.Ocelot.Infrastructure
{
    public class AuthJwtService
    {
        //public string GenerateJwt()
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_products_api_secret"));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        //    var expirationDate = DateTime.UtcNow.AddHours(2);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, user.Username.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };
        //}
    }
}
