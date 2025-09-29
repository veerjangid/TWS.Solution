using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Configurations.TypeSpecific
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for EntityInvestorDetail entity
    /// Reference: DatabaseSchema.md Table 9
    /// </summary>
    public class EntityInvestorDetailConfiguration : IEntityTypeConfiguration<EntityInvestorDetail>
    {
        public void Configure(EntityTypeBuilder<EntityInvestorDetail> builder)
        {
            // Table name
            builder.ToTable("EntityInvestorDetails");

            // Primary Key
            builder.HasKey(eid => eid.Id);
            builder.Property(eid => eid.Id)
                .ValueGeneratedOnAdd();

            // Foreign Key to InvestorProfile (one-to-one, unique)
            builder.Property(eid => eid.InvestorProfileId)
                .IsRequired();

            // Unique constraint on InvestorProfileId (one-to-one relationship)
            builder.HasIndex(eid => eid.InvestorProfileId)
                .IsUnique()
                .HasDatabaseName("IX_EntityInvestorDetails_InvestorProfileId_Unique");

            // CompanyName field
            builder.Property(eid => eid.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            // IsUSCompany boolean
            builder.Property(eid => eid.IsUSCompany)
                .IsRequired();

            // EntityType enum (stored as int)
            builder.Property(eid => eid.EntityType)
                .IsRequired()
                .HasConversion<int>();

            // Date fields
            builder.Property(eid => eid.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(eid => eid.UpdatedAt)
                .IsRequired(false);

            // Index on EntityType
            builder.HasIndex(eid => eid.EntityType)
                .HasDatabaseName("IX_EntityInvestorDetails_EntityType");
        }
    }
}