using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(table => table.HasCheckConstraint("CK_Payments_Amount", "\"Amount\" > 0"));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignmentId).IsRequired();
        builder.Property(x => x.CustomerProfileId).IsRequired();
        builder.OwnsOne(x => x.Amount, owned =>
        {
            owned.Property(m => m.Value)
                .HasColumnName("Amount")
                .HasPrecision(12, 2);

            owned.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasConversion<int>()
                .HasDefaultValue(Currency.SAR);
        });
        builder.Property(x => x.PaymentMethod)
            .HasConversion<int>();
        builder.Property(x => x.Status)
            .HasConversion<int>();
        builder.Property(x => x.PaidAt);

        builder.Property(x => x.ProviderReference)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.Status, x.CreatedAtUtc })
            .HasDatabaseName("IX_Payments_Status_CreatedAtUtc");

        builder.HasIndex(x => x.AssignmentId);

        builder.HasOne(x => x.Assignment)
            .WithMany()
            .HasForeignKey(x => x.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CustomerProfile)
            .WithMany()
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
