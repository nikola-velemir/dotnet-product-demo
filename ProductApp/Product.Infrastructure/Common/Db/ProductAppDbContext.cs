using Microsoft.EntityFrameworkCore;
using Product.Application.Common.Db;
using Product.Infrastructure.Product.Entity;

namespace Product.Infrastructure.Common.Db;

/// <summary>
/// Ovde zelimo internal, kako ne bi drugi assembiliji koristili konkretnu implementaciju IDbContext.
/// </summary>
internal class ProductAppDbContext : DbContext, IDbContext
{
    public DbSet<Domain.Product.Entity.Product> Products { get; init; }
    
    public ProductAppDbContext(DbContextOptions<ProductAppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Ovo ce pokupiti sve IEntityTypeConfiguration implementacije i registrovati ih kada se pokrene
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductAppDbContext).Assembly);
        
        // Ovde je explicitno navedeno koje konfiguracije zelimo
        modelBuilder.ApplyConfiguration(new Configuration.Product());
        modelBuilder.ApplyConfiguration(new Configuration.Variant());
    }
}