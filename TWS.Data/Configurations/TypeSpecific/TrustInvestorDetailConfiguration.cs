using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Configurations.TypeSpecific
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for TrustInvestorDetail entity
    /// Reference: DatabaseSchema.md Table 8
    /// </summary>
    public class TrustInvestorDetailConfiguration : IEntityTypeConfiguration<TrustInvestorDetail>
    {
        public void Configure(EntityTypeBuilder<TrustInvestorDetail> builder)
        {
            // Table name
            builder.ToTable("TrustInvestorDetails");

            // Primary Key
            builder.HasKey(tid => tid.Id);
            builder.Property(tid => tid.Id)
                .ValueGeneratedOnAdd();

            // Foreign Key to InvestorProfile (one-to-one, unique)
            builder.Property(tid => tid.InvestorProfileId)
                .IsRequired();

            // Unique constraint on InvestorProfileId (one-to-one relationship)
            builder.HasIndex(tid => tid.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_TrustInvestorDetails_InvestorProfileId_Unique");

            // TrustName field
            builder.Property(tid => tid.TrustName)
                .IsRequired()
                .HasMaxLength(200);

            // IsUSTrust boolean
            builder.Property(tid => tid.IsUSTrust)
                .IsRequired();

            // TrustType enum (stored as int)
            builder.Property(tid => tid.TrustType)
                .IsRequired()
                .HasConversion<int>();

            // Date fields
            builder.Property(tid => tid.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(tid => tid.UpdatedAt)
                .IsRequired(false);

            // Index on TrustType
            builder.HasIndex(tid => tid.TrustType)
                .HasDatabaseName("IX_TrustInvestorDetails_TrustType");
        }
    }
}