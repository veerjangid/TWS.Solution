using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.Portal;

namespace TWS.Data.Configurations.Portal
{
    /// <summary>
    /// Entity Framework configuration for Offering entity
    /// Reference: DatabaseSchema.md Table 25/30
    /// </summary>
    public class OfferingConfiguration : IEntityTypeConfiguration<Offering>
    {
        public void Configure(EntityTypeBuilder<Offering> builder)
        {
            // Table mapping
            builder.ToTable("Offerings");

            // Primary Key
            builder.HasKey(o => o.Id);

            // Properties configuration
            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(o => o.Description)
                .HasMaxLength(2000)
                .IsRequired();

            // Enum to string conversion for OfferingStatus
            builder.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue(OfferingStatus.Raising);

            builder.Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(o => o.LastModifiedDate)
                .IsRequired(false);

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(o => o.UpdatedAt)
                .IsRequired(false);

            builder.Property(o => o.ImagePath)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.OfferingType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(o => o.TotalValue)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            builder.Property(o => o.PDFPath)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.CreatedByUserId)
                .HasMaxLength(450)
                .IsRequired(false);

            builder.Property(o => o.ModifiedByUserId)
                .HasMaxLength(450)
                .IsRequired(false);

            // Indexes for performance
            builder.HasIndex(o => o.Status)
                .HasDatabaseName("IX_Offerings_Status");

            builder.HasIndex(o => o.CreatedDate)
                .HasDatabaseName("IX_Offerings_CreatedDate");

            builder.HasIndex(o => o.OfferingType)
                .HasDatabaseName("IX_Offerings_OfferingType");

            builder.HasIndex(o => o.CreatedByUserId)
                .HasDatabaseName("IX_Offerings_CreatedByUserId");

            builder.HasIndex(o => o.ModifiedByUserId)
                .HasDatabaseName("IX_Offerings_ModifiedByUserId");

            // Relationships
            // One-to-many with InvestorInvestments
            builder.HasMany(o => o.InvestorInvestments)
                .WithOne(ii => ii.Offering)
                .HasForeignKey(ii => ii.OfferingId)
                .OnDelete(DeleteBehavior.Restrict); // Preserve investment history

            // Many-to-one with ApplicationUser (CreatedByUser)
            builder.HasOne(o => o.CreatedByUser)
                .WithMany()
                .HasForeignKey(o => o.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull); // Set to null if user deleted

            // Many-to-one with ApplicationUser (ModifiedByUser)
            builder.HasOne(o => o.ModifiedByUser)
                .WithMany()
                .HasForeignKey(o => o.ModifiedByUserId)
                .OnDelete(DeleteBehavior.SetNull); // Set to null if user deleted

            // One-to-many with OfferingDocuments
            builder.HasMany(o => o.OfferingDocuments)
                .WithOne(od => od.Offering)
                .HasForeignKey(od => od.OfferingId)
                .OnDelete(DeleteBehavior.Cascade); // Delete documents when offering deleted
        }
    }
}