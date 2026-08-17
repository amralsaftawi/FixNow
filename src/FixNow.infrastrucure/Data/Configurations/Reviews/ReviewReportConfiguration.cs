using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ReviewReportConfiguration : IEntityTypeConfiguration<ReviewReport>
{
    public void Configure(EntityTypeBuilder<ReviewReport> builder)
    {
        builder.ToTable("ReviewReports");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReviewId).IsRequired();
        builder.Property(x => x.ReporterUserId).IsRequired();
        builder.Property(x => x.Reason)
            .HasConversion(
                v => (int)v,
                v => (ReviewReportReason)v)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsUnicode();
        builder.Property(x => x.Status)
            .HasConversion(
                v => (int)v,
                v => (ReviewReportStatus)v)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.ReviewId, x.ReporterUserId })
            .IsUnique()
            .HasDatabaseName("IX_ReviewReports_ReviewId_ReporterUserId");

        builder.HasIndex(x => x.ReviewId)
            .HasDatabaseName("IX_ReviewReports_ReviewId");

        builder.HasOne(x => x.Review)
            .WithMany()
            .HasForeignKey(x => x.ReviewId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReporterUser)
            .WithMany()
            .HasForeignKey(x => x.ReporterUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
