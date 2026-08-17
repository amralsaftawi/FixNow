using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceRequestId).IsRequired();
        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Version)
            .IsRowVersion();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.OwnsOne(x => x.ServicePrice, owned =>
        {
            owned.Property(m => m.Value)
                .HasColumnName("ServicePrice")
                .HasPrecision(12, 2);

            owned.Property(m => m.Currency)
                .HasColumnName("ServicePriceCurrency")
                .HasConversion<int>();
        });

        builder.OwnsOne(x => x.InspectionFee, owned =>
        {
            owned.Property(m => m.Value)
                .HasColumnName("InspectionFee")
                .HasPrecision(12, 2);

            owned.Property(m => m.Currency)
                .HasColumnName("InspectionFeeCurrency")
                .HasConversion<int>();
        });

        builder.HasIndex(x => x.ServiceRequestId)
            .IsUnique()
            .HasDatabaseName("IX_Jobs_ServiceRequestId");

        builder.HasOne(x => x.ServiceRequest)
            .WithOne()
            .HasForeignKey<Job>(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany()
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Timeline)
            .WithOne(x => x.Job)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AdditionalCharges)
            .WithOne(x => x.Job)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
