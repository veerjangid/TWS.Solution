using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.Beneficiaries
{
    /// <summary>
    /// Beneficiary entity - stores beneficiary information for investor profiles
    /// Maps to Beneficiaries table (DatabaseSchema.md Table 19)
    /// One-to-many relationship with InvestorProfile
    /// Supports Primary and Contingent beneficiary types
    /// </summary>
    public class Beneficiary
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile (one-to-many)
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Type of beneficiary (Primary or Contingent)
        /// </summary>
        [Required]
        public BeneficiaryType BeneficiaryType { get; set; }

        /// <summary>
        /// Beneficiary's first, middle, and last name
        /// Combined field as per DatabaseSchema.md
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string FirstMiddleLastName { get; set; } = string.Empty;

        /// <summary>
        /// Social Security Number (will be encrypted)
        /// Field-level encryption applied before storage
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SocialSecurityNumber { get; set; } = string.Empty;

        /// <summary>
        /// Beneficiary's date of birth
        /// </summary>
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Beneficiary's phone number
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Relationship to the account owner
        /// Examples: Spouse, Child, Parent, Sibling, Friend, etc.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string RelationshipToOwner { get; set; } = string.Empty;

        /// <summary>
        /// Beneficiary's street address
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Beneficiary's city
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Beneficiary's state
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Beneficiary's zip code
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Zip { get; set; } = string.Empty;

        /// <summary>
        /// Percentage of benefit allocated to this beneficiary
        /// Must be between 0 and 100
        /// Total percentages per beneficiary type must equal 100%
        /// Business logic validation performed in service layer
        /// </summary>
        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentageOfBenefit { get; set; }

        /// <summary>
        /// Timestamp when beneficiary record was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when beneficiary record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to InvestorProfile (one-to-many, required)
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public InvestorProfile InvestorProfile { get; set; } = null!;
    }
}