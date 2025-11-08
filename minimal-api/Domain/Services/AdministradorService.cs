using System.Data.Common;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services;

public class AdministradorService : IAdministradorService
{
    private readonly DbContexto _context;

    public AdministradorService(DbContexto context)
    {
        _context = context;
    }
    
    public Administrador? Login(LoginDTO loginDTO)
    {
        var adm = _context.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }
}