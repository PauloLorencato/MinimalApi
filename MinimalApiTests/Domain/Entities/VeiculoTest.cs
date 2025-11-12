using MinimalApi.Domain.Entities;

namespace MinimalApiTests.Domain.Entities
{
    [TestClass]
    public class VeiculoTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var veiculo = new Veiculo();

            // Act
            veiculo.Id = 1;
            veiculo.Marca = "teste";
            veiculo.Nome = "teste";
            veiculo.Ano = 2000;

            // Assert
            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("teste", veiculo.Marca);
            Assert.AreEqual("teste", veiculo.Nome);
            Assert.AreEqual(2000, veiculo.Ano);
        }
    }
}