using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TechnicianReportConfiguration : IEntityTypeConfiguration<TechnicianReport>
{
    public void Configure(EntityTypeBuilder<TechnicianReport> builder)
    {
        builder.ToTable("TechnicianReports");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.ReporterUserId).IsRequired();
        builder.Property(x => x.Reason)
            .HasConversion(
                v => (int)v,
                v => (TechnicianReportReason)v)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsUnicode();
        builder.Property(x => x.Status)
            .HasConversion(
                v => (int)v,
                v => (TechnicianReportStatus)v)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.TechnicianProfileId, x.ReporterUserId })
            .IsUnique()
            .HasDatabaseName("IX_TechnicianReports_TechnicianProfileId_ReporterUserId");

        builder.HasIndex(x => x.TechnicianProfileId)
            .HasDatabaseName("IX_TechnicianReports_TechnicianProfileId");

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany()
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReporterUser)
            .WithMany()
            .HasForeignKey(x => x.ReporterUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
