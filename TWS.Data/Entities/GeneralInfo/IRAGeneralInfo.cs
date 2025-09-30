using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("IRAGeneralInfo")]
    public class IRAGeneralInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int IRAInvestorDetailId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(100)]
        public string SSN { get; set; } = string.Empty; // Will be encrypted

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string CustodianName { get; set; } = string.Empty;

        [Required]
        public IRAAccountType AccountType { get; set; }

        [Required]
        [MaxLength(100)]
        public string IRAAccountNumber { get; set; } = string.Empty;

        [Required]
        public bool IsRollingOverToCNB { get; set; }

        [Required]
        [MaxLength(20)]
        public string CustodianPhoneNumber { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CustodianFaxNumber { get; set; }

        [Required]
        public bool HasLiquidatedAssets { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(IRAInvestorDetailId))]
        public virtual IRAInvestorDetail IRAInvestorDetail { get; set; } = null!;
    }
}