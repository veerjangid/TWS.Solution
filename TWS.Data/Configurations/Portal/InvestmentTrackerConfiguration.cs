using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.Portal;

namespace TWS.Data.Configurations.Portal
{
    /// <summary>
    /// Entity Framework configuration for InvestmentTracker entity
    /// Reference: DatabaseSchema.md Table 32
    /// </summary>
    public class InvestmentTrackerConfiguration : IEntityTypeConfiguration<InvestmentTracker>
    {
        public void Configure(EntityTypeBuilder<InvestmentTracker> builder)
        {
            // Table mapping
            builder.ToTable("InvestmentTrackers");

            // Primary Key
            builder.HasKey(it => it.Id);

            // Properties configuration
            builder.Property(it => it.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(it => it.OfferingId)
                .IsRequired();

            builder.Property(it => it.InvestorProfileId)
                .IsRequired();

            // Enum to string conversion for InvestmentStatus
            builder.Property(it => it.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(it => it.LeadOwnerLicensedRep)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(it => it.Relationship)
                .HasMaxLength(200)
                .IsRequired();

            // Enum to string conversion for PortalInvestmentType
            builder.Property(it => it.InvestmentType)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(it => it.ClientName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(it => it.InvestmentHeldInNamesOf)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(it => it.DateInvestmentClosed)
                .IsRequired(false);

            // Financial fields with decimal precision (18,2)
            builder.Property(it => it.OriginalEquityInvestmentAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.TotalTWSAUM)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.RepCommissionAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.TWSRevenue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.DSTRevenue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.AltRevenue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.TaxStrategyRevenue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.OilAndGasRevenue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.InitialVsRecurringRevenue)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(it => it.MarketingSource)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(it => it.ReferredBy)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(it => it.Notes)
                .HasMaxLength(2000)
                .IsRequired(false);

            builder.Property(it => it.AltAUM)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.DSTAUM)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.OGAUM)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.TaxStrategyAUM)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(it => it.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(it => it.UpdatedAt)
                .IsRequired(false);

            // Indexes for performance

            // Single column indexes
            builder.HasIndex(it => it.OfferingId)
                .HasDatabaseName("IX_InvestmentTrackers_OfferingId");

            builder.HasIndex(it => it.InvestorProfileId)
                .HasDatabaseName("IX_InvestmentTrackers_InvestorProfileId");

            builder.HasIndex(it => it.Status)
                .HasDatabaseName("IX_InvestmentTrackers_Status");

            builder.HasIndex(it => it.LeadOwnerLicensedRep)
                .HasDatabaseName("IX_InvestmentTrackers_LeadOwnerLicensedRep");

            builder.HasIndex(it => it.InvestmentType)
                .HasDatabaseName("IX_InvestmentTrackers_InvestmentType");

            // Composite index for efficient queries
            builder.HasIndex(it => new { it.OfferingId, it.InvestorProfileId })
                .HasDatabaseName("IX_InvestmentTrackers_OfferingId_InvestorProfileId");

            // Relationships

            // Many-to-one with Offering (Restrict delete to preserve tracking history)
            builder.HasOne(it => it.Offering)
                .WithMany()
                .HasForeignKey(it => it.OfferingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Many-to-one with InvestorProfile (Cascade delete when profile is deleted)
            builder.HasOne(it => it.InvestorProfile)
                .WithMany()
                .HasForeignKey(it => it.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}