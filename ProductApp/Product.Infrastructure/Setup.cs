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
    public static IServiceCollection ConfigureInfrastructureDependencies(
        //Kolekcija svih registrovanih Zavisnosti
        this IServiceCollection services,
        // Konfiguracija koja proizilazi iza appsettings.json
        IConfiguration configuration)
    {
        //Dodavanje ProductAppDbContext u zavisnosti, apstrakcija je IDbContext
        // Ovde se dodaje kao scoped, jer da sam rekao da je IDConext singleton, ne bi imao trhead safety.
        // U smislu thread safety, mislim da EF core bi pratio promene koje su van zivotnog ciklusa jednog HTTP zahteva.
        // Da je singleton, change tracker od EF bi pratio svakave izmene, te ne bi imao validno stanje u bazi podataka.
        services.AddDbContext<IDbContext, ProductAppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}