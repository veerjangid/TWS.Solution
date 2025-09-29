using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Core;

namespace TWS.Data.Configurations.Core
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for InvestorProfile entity
    /// Reference: DatabaseSchema.md Table 4
    /// </summary>
    public class InvestorProfileConfiguration : IEntityTypeConfiguration<InvestorProfile>
    {
        public void Configure(EntityTypeBuilder<InvestorProfile> builder)
        {
            // Table name
            builder.ToTable("InvestorProfiles");

            // Primary Key
            builder.HasKey(ip => ip.Id);
            builder.Property(ip => ip.Id)
                .ValueGeneratedOnAdd();

            // Foreign Key to ApplicationUser (one-to-one, unique)
            builder.Property(ip => ip.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.HasOne(ip => ip.User)
                .WithOne()
                .HasForeignKey<InvestorProfile>(ip => ip.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint on UserId (one-to-one relationship)
            builder.HasIndex(ip => ip.UserId)
                .IsUnique()
                .HasDatabaseName("IX_InvestorProfiles_UserId_Unique");

            // InvestorType enum (stored as int)
            builder.Property(ip => ip.InvestorType)
                .IsRequired()
                .HasConversion<int>();

            // IsAccredited boolean
            builder.Property(ip => ip.IsAccredited)
                .IsRequired()
                .HasDefaultValue(false);

            // AccreditationType enum (nullable, stored as int)
            builder.Property(ip => ip.AccreditationType)
                .IsRequired(false)
                .HasConversion<int>();

            // ProfileCompletionPercentage
            builder.Property(ip => ip.ProfileCompletionPercentage)
                .IsRequired()
                .HasDefaultValue(0);

            // Date fields
            builder.Property(ip => ip.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(ip => ip.UpdatedAt)
                .IsRequired(false);

            // IsActive boolean
            builder.Property(ip => ip.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // One-to-one relationship with IndividualInvestorDetail
            builder.HasOne(ip => ip.IndividualProfile)
                .WithOne(iid => iid.InvestorProfile)
                .HasForeignKey<TWS.Data.Entities.TypeSpecific.IndividualInvestorDetail>(iid => iid.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one relationship with JointInvestorDetail
            builder.HasOne(ip => ip.JointProfile)
                .WithOne(jid => jid.InvestorProfile)
                .HasForeignKey<TWS.Data.Entities.TypeSpecific.JointInvestorDetail>(jid => jid.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one relationship with IRAInvestorDetail
            builder.HasOne(ip => ip.IRAProfile)
                .WithOne(iid => iid.InvestorProfile)
                .HasForeignKey<TWS.Data.Entities.TypeSpecific.IRAInvestorDetail>(iid => iid.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one relationship with TrustInvestorDetail
            builder.HasOne(ip => ip.TrustProfile)
                .WithOne(tid => tid.InvestorProfile)
                .HasForeignKey<TWS.Data.Entities.TypeSpecific.TrustInvestorDetail>(tid => tid.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one relationship with EntityInvestorDetail
            builder.HasOne(ip => ip.EntityProfile)
                .WithOne(eid => eid.InvestorProfile)
                .HasForeignKey<TWS.Data.Entities.TypeSpecific.EntityInvestorDetail>(eid => eid.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(ip => ip.InvestorType)
                .HasDatabaseName("IX_InvestorProfiles_InvestorType");

            builder.HasIndex(ip => ip.IsActive)
                .HasDatabaseName("IX_InvestorProfiles_IsActive");

            builder.HasIndex(ip => ip.IsAccredited)
                .HasDatabaseName("IX_InvestorProfiles_IsAccredited");
        }
    }
}