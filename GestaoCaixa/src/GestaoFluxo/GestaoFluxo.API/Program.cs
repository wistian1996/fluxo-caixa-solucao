using Asp.Versioning;
using GestaoFluxo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using GestaoFluxo.Application;
using GestaoFluxo.Infrastructure;
using GestaoFluxo.API.Configurations;
using Serilog;
using FluentValidation;
using System;
using GestaoFluxo.API;
using System.Reflection.PortableExecutable;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();

var connection = builder.Configuration.GetConnectionString("Mysql");
builder.Services.AddDbContext<GestaoFluxoDbContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

builder.Services.AddInfrastructure();
builder.Services.AddApplicationAuthentication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
});

//var openTelemetryBuilder = builder.Services.AddOpenTelemetry();

//openTelemetryBuilder.ConfigureResource(resource => resource
//    .AddService(builder.Environment.ApplicationName));

//openTelemetryBuilder.WithMetrics(metrics => metrics
//    .AddAspNetCoreInstrumentation()
//    .AddConsoleExporter());

//openTelemetryBuilder.WithTracing(builder => builder
//    .AddAspNetCoreInstrumentation()
//    .AddConsoleExporter());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GestaoFluxoDbContext>();

    var pendingMigrations = db.Database.GetPendingMigrations();

    if (pendingMigrations.Any())
    {
        db.Database.Migrate();
    }
}

app.Run();
