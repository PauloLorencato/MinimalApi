using System.Data.Common;
using Microsoft.EntityFrameworkCore;
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

    public Administrador? Incluir(Administrador administrador)
    {
        _context.Administradores.Add(administrador);
        _context.SaveChanges();
        return administrador;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        var adm = _context.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }

    public Administrador? BuscaPorId(int id)
    {
        return _context.Administradores.Where(v => v.Id == id).FirstOrDefault();
    }

    public List<Administrador>? Todos(int? pagina)
    {
        var query = _context.Administradores.AsQueryable();

        int itensPorPagina = 10;

        if (pagina != null)
        {
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
        }

        return query.ToList();
    }
}