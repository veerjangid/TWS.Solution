using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.TypeSpecific
{
    /// <summary>
    /// Joint Investor specific details
    /// Maps to JointInvestorDetails table (DatabaseSchema.md Table 6)
    /// One-to-one relationship with InvestorProfile
    /// Used when InvestorType = Joint
    /// </summary>
    public class JointInvestorDetail
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile (one-to-one, unique)
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Indicates if this is a joint investment account
        /// </summary>
        [Required]
        public bool IsJointInvestment { get; set; }

        /// <summary>
        /// Type of joint account (6 predefined types)
        /// </summary>
        [Required]
        public JointAccountType JointAccountType { get; set; }

        /// <summary>
        /// Primary account holder's first name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string PrimaryFirstName { get; set; } = string.Empty;

        /// <summary>
        /// Primary account holder's last name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string PrimaryLastName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if primary account holder is a US citizen
        /// </summary>
        [Required]
        public bool PrimaryIsUSCitizen { get; set; }

        /// <summary>
        /// Secondary account holder's first name (nullable)
        /// </summary>
        [MaxLength(100)]
        public string? SecondaryFirstName { get; set; }

        /// <summary>
        /// Secondary account holder's last name (nullable)
        /// </summary>
        [MaxLength(100)]
        public string? SecondaryLastName { get; set; }

        /// <summary>
        /// Indicates if secondary account holder is a US citizen (nullable)
        /// </summary>
        public bool? SecondaryIsUSCitizen { get; set; }

        /// <summary>
        /// Timestamp when record was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to InvestorProfile (one-to-one, required)
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public InvestorProfile InvestorProfile { get; set; } = null!;
    }
}