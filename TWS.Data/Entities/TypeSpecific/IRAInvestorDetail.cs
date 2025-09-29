using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.TypeSpecific
{
    /// <summary>
    /// IRA Investor specific details
    /// Maps to IRAInvestorDetails table (DatabaseSchema.md Table 7)
    /// One-to-one relationship with InvestorProfile
    /// Used when InvestorType = IRA
    /// CRITICAL: Uses exactly 5 IRA types (Traditional, Roth, SEP, Inherited, Inherited Roth)
    /// </summary>
    public class IRAInvestorDetail
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
        /// Type of IRA account (EXACTLY 5 types: Traditional, Roth, SEP, Inherited, Inherited Roth)
        /// </summary>
        [Required]
        public IRAAccountType IRAType { get; set; }

        /// <summary>
        /// Name of the IRA account
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string NameOfIRA { get; set; } = string.Empty;

        /// <summary>
        /// First name of the IRA account holder
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the IRA account holder
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the IRA account holder is a US citizen
        /// </summary>
        [Required]
        public bool IsUSCitizen { get; set; }

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