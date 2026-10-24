using Microsoft.Extensions.DependencyInjection;

namespace Product.Presentation;

public static class Setup
{
    public static IServiceCollection ConfigurePresentationDependencies(this IServiceCollection services)
    {
        // Registracija svih [ApiController] klasa.
        services.AddControllers();
        return services;
    }
}