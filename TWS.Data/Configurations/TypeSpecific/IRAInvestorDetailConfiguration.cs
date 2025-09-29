using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Configurations.TypeSpecific
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for IRAInvestorDetail entity
    /// Reference: DatabaseSchema.md Table 7
    /// CRITICAL: Uses exactly 5 IRA types
    /// </summary>
    public class IRAInvestorDetailConfiguration : IEntityTypeConfiguration<IRAInvestorDetail>
    {
        public void Configure(EntityTypeBuilder<IRAInvestorDetail> builder)
        {
            // Table name
            builder.ToTable("IRAInvestorDetails");

            // Primary Key
            builder.HasKey(iid => iid.Id);
            builder.Property(iid => iid.Id)
                .ValueGeneratedOnAdd();

            // Foreign Key to InvestorProfile (one-to-one, unique)
            builder.Property(iid => iid.InvestorProfileId)
                .IsRequired();

            // Unique constraint on InvestorProfileId (one-to-one relationship)
            builder.HasIndex(iid => iid.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_IRAInvestorDetails_InvestorProfileId_Unique");

            // IRAType enum (stored as int) - EXACTLY 5 types
            builder.Property(iid => iid.IRAType)
                .IsRequired()
                .HasConversion<int>();

            // NameOfIRA field
            builder.Property(iid => iid.NameOfIRA)
                .IsRequired()
                .HasMaxLength(200);

            // FirstName field
            builder.Property(iid => iid.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            // LastName field
            builder.Property(iid => iid.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // IsUSCitizen boolean
            builder.Property(iid => iid.IsUSCitizen)
                .IsRequired();

            // Date fields
            builder.Property(iid => iid.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(iid => iid.UpdatedAt)
                .IsRequired(false);

            // Index on IRAType
            builder.HasIndex(iid => iid.IRAType)
                .HasDatabaseName("IX_IRAInvestorDetails_IRAType");
        }
    }
}