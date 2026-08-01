using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.RefreshTokenId).IsRequired();
        builder.Property(x => x.DeviceName)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();
        builder.Property(x => x.IpAddress)
            .IsRequired()
            .HasMaxLength(45)
            .IsUnicode(false);
        builder.Property(x => x.UserAgent)
            .IsRequired()
            .HasMaxLength(1000)
            .IsUnicode(false);
        builder.Property(x => x.StartedAt).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.EndedAt);
        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RefreshToken)
            .WithMany()
            .HasForeignKey(x => x.RefreshTokenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
