using AstraCore.AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class AccessPointConfiguration : IEntityTypeConfiguration<AccessPoint>
{
    public void Configure(EntityTypeBuilder<AccessPoint> builder)
    {
        builder.HasKey(ap => ap.Id);

        builder.Property(ap => ap.Id)
            .ValueGeneratedNever();

        builder.Property(ap => ap.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ap => ap.Location)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ap => ap.Description)
            .HasMaxLength(500);

        builder.Property(ap => ap.Type)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(ap => ap.RequiredAccessLevel)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // EF Core populates _accessLogs directly; AccessLogs exposes it as read-only (DDD encapsulation)
        builder.HasMany(ap => ap.AccessLogs)
            .WithOne(l => l.AccessPoint)
            .HasForeignKey(l => l.AccessPointId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(ap => ap.AccessLogs)
            .HasField("_accessLogs")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
