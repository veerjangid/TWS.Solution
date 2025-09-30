using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Data.Configurations.GeneralInfo
{
    public class TrustGrantorConfiguration : IEntityTypeConfiguration<TrustGrantor>
    {
        public void Configure(EntityTypeBuilder<TrustGrantor> builder)
        {
            builder.ToTable("TrustGrantors");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.TrustGeneralInfoId)
                .HasColumnName("TrustGeneralInfoId")
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
            builder.HasOne(x => x.TrustGeneralInfo)
                .WithMany(x => x.TrustGrantors)
                .HasForeignKey(x => x.TrustGeneralInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TrustGeneralInfoId)
                .HasDatabaseName("IX_TrustGrantors_TrustGeneralInfoId");
        }
    }
}