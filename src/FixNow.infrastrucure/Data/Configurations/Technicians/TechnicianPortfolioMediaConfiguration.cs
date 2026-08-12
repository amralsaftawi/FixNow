using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TechnicianPortfolioMediaConfiguration
    : IEntityTypeConfiguration<TechnicianPortfolioMedia>
{
    public void Configure(EntityTypeBuilder<TechnicianPortfolioMedia> builder)
    {
        builder.ToTable("TechnicianPortfolioMedia");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TechnicianPortfolioItemId).IsRequired();
        builder.Property(x => x.MediaKey)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(x => x.DisplayOrder).IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.TechnicianPortfolioItem)
            .WithMany(x => x.Media)
            .HasForeignKey(x => x.TechnicianPortfolioItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TechnicianPortfolioItemId);

        builder.Ignore(x => x.DomainEvents);
    }
}
