using Microsoft.Extensions.DependencyInjection;
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
        return services;
    }
}