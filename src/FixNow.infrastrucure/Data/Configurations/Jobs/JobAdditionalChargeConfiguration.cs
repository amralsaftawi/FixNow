using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class JobAdditionalChargeConfiguration : IEntityTypeConfiguration<JobAdditionalCharge>
{
    public void Configure(EntityTypeBuilder<JobAdditionalCharge> builder)
    {
        builder.ToTable(table => table.HasCheckConstraint("CK_JobAdditionalCharges_Amount", "\"Amount\" > 0"));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.JobId).IsRequired();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();
        builder.OwnsOne(x => x.Amount, owned =>
        {
            owned.Property(m => m.Value)
                .HasColumnName("Amount")
                .HasPrecision(12, 2);

            owned.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasConversion<int>();
        });

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Job)
            .WithMany(x => x.AdditionalCharges)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
