using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class IndividualGeneralInfoConfiguration : IEntityTypeConfiguration<IndividualGeneralInfo>
    {
        public void Configure(EntityTypeBuilder<IndividualGeneralInfo> builder)
        {
            builder.ToTable("IndividualGeneralInfo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IndividualInvestorDetailId)
                .HasColumnName("IndividualInvestorDetailId")
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

            builder.Property(x => x.DriverLicensePath)
                .HasColumnName("DriverLicensePath")
                .HasMaxLength(500);

            builder.Property(x => x.W9Path)
                .HasColumnName("W9Path")
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.IndividualInvestorDetail)
                .WithOne()
                .HasForeignKey<IndividualGeneralInfo>(x => x.IndividualInvestorDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.IndividualInvestorDetailId)
                .IsUnique()
                .HasDatabaseName("IX_IndividualGeneralInfo_IndividualInvestorDetailId");

            builder.HasIndex(x => x.Email)
                .HasDatabaseName("IX_IndividualGeneralInfo_Email");
        }
    }
}