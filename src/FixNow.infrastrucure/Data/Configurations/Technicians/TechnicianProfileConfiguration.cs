using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TechnicianProfileConfiguration : IEntityTypeConfiguration<TechnicianProfile>
{
    public void Configure(EntityTypeBuilder<TechnicianProfile> builder)
    {
        builder.ToTable("TechnicianProfiles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.VerificationStatus)
            .HasConversion<int>();
        builder.Property(x => x.Availability)
            .HasConversion<int>();
        builder.Property(x => x.YearsOfExperience).IsRequired();
        builder.Property(x => x.Bio)
            .HasMaxLength(1000)
            .IsUnicode();
        builder.Property(x => x.NationalIdImageKey)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(x => x.IsProfileCompleted)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.UserId)
            .IsUnique()
            .HasDatabaseName("IX_TechnicianProfiles_UserId");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Services)
            .WithOne(x => x.TechnicianProfile)
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
