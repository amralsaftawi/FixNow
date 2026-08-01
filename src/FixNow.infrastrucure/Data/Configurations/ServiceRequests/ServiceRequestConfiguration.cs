using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("ServiceRequests");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerProfileId).IsRequired();
        builder.Property(x => x.AddressId).IsRequired();
        builder.Property(x => x.ServiceCategoryId).IsRequired();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000)
            .IsUnicode();
        builder.Property(x => x.Priority)
            .HasConversion<int>();
        builder.Property(x => x.Status)
            .HasConversion<int>();
        builder.Property(x => x.RequestedAt).IsRequired();
        builder.Property(x => x.ScheduledAt);
        builder.Property(x => x.CompletedAt);
        builder.Property(x => x.CancelledAt);
        builder.Property(x => x.CancellationReason)
            .HasConversion<int>();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.Status, x.ServiceCategoryId })
            .HasDatabaseName("IX_ServiceRequests_Status_ServiceCategoryId");

        builder.HasOne(x => x.CustomerProfile)
            .WithMany()
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceCategory)
            .WithMany()
            .HasForeignKey(x => x.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Images)
            .WithOne(x => x.ServiceRequest)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Timeline)
            .WithOne(x => x.ServiceRequest)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
