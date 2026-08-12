using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TechnicianPortfolioItemConfiguration
    : IEntityTypeConfiguration<TechnicianPortfolioItem>
{
    public void Configure(EntityTypeBuilder<TechnicianPortfolioItem> builder)
    {
        builder.ToTable("TechnicianPortfolioItems");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany(x => x.PortfolioItems)
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Media)
            .WithOne(x => x.TechnicianPortfolioItem)
            .HasForeignKey(x => x.TechnicianPortfolioItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TechnicianProfileId);

        builder.Ignore(x => x.DomainEvents);
    }
}
