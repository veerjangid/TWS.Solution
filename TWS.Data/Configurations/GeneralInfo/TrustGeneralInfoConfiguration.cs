using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class TrustGeneralInfoConfiguration : IEntityTypeConfiguration<TrustGeneralInfo>
    {
        public void Configure(EntityTypeBuilder<TrustGeneralInfo> builder)
        {
            builder.ToTable("TrustGeneralInfo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.TrustInvestorDetailId)
                .HasColumnName("TrustInvestorDetailId")
                .IsRequired();

            builder.Property(x => x.TrustName)
                .HasColumnName("TrustName")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IsUSTrust)
                .HasColumnName("IsUSTrust")
                .IsRequired();

            builder.Property(x => x.TrustType)
                .HasColumnName("TrustType")
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

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.TrustInvestorDetail)
                .WithOne()
                .HasForeignKey<TrustGeneralInfo>(x => x.TrustInvestorDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Collection Relationship
            builder.HasMany(x => x.TrustGrantors)
                .WithOne(x => x.TrustGeneralInfo)
                .HasForeignKey(x => x.TrustGeneralInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TrustInvestorDetailId)
                .IsUnique()
                .HasDatabaseName("IX_TrustGeneralInfo_TrustInvestorDetailId");

            builder.HasIndex(x => x.TrustType)
                .HasDatabaseName("IX_TrustGeneralInfo_TrustType");
        }
    }
}