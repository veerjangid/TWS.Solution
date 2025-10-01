using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Portal;

namespace TWS.Data.Entities.Core
{
    /// <summary>
    /// InvestorInvestment junction entity for many-to-many relationship
    /// Maps to InvestorInvestments table (DatabaseSchema.md Table 26/31)
    /// Links InvestorProfile to Offering with investment details
    /// One investor can have multiple investments, one offering can have multiple investors
    /// </summary>
    public class InvestorInvestment
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Foreign key to Offering
        /// </summary>
        [Required]
        public int OfferingId { get; set; }

        /// <summary>
        /// Date when the investment was made
        /// </summary>
        [Required]
        public DateTime InvestmentDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Investment amount
        /// Precision: (18,2) for decimal currency values
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Status of the investment (13 predefined statuses)
        /// NeedDSTToComeOut, Onboarding, InvestmentPaperwork, BDApproval, etc.
        /// </summary>
        [Required]
        public InvestmentStatus Status { get; set; }

        /// <summary>
        /// Additional notes about the investment
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

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
        /// Navigation property to InvestorProfile (many-to-one, required)
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public InvestorProfile InvestorProfile { get; set; } = null!;

        /// <summary>
        /// Navigation property to Offering (many-to-one, required)
        /// </summary>
        [ForeignKey(nameof(OfferingId))]
        public Offering Offering { get; set; } = null!;
    }
}