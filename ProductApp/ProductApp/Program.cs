using Product.Application;
using Product.Infrastructure;
using Product.Presentation;
using Product.Presentation.CORS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplicationDependencies()
    .ConfigureInfrastructureDependencies(builder.Configuration)
    .ConfigurePresentationDependencies();

builder.Services.ConfigureCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// CORS must be before routing/endpoints
app.UseCorsConfiguration(); 

app.UseRouting();

app.UseAuthorization();


app.MapControllers(); 

app.Run();