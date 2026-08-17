using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignmentId).IsRequired();
        builder.Property(x => x.ServiceRequestId).IsRequired();
        builder.Property(x => x.CustomerProfileId).IsRequired();
        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.Rating)
            .HasConversion(
                v => v.Value,
                v => Rating.Create(v).Value)
            .IsRequired();
        builder.Property(x => x.Comment)
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(x => x.IsHidden)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.AssignmentId)
            .IsUnique()
            .HasDatabaseName("IX_Reviews_AssignmentId");

        builder.ToTable(table => table.HasCheckConstraint("CK_Reviews_Rating", "\"Rating\" BETWEEN 0 AND 5"));

        builder.HasOne(x => x.Assignment)
            .WithMany()
            .HasForeignKey(x => x.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceRequest)
            .WithMany()
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CustomerProfile)
            .WithMany()
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany()
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
