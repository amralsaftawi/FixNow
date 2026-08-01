using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class OTPRecordConfiguration : IEntityTypeConfiguration<OTPRecord>
{
    public void Configure(EntityTypeBuilder<OTPRecord> builder)
    {
        builder.ToTable("OtpRecords");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CodeHash)
            .IsRequired()
            .HasMaxLength(256)
            .IsUnicode(false);
        builder.Property(x => x.Purpose)
            .HasConversion<int>();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.VerifiedAt);
        builder.Property(x => x.AttemptsCount).IsRequired();
        builder.Property(x => x.MaxAttempts).IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.ToTable(table => table.HasCheckConstraint("CK_OtpRecords_Attempts", "\"AttemptsCount\" >= 0 AND \"AttemptsCount\" <= \"MaxAttempts\""));

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
