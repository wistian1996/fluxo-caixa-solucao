using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ApiGateway.Ocelot.Models;
using System.Security.Claims;

namespace ApiGateway.Ocelot.Infrastructure.IdentityServer
{
    public class JwtTokenService : IJwtTokenService
    {
        public JwtResponse GenerateTokenUsingRsa(LoginRequest loginRequest)
        {
            var key = JwtRsaSecurityService.GetRsaSecurityKey();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            var expirationDate = DateTime.UtcNow.AddHours(JwtTokenParams.TimeExpirationTokenInHours);
            var expirationDateUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var issuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, "TESTANDO JWT"),
                new Claim(JwtRegisteredClaimNames.Sub, loginRequest.ComercianteId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                audience: JwtTokenParams.Audience,
                issuer: JwtTokenParams.Issuer,
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials);

            var authToken = new JwtResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = (int)TimeSpan.FromHours(JwtTokenParams.TimeExpirationTokenInHours).TotalSeconds
            };

            return authToken;
        }

        //public AuthToken GenerateToken(AuthUser user)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_categories_api_secret"));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        //    var expirationDate = DateTime.UtcNow.AddHours(2);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, user.Username.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    var token = new JwtSecurityToken(audience: "categoriesAudience",
        //                                      issuer: "categoriesIssuer",
        //                                      claims: claims,
        //                                      expires: expirationDate,
        //                                      signingCredentials: credentials);

        //    var authToken = new AuthToken();
        //    authToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
        //    authToken.ExpirationDate = expirationDate;

        //    return authToken;
        //}
    }
}
