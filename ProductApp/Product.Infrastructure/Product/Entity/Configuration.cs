using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.Product.Entity;

/// <summary>
/// Razlog zasto koristimo IEntityTypeConfiguration, je zato sto bismo inace koristili atribute unutar domenskih klasa.
/// Problem sa ovim je sto onda Domen zna za nacin perzistencije i interakcije u bazi, sto remeti poentu domena i infrastrukturnog sloja.
/// Drugi razlog je sto ovde mozemo mnogo vise izraziti sta zelimo i bolje modelovati interakciju sa bazom podataka.
/// </summary>
internal class Configuration
{
    internal class Product : IEntityTypeConfiguration<Domain.Product.Entity.Product>
    {
        public void Configure(EntityTypeBuilder<Domain.Product.Entity.Product> builder)
        {
            builder.ToTable("products");

            // Ovde zadajem da je Id zapravo primarni kljuc, i da je ime kolone "id"
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");

            // Ovde zadajem ime kolone, maksimalnu duzinu stringa, kao i da ne moze biti null.
            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100);
            // Ovde zadajem ime kolone, maksimalnu duzinu stringa, kao i da ne moze biti null.
            builder.Property(p => p.Description)
                .HasColumnName("description")
                .IsRequired()
                .HasMaxLength(200);

            // Ovde mapiram varijante tako da se u runtime dobije kolekcija varijanti unutar product objekta.
            // Product ima vise Variant-i (1:N). Veza je jednosmerna: Variant nema referencu na Product.
            // FK kolona "product_id" je shadow property: postoji u tabeli, ali ne i kao polje u klasi Variant.
            // EF je koristi za JOIN kada pozovemo Include(p => p.Variants).
            // Takodje kazem da se radi CASCADE prilikom brisanja, odnosno ako se obrise product brisue se sve njegove varijante.
            builder.HasMany(p => p.Variants)
                .WithOne()
                .HasForeignKey("productId")
                .OnDelete(DeleteBehavior.Cascade);

            // EF cita kolekciju direktno preko private polja _variants (ne preko property-ja).
            // Tako entitet moze da izlozi Variants samo za citanje, a izmene idu kroz metode domena.
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