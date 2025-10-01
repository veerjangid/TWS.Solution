using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Core.Enums;
using TWS.Data.Entities.Financial;

namespace TWS.Data.Configurations.Financial
{
    /// <summary>
    /// Entity Framework configuration for FinancialTeamMember entity
    /// Table 29 in DatabaseSchema.md
    /// </summary>
    public class FinancialTeamMemberConfiguration : IEntityTypeConfiguration<FinancialTeamMember>
    {
        public void Configure(EntityTypeBuilder<FinancialTeamMember> builder)
        {
            // Table name
            builder.ToTable("FinancialTeamMembers");

            // Primary Key
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // InvestorProfileId - Foreign Key
            builder.Property(f => f.InvestorProfileId)
                .IsRequired();

            // MemberType - Enum stored as string
            builder.Property(f => f.MemberType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Name
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Email
            builder.Property(f => f.Email)
                .IsRequired()
                .HasMaxLength(200);

            // PhoneNumber
            builder.Property(f => f.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // CreatedAt
            builder.Property(f => f.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // UpdatedAt
            builder.Property(f => f.UpdatedAt)
                .IsRequired(false);

            // Relationship: Many FinancialTeamMembers to One InvestorProfile
            builder.HasOne(f => f.InvestorProfile)
                .WithMany(i => i.FinancialTeamMembers)
                .HasForeignKey(f => f.InvestorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(f => f.InvestorProfileId)
                .HasDatabaseName("IX_FinancialTeamMembers_InvestorProfileId");

            builder.HasIndex(f => f.MemberType)
                .HasDatabaseName("IX_FinancialTeamMembers_MemberType");

            // Composite index for common queries
            builder.HasIndex(f => new { f.InvestorProfileId, f.MemberType })
                .HasDatabaseName("IX_FinancialTeamMembers_InvestorProfileId_MemberType");
        }
    }
}