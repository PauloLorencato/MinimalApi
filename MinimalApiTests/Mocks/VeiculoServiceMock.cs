using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;

namespace MinimalApiTests.Mocks
{
    public class VeiculoServiceMock : IVeiculoService
    {
        private static List<Veiculo> veiculos = new List<Veiculo>(){
        new Veiculo{
            Id = 1,
            Nome = "Carro A",
            Marca = "Marca A",
            Ano = 2020
        },
        new Veiculo{
            Id = 2,
            Nome = "Carro B",
            Marca = "Marca B",
            Ano = 2021
        }
    };
        public void Apagar(Veiculo veiculo)
        {
            veiculos.Remove(veiculo);
        }

        public void Atualizar(Veiculo veiculo)
        {
            veiculos[veiculo.Id - 1] = veiculo;
        }

        public Veiculo? BuscaPorId(int id)
        {
            return veiculos.Find(v => v.Id == id);
        }

        public void Incluir(Veiculo veiculo)
        {
            veiculo.Id = veiculos.Count() + 1;
            veiculos.Add(veiculo);
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marga = null)
        {
            return veiculos;
        }
    }
}