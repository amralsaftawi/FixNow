using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerProfileId).IsRequired();
        builder.Property(x => x.Label)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();
        builder.Property(x => x.CountryId).IsRequired();
        builder.Property(x => x.CityId).IsRequired();
        builder.Property(x => x.AreaId).IsRequired();
        builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();
        builder.Property(x => x.BuildingNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode();
        builder.Property(x => x.Floor)
            .HasMaxLength(50)
            .IsUnicode();
        builder.Property(x => x.Apartment)
            .HasMaxLength(50)
            .IsUnicode();
        builder.Property(x => x.Latitude)
            .HasPrecision(9, 6);
        builder.Property(x => x.Longitude)
            .HasPrecision(9, 6);
        builder.Property(x => x.FullAddress)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();
        builder.Property(x => x.IsDefault)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.CustomerProfile)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
