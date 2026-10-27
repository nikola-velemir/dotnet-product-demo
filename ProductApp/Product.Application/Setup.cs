using Microsoft.Extensions.DependencyInjection;
using Product.Application.Common.Logs.Service;
using Product.Application.Product.Service;

namespace Product.Application;

/// <summary>
/// Koristimo static da bi mogli da vezujemo pozive.
/// </summary>
public static class Setup
{
    public static IServiceCollection ConfigureApplicationDependencies(this IServiceCollection services)
    {
        // Od zivotnih ciklusa zavisnosti, koje registrujete u kontejner zavisnosti imate:
        // - Scoped - instanca se kreira jednom po http zahtevu
        // - Transient - instanca se kreira kada god je neka druga zavisnost zahteva (u toku istog http zahteva)
        // - Singleton - instanca se kreira jednom kada se aplikacija pokrene
        // Pazite da ne koristite scoped instance u singleton objektima, ovo izaziva gresku jer se ne moze napraviti scoped instanca, kako pri pokretanju aplikacije ne postoji HTTP zahtev
        
        // Dodavanje ProductService kao zavisnosti, apstrakcija je IProductService.
        // Zivonti ciklus je Scoped, odnosno kreira se instanca pri prijemu http zahteva.
        services.AddScoped<IProductService, ProductService>();
        
        //Dodavanja LogService kao zavisnosti, apstrakcija je ILogService.
        //Zivotni ciklus je Transient, odnosno kreira se kada ga bilo koja druga zavisnost zahteva.
        //Drugim recima, ako dve razlicite zavisnost zahtevaju ovaj servis, to ce biti dve potpuno razlicite instance.
        services.AddTransient<ILogService, LogService>();
        return services;
    }
}