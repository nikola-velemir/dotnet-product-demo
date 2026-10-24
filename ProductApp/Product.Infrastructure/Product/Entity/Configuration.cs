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
            builder.Property(p=>p.Id).HasColumnName("id");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100);
        
            builder.Property(p => p.Description)
                .HasColumnName("description")
                .IsRequired()
                .HasMaxLength(200);
            
            builder.HasMany(p => p.Variants)
                .WithOne()
                .HasForeignKey("productId") // EF Core will create a ProductId shadow foreign key column in the variants table
                .OnDelete(DeleteBehavior.Cascade);

            // Tell EF Core to use the private backing field _variants
            builder.Navigation(p => p.Variants)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_variants");
        }
    }
    internal class Variant : IEntityTypeConfiguration<Domain.Product.Entity.Variant>
    {
        public void Configure(EntityTypeBuilder<Domain.Product.Entity.Variant> builder)
        {
            builder.ToTable("variants");
            
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");
            
            
            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100);

            builder.Property(p => p.Price)
                .HasColumnName("price")
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}