using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.TypeSpecific
{
    /// <summary>
    /// Entity Investor specific details (for companies/organizations)
    /// Maps to EntityInvestorDetails table (DatabaseSchema.md Table 9)
    /// One-to-one relationship with InvestorProfile
    /// Used when InvestorType = Entity
    /// </summary>
    public class EntityInvestorDetail
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
        /// Name of the company/entity
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this is a US-based company
        /// </summary>
        [Required]
        public bool IsUSCompany { get; set; }

        /// <summary>
        /// Type of entity (LLC, Corporation, Partnership)
        /// </summary>
        [Required]
        public EntityType EntityType { get; set; }

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