using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Identity;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Entities.Core
{
    /// <summary>
    /// Base Investor Profile entity that links to ApplicationUser
    /// Maps to InvestorProfiles table (DatabaseSchema.md Table 4)
    /// One-to-one relationship with ApplicationUser
    /// One-to-one relationships with type-specific profile entities
    /// </summary>
    public class InvestorProfile
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to ApplicationUser (one-to-one)
        /// </summary>
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Type of investor (Individual, Joint, IRA, Trust, Entity)
        /// </summary>
        [Required]
        public InvestorType InvestorType { get; set; }

        /// <summary>
        /// Indicates if the investor is accredited
        /// </summary>
        [Required]
        public bool IsAccredited { get; set; } = false;

        /// <summary>
        /// Type of accreditation (nullable, only set if IsAccredited is true)
        /// </summary>
        public AccreditationType? AccreditationType { get; set; }

        /// <summary>
        /// Profile completion percentage (0-100)
        /// </summary>
        [Required]
        [Range(0, 100)]
        public int ProfileCompletionPercentage { get; set; } = 0;

        /// <summary>
        /// Timestamp when profile was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when profile was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indicates if the profile is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation Properties

        /// <summary>
        /// Navigation property to ApplicationUser (one-to-one, required)
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Navigation property to IndividualInvestorDetail (one-to-one, nullable)
        /// Only populated when InvestorType = Individual
        /// </summary>
        public IndividualInvestorDetail? IndividualProfile { get; set; }

        /// <summary>
        /// Navigation property to JointInvestorDetail (one-to-one, nullable)
        /// Only populated when InvestorType = Joint
        /// </summary>
        public JointInvestorDetail? JointProfile { get; set; }

        /// <summary>
        /// Navigation property to IRAInvestorDetail (one-to-one, nullable)
        /// Only populated when InvestorType = IRA
        /// </summary>
        public IRAInvestorDetail? IRAProfile { get; set; }

        /// <summary>
        /// Navigation property to TrustInvestorDetail (one-to-one, nullable)
        /// Only populated when InvestorType = Trust
        /// </summary>
        public TrustInvestorDetail? TrustProfile { get; set; }

        /// <summary>
        /// Navigation property to EntityInvestorDetail (one-to-one, nullable)
        /// Only populated when InvestorType = Entity
        /// </summary>
        public EntityInvestorDetail? EntityProfile { get; set; }
    }
}