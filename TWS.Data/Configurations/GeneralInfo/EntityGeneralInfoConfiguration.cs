using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class EntityGeneralInfoConfiguration : IEntityTypeConfiguration<EntityGeneralInfo>
    {
        public void Configure(EntityTypeBuilder<EntityGeneralInfo> builder)
        {
            builder.ToTable("EntityGeneralInfo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.EntityInvestorDetailId)
                .HasColumnName("EntityInvestorDetailId")
                .IsRequired();

            builder.Property(x => x.CompanyName)
                .HasColumnName("CompanyName")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IsUSCompany)
                .HasColumnName("IsUSCompany")
                .IsRequired();

            builder.Property(x => x.EntityType)
                .HasColumnName("EntityType")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.DateOfFormation)
                .HasColumnName("DateOfFormation")
                .IsRequired();

            builder.Property(x => x.PurposeOfFormation)
                .HasColumnName("PurposeOfFormation")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.TINEIN)
                .HasColumnName("TINEIN")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.HasOperatingAgreement)
                .HasColumnName("HasOperatingAgreement")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.EntityInvestorDetail)
                .WithOne()
                .HasForeignKey<EntityGeneralInfo>(x => x.EntityInvestorDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Collection Relationship
            builder.HasMany(x => x.EntityEquityOwners)
                .WithOne(x => x.EntityGeneralInfo)
                .HasForeignKey(x => x.EntityGeneralInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.EntityInvestorDetailId)
                .IsUnique()
                .HasDatabaseName("IX_EntityGeneralInfo_EntityInvestorDetailId");

            builder.HasIndex(x => x.EntityType)
                .HasDatabaseName("IX_EntityGeneralInfo_EntityType");
        }
    }
}