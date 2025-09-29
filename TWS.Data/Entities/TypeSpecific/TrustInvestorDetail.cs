using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.TypeSpecific
{
    /// <summary>
    /// Trust Investor specific details
    /// Maps to TrustInvestorDetails table (DatabaseSchema.md Table 8)
    /// One-to-one relationship with InvestorProfile
    /// Used when InvestorType = Trust
    /// </summary>
    public class TrustInvestorDetail
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
        /// Name of the trust
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string TrustName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this is a US-based trust
        /// </summary>
        [Required]
        public bool IsUSTrust { get; set; }

        /// <summary>
        /// Type of trust (Revocable or Irrevocable)
        /// </summary>
        [Required]
        public TrustType TrustType { get; set; }

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