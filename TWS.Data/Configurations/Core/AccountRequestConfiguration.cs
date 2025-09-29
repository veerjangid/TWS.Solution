using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TWS.Data.Entities.Core;

namespace TWS.Data.Configurations.Core
{
    /// <summary>
    /// Entity Framework Core Fluent API configuration for AccountRequest entity
    /// Reference: DatabaseSchema.md Table 3
    /// </summary>
    public class AccountRequestConfiguration : IEntityTypeConfiguration<AccountRequest>
    {
        public void Configure(EntityTypeBuilder<AccountRequest> builder)
        {
            // Table name
            builder.ToTable("AccountRequests");

            // Primary Key
            builder.HasKey(ar => ar.Id);
            builder.Property(ar => ar.Id)
                .ValueGeneratedOnAdd();

            // Required fields with max lengths
            builder.Property(ar => ar.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ar => ar.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ar => ar.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ar => ar.Phone)
                .IsRequired()
                .HasMaxLength(20);

            // Optional fields
            builder.Property(ar => ar.Message)
                .HasMaxLength(1000);

            builder.Property(ar => ar.Notes)
                .HasMaxLength(1000);

            // Date fields
            builder.Property(ar => ar.RequestDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(ar => ar.ProcessedDate)
                .IsRequired(false);

            // Boolean field
            builder.Property(ar => ar.IsProcessed)
                .IsRequired()
                .HasDefaultValue(false);

            // Foreign Key to ApplicationUser
            builder.Property(ar => ar.ProcessedByUserId)
                .HasMaxLength(450)
                .IsRequired(false);

            builder.HasOne(ar => ar.ProcessedByUser)
                .WithMany()
                .HasForeignKey(ar => ar.ProcessedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance (Reference: DatabaseSchema.md indexes section)
            builder.HasIndex(ar => ar.Email)
                .HasDatabaseName("IX_AccountRequests_Email");

            builder.HasIndex(ar => ar.RequestDate)
                .HasDatabaseName("IX_AccountRequests_RequestDate");

            builder.HasIndex(ar => ar.IsProcessed)
                .HasDatabaseName("IX_AccountRequests_IsProcessed");
        }
    }
}