using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();

        builder.Property(x => x.ProfileImageKey)
            .HasMaxLength(500);

        builder.Property(x => x.DeletedAt);

        builder.Property(x => x.AccountStatus)
            .HasConversion<int>()
            .HasDefaultValue(AccountStatus.PendingVerification);

        builder.Property(x => x.PreferredLanguage)
            .HasConversion<int>();

        builder.Property(x => x.RegisteredVia)
            .HasConversion<int>();

        builder.Property(x => x.IsEmailVerified)
            .HasDefaultValue(false);

        builder.Property(x => x.IsPhoneNumberVerified)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.OwnsOne(x => x.Email, owned =>
        {
            owned.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(320)
                .IsUnicode(false);

            owned.HasIndex(e => e.Value)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email")
                .HasFilter("\"Email\" IS NOT NULL AND \"DeletedAt\" IS NULL");
        });

        builder.OwnsOne(x => x.PhoneNumber, owned =>
        {
            owned.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(16)
                .IsUnicode(false);

            owned.HasIndex(p => p.Value)
                .IsUnique()
                .HasDatabaseName("IX_Users_PhoneNumber")
                .HasFilter("\"DeletedAt\" IS NULL");
        });

        builder.OwnsOne(x => x.PasswordHash, owned =>
        {
            owned.Property(p => p.Value)
                .HasColumnName("PasswordHash")
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        builder.OwnsOne(x => x.CountryCode, owned =>
        {
            owned.Property(c => c.Value)
                .HasColumnName("CountryCode")
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        builder.Ignore(x => x.DomainEvents);
    }
}
