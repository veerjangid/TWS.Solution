using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class JointAccountHolderConfiguration : IEntityTypeConfiguration<JointAccountHolder>
    {
        public void Configure(EntityTypeBuilder<JointAccountHolder> builder)
        {
            builder.ToTable("JointAccountHolders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.JointGeneralInfoId)
                .HasColumnName("JointGeneralInfoId")
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

            builder.Property(x => x.OrderIndex)
                .HasColumnName("OrderIndex")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.JointGeneralInfo)
                .WithMany(x => x.JointAccountHolders)
                .HasForeignKey(x => x.JointGeneralInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.JointGeneralInfoId)
                .HasDatabaseName("IX_JointAccountHolders_JointGeneralInfoId");

            builder.HasIndex(x => x.Email)
                .HasDatabaseName("IX_JointAccountHolders_Email");

            builder.HasIndex(x => x.OrderIndex)
                .HasDatabaseName("IX_JointAccountHolders_OrderIndex");
        }
    }
}