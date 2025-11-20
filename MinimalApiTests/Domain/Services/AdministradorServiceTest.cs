using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;

namespace MinimalApiTests.Domain.Services
{
    [TestClass]
    public class AdministradorServiceTest
    {
        private DbContexto CriarContextoDeTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));
            System.Console.WriteLine(path);

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DbContexto(configuration);
        }
        [TestMethod]
        public void TestSalvarAdministrador()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var adm = new Administrador();
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorService = new AdministradorService(context);

            // Act
            administradorService.Incluir(adm);

            // Assert
            Assert.AreEqual(1, administradorService.Todos(1).Count());
        }

        [TestMethod]
        public void TestBuscarPorId()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var adm = new Administrador();
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorService = new AdministradorService(context);

            // Act
            administradorService.Incluir(adm);
            var admDb = administradorService.BuscaPorId(adm.Id);

            // Assert
            Assert.AreEqual(1, admDb.Id);
        }

        [TestMethod]
        public void TestLogin()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var adm = new Administrador();
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorService = new AdministradorService(context);

            // Act
            administradorService.Incluir(adm);
            var admDb = administradorService.Login(new LoginDTO
            {
                Email = "teste@teste.com",
                Senha = "teste"
            });

            // Assert
            Assert.IsNotNull(admDb);
            Assert.IsInstanceOfType(admDb, typeof(Administrador));
        }
    }
}