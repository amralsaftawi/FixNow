using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TechnicianExperienceConfiguration
    : IEntityTypeConfiguration<TechnicianExperience>
{
    public void Configure(EntityTypeBuilder<TechnicianExperience> builder)
    {
        builder.ToTable("TechnicianExperiences");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
        builder.Property(x => x.Position)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsUnicode();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired(false);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany(x => x.Experiences)
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TechnicianProfileId);

        builder.Ignore(x => x.DomainEvents);
    }
}
