using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class JobTimelineConfiguration : IEntityTypeConfiguration<JobTimeline>
{
    public void Configure(EntityTypeBuilder<JobTimeline> builder)
    {
        builder.ToTable("JobTimelines");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.JobId).IsRequired();
        builder.Property(x => x.Status)
            .HasConversion<int>();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();
        builder.Property(x => x.OccurredOn).IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.JobId, x.OccurredOn });

        builder.HasOne(x => x.Job)
            .WithMany(x => x.Timeline)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
