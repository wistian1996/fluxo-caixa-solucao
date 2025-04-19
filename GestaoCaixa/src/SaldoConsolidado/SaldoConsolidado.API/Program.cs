using MediatR;
using Microsoft.AspNetCore.Authorization;
using SaldoConsolidado.API;
using SaldoConsolidado.Application;
using SaldoConsolidado.Application.Features.Saldo;
using SaldoConsolidado.Domain.Interfaces;
using SaldoConsolidado.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddApplicationAuthentication();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/api/v1/comerciante/{comercianteId:Guid}/saldo", [AllowAnonymous] async (Guid comercianteId, ISaldoConsolidadoDiarioRepository repository) =>
{
    return await repository.GetSaldoDiario(comercianteId, DateTime.Now);
})
.WithOpenApi();

app.MapGet("/api/v1/comerciante/saldo", [Authorize] async (HttpContext httpContext, IMediator mediator) =>
{
    var userId = httpContext.User.FindFirst(System.IdentityModel
        .Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

    return await mediator.Send(new GetSaldoPorComercianteIdQuery(Guid.Parse(userId!)));
})
.WithOpenApi();

app.Run();