using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.DTOs;
using minimal_api.Infrastructure.Db;
var builder = WebApplication.CreateBuilder(args);

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

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "admin@teste.com" && loginDTO.Senha == "123456")
    {
        return Results.Ok();
    }
    else
        return Results.Unauthorized();
});


app.Run();