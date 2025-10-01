using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Portal;

namespace TWS.Data.Configurations.Portal
{
    /// <summary>
    /// Entity Framework configuration for OfferingDocument entity
    /// Configures documents associated with offerings
    /// </summary>
    public class OfferingDocumentConfiguration : IEntityTypeConfiguration<OfferingDocument>
    {
        public void Configure(EntityTypeBuilder<OfferingDocument> builder)
        {
            // Table mapping
            builder.ToTable("OfferingDocuments");

            // Primary Key
            builder.HasKey(od => od.Id);

            // Properties configuration
            builder.Property(od => od.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(od => od.OfferingId)
                .IsRequired();

            builder.Property(od => od.DocumentName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(od => od.FilePath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(od => od.UploadDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(od => od.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(od => od.UpdatedAt)
                .IsRequired(false);

            // Indexes for performance
            builder.HasIndex(od => od.OfferingId)
                .HasDatabaseName("IX_OfferingDocuments_OfferingId");

            builder.HasIndex(od => od.UploadDate)
                .HasDatabaseName("IX_OfferingDocuments_UploadDate");

            // Relationships
            // Many-to-one with Offering
            builder.HasOne(od => od.Offering)
                .WithMany(o => o.OfferingDocuments)
                .HasForeignKey(od => od.OfferingId)
                .OnDelete(DeleteBehavior.Cascade); // Delete documents when offering deleted
        }
    }
}