using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.Entities;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly DbContexto _context;

        public VeiculoService(DbContexto context)
        {
            _context = context;
        }

        public void Apagar(Veiculo veiculo)
        {
            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _context.Veiculos.Update(veiculo);
            _context.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _context.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }

        public List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marga = null)
        {
            var query = _context.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(x => EF.Functions.Like(x.Nome.ToLower(), $"%{nome.ToLower()}%"));
            }

            int itensPorPagina = 10;

            query = query.Skip(pagina - 1 * itensPorPagina).Take(itensPorPagina);

            return query.ToList();
        }
    }
}