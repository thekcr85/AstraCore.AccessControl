using AstraCore.AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(l => l.Result)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(l => l.Notes)
            .HasMaxLength(500);

        // Computed property — not stored in the database
        builder.Ignore(l => l.WasSuccessful);

        // Indexes to support the log query patterns in IAccessLogRepository
        builder.HasIndex(l => l.AccessCardId);
        builder.HasIndex(l => l.AccessPointId);
        builder.HasIndex(l => l.AttemptedAt);
    }
}
