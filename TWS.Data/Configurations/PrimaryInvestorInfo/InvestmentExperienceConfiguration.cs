using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.PrimaryInvestorInfo;

namespace TWS.Data.Configurations.PrimaryInvestorInfo
{
    /// <summary>
    /// Fluent API configuration for InvestmentExperience entity
    /// Table 20 in DatabaseSchema.md
    /// </summary>
    public class InvestmentExperienceConfiguration : IEntityTypeConfiguration<InvestmentExperience>
    {
        public void Configure(EntityTypeBuilder<InvestmentExperience> builder)
        {
            // Table name
            builder.ToTable("InvestmentExperiences");

            // Primary key
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            // Foreign key to PrimaryInvestorInfo
            builder.Property(i => i.PrimaryInvestorInfoId)
                .IsRequired();

            // Index on foreign key
            builder.HasIndex(i => i.PrimaryInvestorInfoId)
                .HasDatabaseName("IX_InvestmentExperiences_PrimaryInvestorInfoId");

            // Enum fields - Store as string
            builder.Property(i => i.AssetClass)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(i => i.ExperienceLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Optional description field for 'Other' asset class
            builder.Property(i => i.OtherDescription)
                .HasMaxLength(500);

            // Timestamps
            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(i => i.UpdatedAt);

            // Composite index for querying by asset class
            builder.HasIndex(i => new { i.PrimaryInvestorInfoId, i.AssetClass })
                .HasDatabaseName("IX_InvestmentExperiences_PrimaryInvestorInfoId_AssetClass");
        }
    }
}