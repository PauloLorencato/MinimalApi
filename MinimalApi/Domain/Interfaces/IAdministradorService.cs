using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces
{
    public interface IAdministradorService
    {
        Administrador? Login(LoginDTO loginDTO);
        Administrador? Incluir(Administrador administrador);
        Administrador? BuscaPorId(int id);
        List<Administrador>? Todos(int? pagina);
    }
}