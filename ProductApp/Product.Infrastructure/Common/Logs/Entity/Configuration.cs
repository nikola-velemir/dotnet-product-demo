using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Common.Logs.Entity;

namespace Product.Infrastructure.Common.Logs.Entity;

// Konfiguracija za entitet Log, kako ne zelim domen da zagadim sa informacijama o perzistenciji
internal class Configuration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        // Zadajem da treba napraviti tabelu logs
        builder.ToTable("logs");
        // Zadajem ime kolone, maksimalnu duzinu stringa Action
        builder.Property(l => l.Action).HasColumnName("action").HasMaxLength(200);
        // Zadajem ime kolone za timestamp
        builder.Property(l => l.Timestamp).HasColumnName("timestamp");
    }
}