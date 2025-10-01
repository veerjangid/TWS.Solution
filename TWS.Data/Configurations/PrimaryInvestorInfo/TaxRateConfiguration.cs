using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.PrimaryInvestorInfo;

namespace TWS.Data.Configurations.PrimaryInvestorInfo
{
    /// <summary>
    /// Fluent API configuration for TaxRate entity
    /// Table 22 in DatabaseSchema.md
    /// </summary>
    public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
    {
        public void Configure(EntityTypeBuilder<TaxRate> builder)
        {
            // Table name
            builder.ToTable("TaxRates");

            // Primary key
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            // Foreign key to PrimaryInvestorInfo
            builder.Property(t => t.PrimaryInvestorInfoId)
                .IsRequired();

            // Index on foreign key
            builder.HasIndex(t => t.PrimaryInvestorInfoId)
                .HasDatabaseName("IX_TaxRates_PrimaryInvestorInfoId");

            // Enum field - Store as string
            builder.Property(t => t.TaxRateRange)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Timestamps
            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedAt);

            // Composite index for querying by tax rate range
            builder.HasIndex(t => new { t.PrimaryInvestorInfoId, t.TaxRateRange })
                .HasDatabaseName("IX_TaxRates_PrimaryInvestorInfoId_TaxRateRange");
        }
    }
}