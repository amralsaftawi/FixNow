using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ServiceRequestImageConfiguration : IEntityTypeConfiguration<ServiceRequestImage>
{
    public void Configure(EntityTypeBuilder<ServiceRequestImage> builder)
    {
        builder.ToTable("ServiceRequestImages");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceRequestId).IsRequired();
        builder.Property(x => x.ImageKey)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.ServiceRequest)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
