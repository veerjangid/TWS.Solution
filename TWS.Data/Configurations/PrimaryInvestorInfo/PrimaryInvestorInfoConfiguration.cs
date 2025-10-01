using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.PrimaryInvestorInfo;

namespace TWS.Data.Configurations.PrimaryInvestorInfo
{
    /// <summary>
    /// Fluent API configuration for PrimaryInvestorInfo entity
    /// Table 18 in DatabaseSchema.md
    /// </summary>
    public class PrimaryInvestorInfoConfiguration : IEntityTypeConfiguration<Entities.PrimaryInvestorInfo.PrimaryInvestorInfo>
    {
        public void Configure(EntityTypeBuilder<Entities.PrimaryInvestorInfo.PrimaryInvestorInfo> builder)
        {
            // Table name
            builder.ToTable("PrimaryInvestorInfos");

            // Primary key
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Foreign key to InvestorProfile - One-to-one relationship
            builder.HasOne(p => p.InvestorProfile)
                .WithOne()
                .HasForeignKey<Entities.PrimaryInvestorInfo.PrimaryInvestorInfo>(p => p.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique index on InvestorProfileId
            builder.HasIndex(p => p.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_PrimaryInvestorInfos_InvestorProfileId");

            // Required fields with max lengths
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LegalStreetAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.State)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Zip)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.CellPhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.IsMarried)
                .IsRequired();

            // Social Security Number (encrypted field)
            builder.Property(p => p.SocialSecurityNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.DriversLicenseNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.DriversLicenseExpirationDate)
                .IsRequired();

            builder.Property(p => p.Occupation)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.EmployerName)
                .HasMaxLength(200);

            builder.Property(p => p.RetiredProfession)
                .HasMaxLength(200);

            builder.Property(p => p.HasAlternateAddress)
                .IsRequired();

            builder.Property(p => p.AlternateAddress)
                .HasMaxLength(500);

            // Decimal fields with precision (18,2)
            builder.Property(p => p.LowestIncomeLastTwoYears)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.AnticipatedIncomeThisYear)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.IsRelyingOnJointIncome)
                .IsRequired();

            // Timestamps
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(p => p.UpdatedAt);

            // One-to-many relationships with supporting entities
            builder.HasMany(p => p.BrokerAffiliations)
                .WithOne(b => b.PrimaryInvestorInfo)
                .HasForeignKey(b => b.PrimaryInvestorInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.InvestmentExperiences)
                .WithOne(i => i.PrimaryInvestorInfo)
                .HasForeignKey(i => i.PrimaryInvestorInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.SourceOfFunds)
                .WithOne(s => s.PrimaryInvestorInfo)
                .HasForeignKey(s => s.PrimaryInvestorInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.TaxRates)
                .WithOne(t => t.PrimaryInvestorInfo)
                .HasForeignKey(t => t.PrimaryInvestorInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes on frequently queried fields
            builder.HasIndex(p => p.Email)
                .HasDatabaseName("IX_PrimaryInvestorInfos_Email");

            builder.HasIndex(p => p.LastName)
                .HasDatabaseName("IX_PrimaryInvestorInfos_LastName");
        }
    }
}