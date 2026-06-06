using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Api.Data;

public class FootballFantasyDbFactory
    : IDesignTimeDbContextFactory<FootballFantasyDb>
{
    public FootballFantasyDb CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        var optionsBuilder = new DbContextOptionsBuilder<FootballFantasyDb>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention();
        return new FootballFantasyDb(optionsBuilder.Options);
    }
}
