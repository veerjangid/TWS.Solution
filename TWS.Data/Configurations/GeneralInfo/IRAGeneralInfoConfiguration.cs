using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class IRAGeneralInfoConfiguration : IEntityTypeConfiguration<IRAGeneralInfo>
    {
        public void Configure(EntityTypeBuilder<IRAGeneralInfo> builder)
        {
            builder.ToTable("IRAGeneralInfo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IRAInvestorDetailId)
                .HasColumnName("IRAInvestorDetailId")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .IsRequired();

            builder.Property(x => x.SSN)
                .HasColumnName("SSN")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Address)
                .HasColumnName("Address")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.CustodianName)
                .HasColumnName("CustodianName")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.AccountType)
                .HasColumnName("AccountType")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IRAAccountNumber)
                .HasColumnName("IRAAccountNumber")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.IsRollingOverToCNB)
                .HasColumnName("IsRollingOverToCNB")
                .IsRequired();

            builder.Property(x => x.CustodianPhoneNumber)
                .HasColumnName("CustodianPhoneNumber")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.CustodianFaxNumber)
                .HasColumnName("CustodianFaxNumber")
                .HasMaxLength(20);

            builder.Property(x => x.HasLiquidatedAssets)
                .HasColumnName("HasLiquidatedAssets")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.IRAInvestorDetail)
                .WithOne()
                .HasForeignKey<IRAGeneralInfo>(x => x.IRAInvestorDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.IRAInvestorDetailId)
                .IsUnique()
                .HasDatabaseName("IX_IRAGeneralInfo_IRAInvestorDetailId");

            builder.HasIndex(x => x.Email)
                .HasDatabaseName("IX_IRAGeneralInfo_Email");

            builder.HasIndex(x => x.AccountType)
                .HasDatabaseName("IX_IRAGeneralInfo_AccountType");
        }
    }
}