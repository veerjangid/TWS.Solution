using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Configurations.TypeSpecific
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for JointInvestorDetail entity
    /// Reference: DatabaseSchema.md Table 6
    /// </summary>
    public class JointInvestorDetailConfiguration : IEntityTypeConfiguration<JointInvestorDetail>
    {
        public void Configure(EntityTypeBuilder<JointInvestorDetail> builder)
        {
            // Table name
            builder.ToTable("JointInvestorDetails");

            // Primary Key
            builder.HasKey(jid => jid.Id);
            builder.Property(jid => jid.Id)
                .ValueGeneratedOnAdd();

            // Foreign Key to InvestorProfile (one-to-one, unique)
            builder.Property(jid => jid.InvestorProfileId)
                .IsRequired();

            // Unique constraint on InvestorProfileId (one-to-one relationship)
            builder.HasIndex(jid => jid.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_JointInvestorDetails_InvestorProfileId_Unique");

            // IsJointInvestment boolean
            builder.Property(jid => jid.IsJointInvestment)
                .IsRequired();

            // JointAccountType enum (stored as int)
            builder.Property(jid => jid.JointAccountType)
                .IsRequired()
                .HasConversion<int>();

            // Primary account holder fields
            builder.Property(jid => jid.PrimaryFirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(jid => jid.PrimaryLastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(jid => jid.PrimaryIsUSCitizen)
                .IsRequired();

            // Secondary account holder fields (nullable)
            builder.Property(jid => jid.SecondaryFirstName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(jid => jid.SecondaryLastName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(jid => jid.SecondaryIsUSCitizen)
                .IsRequired(false);

            // Date fields
            builder.Property(jid => jid.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(jid => jid.UpdatedAt)
                .IsRequired(false);

            // Index on JointAccountType
            builder.HasIndex(jid => jid.JointAccountType)
                .HasDatabaseName("IX_JointInvestorDetails_JointAccountType");
        }
    }
}