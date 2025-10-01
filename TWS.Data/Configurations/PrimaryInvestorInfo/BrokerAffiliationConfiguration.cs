using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.PrimaryInvestorInfo;

namespace TWS.Data.Configurations.PrimaryInvestorInfo
{
    /// <summary>
    /// Fluent API configuration for BrokerAffiliation entity
    /// Table 19 in DatabaseSchema.md
    /// </summary>
    public class BrokerAffiliationConfiguration : IEntityTypeConfiguration<BrokerAffiliation>
    {
        public void Configure(EntityTypeBuilder<BrokerAffiliation> builder)
        {
            // Table name
            builder.ToTable("BrokerAffiliations");

            // Primary key
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();

            // Foreign key to PrimaryInvestorInfo
            builder.Property(b => b.PrimaryInvestorInfoId)
                .IsRequired();

            // Index on foreign key
            builder.HasIndex(b => b.PrimaryInvestorInfoId)
                .HasDatabaseName("IX_BrokerAffiliations_PrimaryInvestorInfoId");

            // Required boolean fields
            builder.Property(b => b.IsEmployeeOfBrokerDealer)
                .IsRequired();

            builder.Property(b => b.IsRelatedToEmployee)
                .IsRequired();

            builder.Property(b => b.IsSeniorOfficer)
                .IsRequired();

            builder.Property(b => b.IsManagerMemberExecutive)
                .IsRequired();

            // Optional string fields with max lengths
            builder.Property(b => b.BrokerDealerName)
                .HasMaxLength(200);

            builder.Property(b => b.RelatedBrokerDealerName)
                .HasMaxLength(200);

            builder.Property(b => b.EmployeeName)
                .HasMaxLength(200);

            builder.Property(b => b.Relationship)
                .HasMaxLength(100);

            // Timestamps
            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(b => b.UpdatedAt);
        }
    }
}