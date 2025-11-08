using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorService, AdministradorService>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    var cs = builder.Configuration.GetConnectionString("mysql");
    // Avoid AutoDetect at design-time which attempts a DB connection; use a fixed server version instead.
    options.UseMySql(
        cs,
        ServerVersion.Parse("8.0.33-mysql")
    );
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) =>
{
    if (administradorService.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});


app.Run();