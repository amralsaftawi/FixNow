using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.ToTable("ServiceCategories");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();
        builder.Property(x => x.IconKey)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(x => x.DisplayOrder).IsRequired();
        builder.OwnsOne(x => x.Price, owned =>
        {
            owned.Property(m => m.Value)
                .HasColumnName("Price")
                .HasPrecision(12, 2);

            owned.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .HasConversion<int>();
        });
        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Ignore(x => x.DomainEvents);
    }
}
