using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;

namespace TWS.Data.Entities.Portal
{
    /// <summary>
    /// Offering entity representing investment opportunities
    /// Maps to Offerings table (DatabaseSchema.md Table 25/30)
    /// Referenced by InvestorInvestments for many-to-many relationship with InvestorProfile
    /// </summary>
    public class Offering
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Name of the offering (investment opportunity)
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the offering
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Status of the offering (Raising, Closed, ComingSoon)
        /// Default: Raising
        /// </summary>
        [Required]
        public OfferingStatus Status { get; set; } = OfferingStatus.Raising;

        /// <summary>
        /// Date when offering was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when offering was last modified
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Timestamp when record was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Path to offering image/picture
        /// </summary>
        [MaxLength(500)]
        public string? ImagePath { get; set; }

        /// <summary>
        /// Type of offering: Private Placement 506(c), 1031 Exchange Investment, Universal Offering, Tax Strategy, Roth IRA Conversion
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string OfferingType { get; set; } = string.Empty;

        /// <summary>
        /// Total value of investment
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Path to offering PDF document
        /// </summary>
        [MaxLength(500)]
        public string? PDFPath { get; set; }

        /// <summary>
        /// Foreign key to ApplicationUser - Advisor who created the offering
        /// </summary>
        [MaxLength(450)]
        public string? CreatedByUserId { get; set; }

        /// <summary>
        /// Foreign key to ApplicationUser - Last user who modified the offering
        /// </summary>
        [MaxLength(450)]
        public string? ModifiedByUserId { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to InvestorInvestments (one-to-many)
        /// Collection of investments made in this offering
        /// </summary>
        public ICollection<TWS.Data.Entities.Core.InvestorInvestment>? InvestorInvestments { get; set; }

        /// <summary>
        /// Navigation property to ApplicationUser - Creator
        /// </summary>
        public TWS.Data.Entities.Identity.ApplicationUser? CreatedByUser { get; set; }

        /// <summary>
        /// Navigation property to ApplicationUser - Last modifier
        /// </summary>
        public TWS.Data.Entities.Identity.ApplicationUser? ModifiedByUser { get; set; }

        /// <summary>
        /// Navigation property to OfferingDocuments (one-to-many)
        /// Collection of documents associated with this offering
        /// </summary>
        public ICollection<OfferingDocument>? OfferingDocuments { get; set; }
    }
}