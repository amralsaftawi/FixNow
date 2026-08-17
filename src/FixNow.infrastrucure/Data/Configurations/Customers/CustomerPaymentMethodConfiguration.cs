using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class CustomerPaymentMethodConfiguration : IEntityTypeConfiguration<CustomerPaymentMethod>
{
    public void Configure(EntityTypeBuilder<CustomerPaymentMethod> builder)
    {
        builder.ToTable("CustomerPaymentMethods");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerProfileId).IsRequired();
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(x => x.IsDefault)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.CustomerProfile)
            .WithMany(x => x.PaymentMethods)
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
