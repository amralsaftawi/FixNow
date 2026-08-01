using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceRequestId).IsRequired();
        builder.Property(x => x.TechnicianProfileId).IsRequired();
        builder.Property(x => x.Status)
            .HasConversion<int>();
        builder.Property(x => x.AssignedAt).IsRequired();
        builder.Property(x => x.AcceptedAt);
        builder.Property(x => x.RejectedAt);
        builder.Property(x => x.CompletedAt);
        builder.Property(x => x.RejectReason)
            .HasConversion<int>();

        builder.Property(x => x.CreatedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastModifiedUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.TechnicianProfileId, x.Status })
            .HasDatabaseName("IX_Assignments_TechnicianProfileId_Status");

        builder.HasOne(x => x.ServiceRequest)
            .WithMany()
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TechnicianProfile)
            .WithMany()
            .HasForeignKey(x => x.TechnicianProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.DomainEvents);
    }
}
