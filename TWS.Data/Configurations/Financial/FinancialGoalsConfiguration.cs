using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.Financial;

namespace TWS.Data.Configurations.Financial
{
    /// <summary>
    /// Entity Framework configuration for FinancialGoals entity.
    /// Defines table structure, relationships, and constraints for financial goals data.
    /// </summary>
    public class FinancialGoalsConfiguration : IEntityTypeConfiguration<FinancialGoals>
    {
        public void Configure(EntityTypeBuilder<FinancialGoals> builder)
        {
            // Table name
            builder.ToTable("FinancialGoals");

            // Primary Key
            builder.HasKey(fg => fg.Id);

            // Properties
            builder.Property(fg => fg.InvestorProfileId)
                .IsRequired();

            // Enum conversions to string
            builder.Property(fg => fg.LiquidityNeeds)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(fg => fg.InvestmentTimeline)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(fg => fg.InvestmentObjective)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(fg => fg.RiskTolerance)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // Boolean goal fields with default values
            builder.Property(fg => fg.DeferTaxes)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(fg => fg.ProtectPrincipal)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(fg => fg.GrowPrincipal)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(fg => fg.ConsistentCashFlow)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(fg => fg.Diversification)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(fg => fg.Retirement)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(fg => fg.EstateLegacyPlanning)
                .IsRequired()
                .HasDefaultValue(false);

            // Additional notes
            builder.Property(fg => fg.AdditionalNotes)
                .HasMaxLength(2000)
                .IsRequired(false);

            // Timestamps
            builder.Property(fg => fg.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(fg => fg.UpdatedAt)
                .IsRequired(false);

            // Unique Index on InvestorProfileId (One-to-One relationship enforcement)
            builder.HasIndex(fg => fg.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_FinancialGoals_InvestorProfileId");

            // Relationships
            builder.HasOne(fg => fg.InvestorProfile)
                .WithOne()
                .HasForeignKey<FinancialGoals>(fg => fg.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_FinancialGoals_InvestorProfile");
        }
    }
}