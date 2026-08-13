using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProblemTypeConfiguration : IEntityTypeConfiguration<ProblemType>
{
    public void Configure(EntityTypeBuilder<ProblemType> builder)
    {
        builder.ToTable("ProblemTypes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode();
        builder.Property(x => x.ServiceCategoryId).IsRequired();
        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.ServiceCategory)
            .WithMany()
            .HasForeignKey(x => x.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ServiceCategoryId, x.Name })
            .IsUnique();

        builder.Ignore(x => x.DomainEvents);
    }
}
