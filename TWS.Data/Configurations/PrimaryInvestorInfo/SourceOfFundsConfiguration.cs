using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.PrimaryInvestorInfo;

namespace TWS.Data.Configurations.PrimaryInvestorInfo
{
    /// <summary>
    /// Fluent API configuration for SourceOfFunds entity
    /// Table 21 in DatabaseSchema.md
    /// </summary>
    public class SourceOfFundsConfiguration : IEntityTypeConfiguration<SourceOfFunds>
    {
        public void Configure(EntityTypeBuilder<SourceOfFunds> builder)
        {
            // Table name
            builder.ToTable("SourceOfFunds");

            // Primary key
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            // Foreign key to PrimaryInvestorInfo
            builder.Property(s => s.PrimaryInvestorInfoId)
                .IsRequired();

            // Index on foreign key
            builder.HasIndex(s => s.PrimaryInvestorInfoId)
                .HasDatabaseName("IX_SourceOfFunds_PrimaryInvestorInfoId");

            // Enum field - Store as string
            builder.Property(s => s.SourceType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Timestamps
            builder.Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(s => s.UpdatedAt);

            // Composite index for querying by source type
            builder.HasIndex(s => new { s.PrimaryInvestorInfoId, s.SourceType })
                .HasDatabaseName("IX_SourceOfFunds_PrimaryInvestorInfoId_SourceType");
        }
    }
}