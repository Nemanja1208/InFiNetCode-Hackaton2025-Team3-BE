using System;
using System.IO;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure_Layer
{
    /// <summary>
    /// Gör det möjligt för dotnet-ef att skapa ApplicationDbContext
    /// utan att starta hela DI-trädet.
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // 1. Hitta mappen där appsettings*.json ligger
            var basePath = Directory.GetCurrentDirectory();
            if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                // Om vi t.ex. står i Infrastructure-Layer ⇒ gå upp ett steg till API-Layer
                basePath = Path.GetFullPath(Path.Combine(basePath, "..", "API-Layer"));
            }

            // 2. Ladda konfigurationen (läser även ev. appsettings.Development.json)
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json",             optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // 3. Skapa DbContext-instansen
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
