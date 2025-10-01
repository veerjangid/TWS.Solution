using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Documents;

namespace TWS.Data.Configurations.Documents
{
    /// <summary>
    /// Entity Framework configuration for InvestorDocument entity
    /// Reference: DatabaseSchema.md Table 23
    /// </summary>
    public class InvestorDocumentConfiguration : IEntityTypeConfiguration<InvestorDocument>
    {
        public void Configure(EntityTypeBuilder<InvestorDocument> builder)
        {
            // Table name
            builder.ToTable("InvestorDocuments");

            // Primary key
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Foreign key to InvestorProfile
            builder.Property(d => d.InvestorProfileId)
                .IsRequired();

            builder.HasOne(d => d.InvestorProfile)
                .WithMany() // One-to-many relationship without navigation property in InvestorProfile
                .HasForeignKey(d => d.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // DocumentName
            builder.Property(d => d.DocumentName)
                .HasMaxLength(255)
                .IsRequired();

            // FileName
            builder.Property(d => d.FileName)
                .HasMaxLength(255)
                .IsRequired();

            // FilePath
            builder.Property(d => d.FilePath)
                .HasMaxLength(500)
                .IsRequired();

            // FileSize - nullable
            builder.Property(d => d.FileSize)
                .IsRequired(false);

            // ContentType - nullable
            builder.Property(d => d.ContentType)
                .HasMaxLength(100)
                .IsRequired(false);

            // UploadDate - default value
            builder.Property(d => d.UploadDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            // CreatedAt - default value
            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            // UpdatedAt - nullable
            builder.Property(d => d.UpdatedAt)
                .IsRequired(false);

            // Indexes

            // Index on InvestorProfileId for lookups
            builder.HasIndex(d => d.InvestorProfileId)
                .HasDatabaseName("IX_InvestorDocuments_InvestorProfileId");

            // Index on UploadDate for sorting and filtering
            builder.HasIndex(d => d.UploadDate)
                .HasDatabaseName("IX_InvestorDocuments_UploadDate");
        }
    }
}