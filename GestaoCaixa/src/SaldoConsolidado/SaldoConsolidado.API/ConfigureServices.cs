using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SaldoConsolidado.API.Infrastructure.IdentityServer;
using SaldoConsolidado.Application.Infrastructure.IdentityServer;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace SaldoConsolidado.API
{
    public static class ConfigureServices
    {
        public static void AddApplicationAuthentication(this IServiceCollection serviceCollection)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = JwtTokenParams.Issuer;
                    options.Audience = JwtTokenParams.Audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtTokenParams.Issuer,

                        ValidateAudience = true,
                        ValidAudience = JwtTokenParams.Audience,

                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                        {
                            var key = IdentityServerJwksService.GetKeyJwksById(kid);

                            if (key != null)
                            {
                                return [key];
                            }

                            return [];
                        },
                    };
                });

            serviceCollection.AddAuthorization();
        }
    }
}
