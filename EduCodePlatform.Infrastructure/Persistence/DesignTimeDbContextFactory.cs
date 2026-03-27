using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EduCodePlatform.Infrastructure.Persistence
{
    //  ЕСЛИ ЧЕСТНО, НЕ ПОМНЮ ТОЧНО, ЗАЧЕМ ЭТО, НО ВСТАВИЛ, ТК ЕСТЬ СЕЙЧАС В ДИПЛОМНОМ САЙТЕ
    //  НО ВРОДЕ БЕЗ ЭТОГГО ЧТО-ТО НЕ РАБОТАЛО

    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Текущая директория = Infrastructure
            var basePath = Directory.GetCurrentDirectory();

            // Поднимаемся к WebApi, чтобы прочитать appsettings.json
            var webApiPath = Path.Combine(basePath, "..", "EduCodePlatform.WebApi");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(webApiPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
