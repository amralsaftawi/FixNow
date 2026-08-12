using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TechnicianServiceConfiguration : IEntityTypeConfiguration<TechnicianService>
{
    public void Configure(EntityTypeBuilder<TechnicianService> builder)
    {
        builder.ToTable(table => table.HasCheckConstraint("CK_TechnicianServices_Price", "\"Price\" > 0"));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.ServiceCategoryId).IsRequired();
        builder.OwnsOne(x => x.Price, owned =>
        {
            owned.Property(m => m.Value)
                .HasColumnName("Price")
                .HasPrecision(12, 2);

            owned.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .HasConversion<int>();
        });

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceCategory)
            .WithMany()
            .HasForeignKey(x => x.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TechnicianProfileId, x.ServiceCategoryId })
            .IsUnique();

        builder.Ignore(x => x.DomainEvents);
    }
}
