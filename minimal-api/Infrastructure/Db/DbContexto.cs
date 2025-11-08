using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.Entities;

namespace minimal_api.Infrastructure.Db
{
    public class DbContexto : DbContext
    {
    private readonly IConfiguration? _config;
        public DbContexto(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Administrador> Administradores { get; set; } = default;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _config?.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString));

                }
            }
        }
    }
}