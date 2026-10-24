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