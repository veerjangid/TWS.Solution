using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("IndividualGeneralInfo")]
    public class IndividualGeneralInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int IndividualInvestorDetailId { get; set; }

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

        [MaxLength(500)]
        public string? DriverLicensePath { get; set; }

        [MaxLength(500)]
        public string? W9Path { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(IndividualInvestorDetailId))]
        public virtual IndividualInvestorDetail IndividualInvestorDetail { get; set; } = null!;
    }
}