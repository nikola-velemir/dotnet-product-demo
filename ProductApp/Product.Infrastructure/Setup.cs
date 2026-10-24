using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Common.Db;
using Product.Infrastructure.Common.Db;

namespace Product.Infrastructure;

/// <summary>
/// Koristimo static da bi mogli da vezujemo pozive.
/// </summary>
public static class Setup
{
    /// <summary>
    /// Ovde koristim static metodu sa this ispred service collection kako bi mogao da kazemsamo services.ConfigureInfrsatructureD....
    /// Ovo se zove extension metoda. Obicno se koristi radi registrovanja zavisnosti u "injektor"
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection ConfigureInfrastructureDependencies(
        //Kolekcija svih registrovanih Zavisnosti
        this IServiceCollection services,
        // Konfiguracija koja proizilazi iza appsettings.json
        IConfiguration configuration)
    {
        //Dodavanje ProductAppDbContext u zavisnosti, apstrakcija je IDbContext
        services.AddDbContext<IDbContext, ProductAppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}