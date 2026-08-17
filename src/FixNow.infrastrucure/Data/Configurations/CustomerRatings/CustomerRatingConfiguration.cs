using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class CustomerRatingConfiguration : IEntityTypeConfiguration<CustomerRating>
{
    public void Configure(EntityTypeBuilder<CustomerRating> builder)
    {
        builder.ToTable("CustomerRatings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.JobId).IsRequired();
        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.CustomerProfileId).IsRequired();

        builder.Property(x => x.Rating)
            .HasConversion(
                v => v.Value,
                v => CustomerRatingScore.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.JobId)
            .IsUnique()
            .HasDatabaseName("IX_CustomerRatings_JobId");

        builder.ToTable(table => table.HasCheckConstraint(
            "CK_CustomerRatings_Rating", "\"Rating\" BETWEEN 1 AND 5"));

        builder.HasOne(x => x.Job)
            .WithMany()
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany()
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CustomerProfile)
            .WithMany()
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
