using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Financial;

namespace TWS.Data.Configurations.Financial
{
    /// <summary>
    /// Entity Framework configuration for PersonalFinancialStatement entity.
    /// </summary>
    public class PersonalFinancialStatementConfiguration : IEntityTypeConfiguration<PersonalFinancialStatement>
    {
        public void Configure(EntityTypeBuilder<PersonalFinancialStatement> builder)
        {
            // Table name
            builder.ToTable("PersonalFinancialStatements");

            // Primary Key
            builder.HasKey(pfs => pfs.Id);

            // Properties
            builder.Property(pfs => pfs.InvestorProfileId)
                .IsRequired();

            builder.Property(pfs => pfs.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pfs => pfs.UploadDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(pfs => pfs.LastModifiedDate)
                .IsRequired(false);

            builder.Property(pfs => pfs.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(pfs => pfs.UpdatedAt)
                .IsRequired(false);

            // Unique Index on InvestorProfileId (One-to-One relationship enforcement)
            builder.HasIndex(pfs => pfs.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_PersonalFinancialStatements_InvestorProfileId");

            // Relationships
            builder.HasOne(pfs => pfs.InvestorProfile)
                .WithOne()
                .HasForeignKey<PersonalFinancialStatement>(pfs => pfs.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PersonalFinancialStatements_InvestorProfile");
        }
    }
}