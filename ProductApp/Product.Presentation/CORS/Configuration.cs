using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Presentation.CORS;

// Ovo je konfiguracija za Cross Origin Resource Sharing (CORS).
// Ovde zadajem da je sve apsolutno dozvoljeno, odnosno bilo kakav zahtev moze da gadja aplikaciju
// Vi cete verovatno zeleti da samo vasa frontend aplikacija bude u mogucnosti da gadja serversku aplikaciju.
// Stoga ce te za dozvoljeni origin zadataki adresu vase frontend aplikacije.
public static class Configuration
{
    private const string AllowAllPolicy = "AllowAllPolicy";

    // 1. Register CORS services
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(AllowAllPolicy, policy =>
            {
                policy.
                    //Ovde dozvoljavam bilo koji origin da salje zahtev serverskoj aplikaciji
                    AllowAnyOrigin()
                    // Dozovoljavam bilo koji metod (GET, POST, PUT...)
                    .AllowAnyMethod()
                    // Dozovoljavam bilo sta da se nalazi u okviru zaglavlja HTTP zahteva.
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    // 2. Add CORS middleware to the pipeline
    // Isto kao i kod drugih Setup klasa, koristim fluent builder da dodam ovu konfiguraciju.
    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors(AllowAllPolicy);
        return app;
    }
}