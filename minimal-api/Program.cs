using Microsoft.AspNetCore.Mvc;
using minimal_api;
var builder = WebApplication.CreateBuilder(args);
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