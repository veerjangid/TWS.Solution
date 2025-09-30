using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class EntityEquityOwnerConfiguration : IEntityTypeConfiguration<EntityEquityOwner>
    {
        public void Configure(EntityTypeBuilder<EntityEquityOwner> builder)
        {
            builder.ToTable("EntityEquityOwners");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.EntityGeneralInfoId)
                .HasColumnName("EntityGeneralInfoId")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Foreign Key Relationship
            builder.HasOne(x => x.EntityGeneralInfo)
                .WithMany(x => x.EntityEquityOwners)
                .HasForeignKey(x => x.EntityGeneralInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.EntityGeneralInfoId)
                .HasDatabaseName("IX_EntityEquityOwners_EntityGeneralInfoId");
        }
    }
}