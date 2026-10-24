using Microsoft.EntityFrameworkCore;
using Product.Application.Common.Db;
using Product.Domain.Common.Logs.Entity;
using Products = Product.Infrastructure.Product;

namespace Product.Infrastructure.Common.Db;

/// <summary>
/// Ovde zelimo internal, kako ne bi drugi assembiliji koristili konkretnu implementaciju IDbContext.
/// </summary>
internal class ProductAppDbContext : DbContext, IDbContext
{
    
    public DbSet<Domain.Product.Entity.Product> Products { get; init; }
    public DbSet<Log> Logs { get; init; }


    public ProductAppDbContext(DbContextOptions<ProductAppDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Ovo ce pokupiti sve IEntityTypeConfiguration implementacije i registrovati ih kada se pokrene
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductAppDbContext).Assembly);
        
        // Ovde je explicitno navedeno koje konfiguracije zelimo
        modelBuilder.ApplyConfiguration(new Products.Entity.Configuration.Product());
        modelBuilder.ApplyConfiguration(new Products.Entity.Configuration.Variant());
        modelBuilder.ApplyConfiguration(new Logs.Entity.Configuration());
    }
}