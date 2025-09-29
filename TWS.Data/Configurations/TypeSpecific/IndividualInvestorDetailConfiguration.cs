using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Configurations.TypeSpecific
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for IndividualInvestorDetail entity
    /// Reference: DatabaseSchema.md Table 5
    /// </summary>
    public class IndividualInvestorDetailConfiguration : IEntityTypeConfiguration<IndividualInvestorDetail>
    {
        public void Configure(EntityTypeBuilder<IndividualInvestorDetail> builder)
        {
            // Table name
            builder.ToTable("IndividualInvestorDetails");

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
                .HasDatabaseName("IX_IndividualInvestorDetails_InvestorProfileId_Unique");

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
        }
    }
}