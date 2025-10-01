using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Accreditation;

namespace TWS.Data.Configurations.Accreditation
{
    /// <summary>
    /// Fluent API configuration for AccreditationDocument entity
    /// </summary>
    public class AccreditationDocumentConfiguration : IEntityTypeConfiguration<AccreditationDocument>
    {
        public void Configure(EntityTypeBuilder<AccreditationDocument> builder)
        {
            // Table name
            builder.ToTable("AccreditationDocuments");

            // Primary key
            builder.HasKey(ad => ad.Id);
            builder.Property(ad => ad.Id)
                .ValueGeneratedOnAdd();

            // Properties
            builder.Property(ad => ad.InvestorAccreditationId)
                .IsRequired();

            builder.Property(ad => ad.DocumentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ad => ad.DocumentPath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(ad => ad.UploadDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(ad => ad.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(ad => ad.UpdatedAt)
                .IsRequired(false);

            // Indexes

            // Index on InvestorAccreditationId
            builder.HasIndex(ad => ad.InvestorAccreditationId)
                .HasDatabaseName("IX_AccreditationDocuments_InvestorAccreditationId");

            // Index on DocumentType
            builder.HasIndex(ad => ad.DocumentType)
                .HasDatabaseName("IX_AccreditationDocuments_DocumentType");

            // Relationships

            // Foreign key to InvestorAccreditation with cascade delete
            builder.HasOne(ad => ad.InvestorAccreditation)
                .WithMany(ia => ia.AccreditationDocuments)
                .HasForeignKey(ad => ad.InvestorAccreditationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AccreditationDocuments_InvestorAccreditations");
        }
    }
}