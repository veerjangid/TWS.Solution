using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Core;

namespace TWS.Data.Configurations.Core
{
    /// <summary>
    /// Entity Framework configuration for InvestorInvestment junction entity
    /// Reference: DatabaseSchema.md Table 26/31
    /// Configures many-to-many relationship between InvestorProfile and Offering
    /// </summary>
    public class InvestorInvestmentConfiguration : IEntityTypeConfiguration<InvestorInvestment>
    {
        public void Configure(EntityTypeBuilder<InvestorInvestment> builder)
        {
            // Table mapping
            builder.ToTable("InvestorInvestments");

            // Primary Key
            builder.HasKey(ii => ii.Id);

            // Properties configuration
            builder.Property(ii => ii.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(ii => ii.InvestorProfileId)
                .IsRequired();

            builder.Property(ii => ii.OfferingId)
                .IsRequired();

            builder.Property(ii => ii.InvestmentDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            // Decimal precision for currency
            builder.Property(ii => ii.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Enum to string conversion for InvestmentStatus
            builder.Property(ii => ii.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(ii => ii.Notes)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(ii => ii.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(ii => ii.UpdatedAt)
                .IsRequired(false);

            // Composite unique constraint to prevent duplicate investments
            // One investor cannot invest in the same offering twice
            builder.HasIndex(ii => new { ii.InvestorProfileId, ii.OfferingId })
                .IsUnique()
                .HasDatabaseName("UQ_InvestorOffering");

            // Indexes for performance
            builder.HasIndex(ii => ii.InvestorProfileId)
                .HasDatabaseName("IX_InvestorInvestments_InvestorProfileId");

            builder.HasIndex(ii => ii.OfferingId)
                .HasDatabaseName("IX_InvestorInvestments_OfferingId");

            builder.HasIndex(ii => ii.Status)
                .HasDatabaseName("IX_InvestorInvestments_Status");

            builder.HasIndex(ii => ii.InvestmentDate)
                .HasDatabaseName("IX_InvestorInvestments_InvestmentDate");

            // Relationships

            // Many-to-one with InvestorProfile (cascade delete)
            builder.HasOne(ii => ii.InvestorProfile)
                .WithMany(ip => ip.InvestorInvestments)
                .HasForeignKey(ii => ii.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Many-to-one with Offering (restrict delete to preserve history)
            builder.HasOne(ii => ii.Offering)
                .WithMany(o => o.InvestorInvestments)
                .HasForeignKey(ii => ii.OfferingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}