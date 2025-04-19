using ApiGateway.Ocelot.Infrastructure.IdentityServer;
using ApiGateway.Ocelot.Middlewares;
using ApiGateway.Ocelot.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new RsaSecurityKey(JwtRsaSecurityService.GetRsaPublicKeyAsPem()),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

var ocelotConfiguration = $"configuration.{builder.Environment.EnvironmentName}.json";

builder.Configuration.AddJsonFile(ocelotConfiguration,
    optional: false, reloadOnChange: true);

builder.Services.AddOcelot();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", p =>
    {
        p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UsePreflightRequestHandler();


app.UseSerilogRequestLogging();

app.MapPost("/identity-server/auth/token", [AllowAnonymous] (
    [FromBody] LoginRequest loginRequest, IJwtTokenService jwtTokenService) =>
{
    return jwtTokenService.GenerateTokenUsingRsa(loginRequest);
});

app.MapGet("/identity-server/.well-known/jwks.json", [AllowAnonymous] () =>
{
    return JwtRsaSecurityService.GetRsaPublicKeyAsJwks();
});

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), subApp =>
{
    //subApp.UseAuthentication();

    //subApp.UseAuthorization();

    subApp.UseOcelot().Wait();
});

app.Run();
