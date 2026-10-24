using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.Product.Entity;

internal class Configuration 
{
    internal class Product: IEntityTypeConfiguration<Domain.Product.Entity.Product>
    {
     
        public void Configure(EntityTypeBuilder<Domain.Product.Entity.Product> builder)
        {
            builder.ToTable("products");
        
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        
            builder.Property(p => p.Description).IsRequired().HasMaxLength(200);
        }   
    }
    internal class Variant : IEntityTypeConfiguration<Domain.Product.Entity.Variant>
    {
        public void Configure(EntityTypeBuilder<Domain.Product.Entity.Variant> builder)
        {
            builder.ToTable("variants");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
        }
    }
}