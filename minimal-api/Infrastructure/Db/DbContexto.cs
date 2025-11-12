using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Infrastructure.Db
{
    public class DbContexto : DbContext
    {
    private readonly IConfiguration? _config;
        public DbContexto(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Administrador> Administradores { get; set; } = default;
        public DbSet<Veiculo> Veiculos { get; set; } = default;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>().HasData(
                new Administrador {
                    Id = 1,
                    Email = "Administrador@teste.com",
                    Senha = "123456",
                    Perfil = "Adm"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _config?.GetConnectionString("MySql")?.ToString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString));

                }
            }
        }
    }
}