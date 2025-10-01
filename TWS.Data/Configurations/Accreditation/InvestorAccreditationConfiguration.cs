using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.Accreditation;

namespace TWS.Data.Configurations.Accreditation
{
    /// <summary>
    /// Fluent API configuration for InvestorAccreditation entity
    /// </summary>
    public class InvestorAccreditationConfiguration : IEntityTypeConfiguration<InvestorAccreditation>
    {
        public void Configure(EntityTypeBuilder<InvestorAccreditation> builder)
        {
            // Table name
            builder.ToTable("InvestorAccreditations");

            // Primary key
            builder.HasKey(ia => ia.Id);
            builder.Property(ia => ia.Id)
                .ValueGeneratedOnAdd();

            // Properties
            builder.Property(ia => ia.InvestorProfileId)
                .IsRequired();

            builder.Property(ia => ia.AccreditationType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(ia => ia.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(ia => ia.VerificationDate)
                .IsRequired(false);

            builder.Property(ia => ia.VerifiedByUserId)
                .IsRequired(false);

            builder.Property(ia => ia.LicenseNumber)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(ia => ia.StateLicenseHeld)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(ia => ia.Notes)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(ia => ia.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(ia => ia.UpdatedAt)
                .IsRequired(false);

            // Unique index on InvestorProfileId (one-to-one)
            builder.HasIndex(ia => ia.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_InvestorAccreditations_InvestorProfileId");

            // Index on AccreditationType
            builder.HasIndex(ia => ia.AccreditationType)
                .HasDatabaseName("IX_InvestorAccreditations_AccreditationType");

            // Index on IsVerified
            builder.HasIndex(ia => ia.IsVerified)
                .HasDatabaseName("IX_InvestorAccreditations_IsVerified");

            // Relationships

            // Foreign key to InvestorProfile (one-to-one) with cascade delete
            builder.HasOne(ia => ia.InvestorProfile)
                .WithOne()
                .HasForeignKey<InvestorAccreditation>(ia => ia.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InvestorAccreditations_InvestorProfiles");

            // Foreign key to ApplicationUser (VerifiedByUserId) with SetNull on delete
            builder.HasOne(ia => ia.VerifiedByUser)
                .WithMany()
                .HasForeignKey(ia => ia.VerifiedByUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_InvestorAccreditations_ApplicationUsers");

            // One-to-many relationship with AccreditationDocuments
            builder.HasMany(ia => ia.AccreditationDocuments)
                .WithOne(ad => ad.InvestorAccreditation)
                .HasForeignKey(ad => ad.InvestorAccreditationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}