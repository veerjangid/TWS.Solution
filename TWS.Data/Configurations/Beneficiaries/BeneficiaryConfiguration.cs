using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.Beneficiaries;

namespace TWS.Data.Configurations.Beneficiaries
{
    /// <summary>
    /// Entity Framework configuration for Beneficiary entity
    /// Reference: DatabaseSchema.md Table 19
    /// </summary>
    public class BeneficiaryConfiguration : IEntityTypeConfiguration<Beneficiary>
    {
        public void Configure(EntityTypeBuilder<Beneficiary> builder)
        {
            // Table name
            builder.ToTable("Beneficiaries");

            // Primary key
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Foreign key to InvestorProfile
            builder.Property(b => b.InvestorProfileId)
                .IsRequired();

            builder.HasOne(b => b.InvestorProfile)
                .WithMany(ip => ip.Beneficiaries) // One-to-many relationship with InvestorProfile.Beneficiaries collection
                .HasForeignKey(b => b.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // BeneficiaryType - enum to string conversion
            builder.Property(b => b.BeneficiaryType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // FirstMiddleLastName
            builder.Property(b => b.FirstMiddleLastName)
                .HasMaxLength(200)
                .IsRequired();

            // SocialSecurityNumber - will be encrypted
            builder.Property(b => b.SocialSecurityNumber)
                .HasMaxLength(100)
                .IsRequired();

            // DateOfBirth
            builder.Property(b => b.DateOfBirth)
                .HasColumnType("date")
                .IsRequired();

            // Phone
            builder.Property(b => b.Phone)
                .HasMaxLength(20)
                .IsRequired();

            // RelationshipToOwner
            builder.Property(b => b.RelationshipToOwner)
                .HasMaxLength(100)
                .IsRequired();

            // Address
            builder.Property(b => b.Address)
                .HasMaxLength(500)
                .IsRequired();

            // City
            builder.Property(b => b.City)
                .HasMaxLength(100)
                .IsRequired();

            // State
            builder.Property(b => b.State)
                .HasMaxLength(50)
                .IsRequired();

            // Zip
            builder.Property(b => b.Zip)
                .HasMaxLength(10)
                .IsRequired();

            // PercentageOfBenefit - decimal(5,2) precision
            builder.Property(b => b.PercentageOfBenefit)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            // CreatedAt - default value
            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            // UpdatedAt - nullable
            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            // Indexes

            // Index on InvestorProfileId for lookups
            builder.HasIndex(b => b.InvestorProfileId)
                .HasDatabaseName("IX_Beneficiaries_InvestorProfileId");

            // Index on BeneficiaryType for filtering
            builder.HasIndex(b => b.BeneficiaryType)
                .HasDatabaseName("IX_Beneficiaries_BeneficiaryType");

            // Composite index on InvestorProfileId and BeneficiaryType
            // Used for percentage validation queries per investor and type
            builder.HasIndex(b => new { b.InvestorProfileId, b.BeneficiaryType })
                .HasDatabaseName("IX_Beneficiaries_InvestorProfileId_BeneficiaryType");

            // Check constraints would typically be added via raw SQL in migrations
            // EF Core doesn't fully support check constraints in Fluent API
            // The following constraints will be added in the migration:
            // 1. CK_PercentageRange: PercentageOfBenefit BETWEEN 0 AND 100
            // 2. CK_BeneficiaryType: BeneficiaryType IN ('Primary', 'Contingent')
        }
    }
}