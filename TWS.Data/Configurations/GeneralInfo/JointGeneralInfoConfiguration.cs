using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class JointGeneralInfoConfiguration : IEntityTypeConfiguration<JointGeneralInfo>
    {
        public void Configure(EntityTypeBuilder<JointGeneralInfo> builder)
        {
            builder.ToTable("JointGeneralInfo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.JointInvestorDetailId)
                .HasColumnName("JointInvestorDetailId")
                .IsRequired();

            builder.Property(x => x.IsJointInvestment)
                .HasColumnName("IsJointInvestment")
                .IsRequired();

            builder.Property(x => x.JointAccountType)
                .HasColumnName("JointAccountType")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.JointInvestorDetail)
                .WithOne()
                .HasForeignKey<JointGeneralInfo>(x => x.JointInvestorDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Collection Relationship
            builder.HasMany(x => x.JointAccountHolders)
                .WithOne(x => x.JointGeneralInfo)
                .HasForeignKey(x => x.JointGeneralInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.JointInvestorDetailId)
                .IsUnique()
                .HasDatabaseName("IX_JointGeneralInfo_JointInvestorDetailId");
        }
    }
}