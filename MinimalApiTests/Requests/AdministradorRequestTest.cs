using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.DTOs;
using MinimalApiTests.Helpers;
using MinimalApi.Domain.Enums;
using MinimalApi.Domain.Entities;

namespace MinimalApiTests.Requests;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    private async Task<string> GetToken()
    {
        var loginDTO = new LoginDTO{
            Email = "adm@teste.com",
            Senha = "123456"
        };
        var response = await Setup.client.PostAsync("/administradores/login", new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return admLogado?.Token ?? "";
    }
    
    [TestMethod]
    public async Task LoginRequest()
    {
        // Arrange
        var loginDTO = new LoginDTO{
            Email = "adm@teste.com",
            Senha = "123456"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Perfil ?? "");
        Assert.IsNotNull(admLogado?.Token ?? "");

        Console.WriteLine(admLogado?.Token);
    }

    [TestMethod]
    public async Task CriarAdmRequest()
    {
        // Arrange
        var novoAdm = new AdministradorDTO{
            Email = "admCriado@teste.com",
            Senha = "123456",
            Perfil = PerfilEnum.Adm
        };

        var content = new StringContent(JsonSerializer.Serialize(novoAdm), Encoding.UTF8,  "Application/json");
        Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.PostAsync("/administradores", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admCriado = JsonSerializer.Deserialize<Administrador>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admCriado?.Email ?? "");
        Assert.IsNotNull(admCriado?.Perfil ?? "");
    }

    [TestMethod]
    public async Task GetAdmsPorPaginaRequest()
    {
        // Arrange
        Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.GetAsync("/administradores/");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var listaAdms = JsonSerializer.Deserialize<List<AdministradorModelView>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(listaAdms);
        Assert.IsTrue(listaAdms!.Count > 0);
        Assert.IsInstanceOfType(listaAdms[0], typeof(AdministradorModelView));
    }

    [TestMethod]
    public async Task GetAdmPorIdRequest()
    {
        // Arrange
        Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.GetAsync("/administradores/1");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admEncontrado = JsonSerializer.Deserialize<AdministradorModelView>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admEncontrado);
        Assert.IsInstanceOfType(admEncontrado, typeof(AdministradorModelView));
    }
}