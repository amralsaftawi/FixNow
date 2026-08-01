using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ServiceRequestTimelineConfiguration : IEntityTypeConfiguration<ServiceRequestTimeline>
{
    public void Configure(EntityTypeBuilder<ServiceRequestTimeline> builder)
    {
        builder.ToTable("ServiceRequestTimelines");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceRequestId).IsRequired();
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

        builder.HasOne(x => x.ServiceRequest)
            .WithMany(x => x.Timeline)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
