using AstraCore.AccessControl.Domain.Entities;
using AstraCore.AccessControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class AccessCardConfiguration : IEntityTypeConfiguration<AccessCard>
{
    public void Configure(EntityTypeBuilder<AccessCard> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.CardNumber)
            .HasConversion(
                cn => cn.Value,
                value => CardNumber.FromDatabase(value))
            .HasMaxLength(16)
            .IsRequired();

        builder.HasIndex(c => c.CardNumber)
            .IsUnique();

        builder.Property(c => c.AccessLevel)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Ignore(c => c.IsExpired);
        builder.Ignore(c => c.IsValid);

        // EF Core populates _accessLogs directly; AccessLogs exposes it as read-only (DDD encapsulation)
        builder.HasMany(c => c.AccessLogs)
            .WithOne(l => l.AccessCard)
            .HasForeignKey(l => l.AccessCardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(c => c.AccessLogs)
            .HasField("_accessLogs")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
