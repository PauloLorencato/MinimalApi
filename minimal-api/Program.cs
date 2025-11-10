using System.IO.Pipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) =>
{
    if (administradorService.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>
{
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculoService.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
});
app.MapGet("/veiculos", ([FromQuery]int? pagina, IVeiculoService veiculoService) =>
{
    var veiculos = veiculoService.Todos(pagina);
    return Results.Ok(veiculos);
});
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion