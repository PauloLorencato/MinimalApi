using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces;

public interface IVeiculoService
{
    List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marga = null);
    Veiculo? BuscaPorId(int id);
    void Incluir(Veiculo veiculo);
    void Atualizar(Veiculo veiculo);
    void Apagar(Veiculo veiculo);
}