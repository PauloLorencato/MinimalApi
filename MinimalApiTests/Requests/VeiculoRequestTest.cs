using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ModelViews;
using MinimalApiTests.Helpers;

namespace MinimalApiTests.Requests;

[TestClass]
public class VeiculoRequestTest
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
        var loginDTO = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "123456"
        };
        var response = await Setup.client.PostAsync("/administradores/login", new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json"));
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return admLogado?.Token ?? "";
    }

    [TestMethod]
    public async Task AdicionaVeiculoRequest()
    {
        // Arrange
        var veiculo = new VeiculoDTO
        {
            Nome = "Corolla",
            Marca = "Toyota",
            Ano = 2020,
        };
        if (!Setup.client.DefaultRequestHeaders.Contains("Authorization"))
            Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        var content = new StringContent(JsonSerializer.Serialize(veiculo), Encoding.UTF8, "Application/json");

        // Act
        var response = await Setup.client.PostAsync("/veiculos", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var veiculoCriado = JsonSerializer.Deserialize<Veiculo>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(veiculoCriado?.Nome);
        Assert.IsNotNull(veiculoCriado?.Marca);
    }

    [TestMethod]
    public async Task GetVeiculoPorId()
    {
        // Arrange
        if (!Setup.client.DefaultRequestHeaders.Contains("Authorization"))
            Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.GetAsync("/veiculos/1");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var veiculo = JsonSerializer.Deserialize<Veiculo>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.AreEqual(1, veiculo?.Id);
        Assert.IsNotNull(veiculo?.Nome);
        Assert.IsNotNull(veiculo?.Marca);
    }

    [TestMethod]
    public async Task GetVeiculosPorPaginaRequest()
    {
        // Arrange
        if (!Setup.client.DefaultRequestHeaders.Contains("Authorization"))
            Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.GetAsync("/veiculos");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var listaVeiculos = JsonSerializer.Deserialize<List<Veiculo>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(listaVeiculos);
        Assert.IsTrue(listaVeiculos!.Count > 0);
        Assert.IsInstanceOfType(listaVeiculos[0], typeof(Veiculo));
    }

    [TestMethod]
    public async Task AtualizarVeiculoRequest()
    {
        // Arrange
        var veiculo = new VeiculoDTO
        {
            Nome = "Civic",
            Marca = "Honda",
            Ano = 2021,
        };

        var content = new StringContent(JsonSerializer.Serialize(veiculo), Encoding.UTF8, "Application/json");

        if (!Setup.client.DefaultRequestHeaders.Contains("Authorization"))
            Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.PutAsync("/veiculos/1", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var veiculoAtualizado = JsonSerializer.Deserialize<Veiculo>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(veiculoAtualizado);
        Assert.IsInstanceOfType(veiculoAtualizado, typeof(Veiculo));
    }

    [TestMethod]
    public async Task ApagarVeiculoRequest()
    {
        // Arrange
        if (!Setup.client.DefaultRequestHeaders.Contains("Authorization"))
            Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetToken()}");

        // Act
        var response = await Setup.client.DeleteAsync("/veiculos/2");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }
}