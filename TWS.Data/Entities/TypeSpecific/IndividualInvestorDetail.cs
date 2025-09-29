using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.TypeSpecific
{
    /// <summary>
    /// Individual Investor specific details
    /// Maps to IndividualInvestorDetails table (DatabaseSchema.md Table 5)
    /// One-to-one relationship with InvestorProfile
    /// Used when InvestorType = Individual
    /// </summary>
    public class IndividualInvestorDetail
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
        /// First name of the individual investor
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the individual investor
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the investor is a US citizen
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