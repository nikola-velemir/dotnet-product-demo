using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Presentation.CORS;

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
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    // 2. Add CORS middleware to the pipeline
    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors(AllowAllPolicy);
        return app;
    }
}