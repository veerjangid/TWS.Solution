using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TWS.Core.Enums;

namespace TWS.Data.Entities.Identity
{
    /// <summary>
    /// Extended IdentityUser with custom TWS fields for investor management
    /// Maps to Users table in database (ASP.NET Identity with custom fields)
    /// Reference: DatabaseSchema.md Table 1
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Full name of the user
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Type of investor account (Individual, Joint, IRA, Trust, Entity)
        /// References InvestorType enum from TWS.Core.Enums
        /// </summary>
        [Required]
        public InvestorType InvestorTypeId { get; set; }

        /// <summary>
        /// Timestamp when the user account was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the user account was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indicates whether the user account is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation Properties

        /// <summary>
        /// Navigation property to IndividualProfile (one-to-one)
        /// Will be added when IndividualProfile entity is created in Phase 2
        /// </summary>
        // public IndividualProfile? IndividualProfile { get; set; }

        /// <summary>
        /// Navigation property to JointProfile (one-to-one)
        /// Will be added when JointProfile entity is created in Phase 2
        /// </summary>
        // public JointProfile? JointProfile { get; set; }

        /// <summary>
        /// Navigation property to IRAProfile (one-to-one)
        /// Will be added when IRAProfile entity is created in Phase 2
        /// </summary>
        // public IRAProfile? IRAProfile { get; set; }

        /// <summary>
        /// Navigation property to TrustProfile (one-to-one)
        /// Will be added when TrustProfile entity is created in Phase 2
        /// </summary>
        // public TrustProfile? TrustProfile { get; set; }

        /// <summary>
        /// Navigation property to EntityProfile (one-to-one)
        /// Will be added when EntityProfile entity is created in Phase 2
        /// </summary>
        // public EntityProfile? EntityProfile { get; set; }

        /// <summary>
        /// Collection of account requests made by this user
        /// Will be added when AccountRequest entity is created in Phase 2
        /// </summary>
        // public ICollection<AccountRequest> AccountRequests { get; set; } = new List<AccountRequest>();

        /// <summary>
        /// Collection of investor investments (many-to-many relationship via junction table)
        /// Will be added when InvestorInvestment entity is created in Phase 3
        /// </summary>
        // public ICollection<InvestorInvestment> InvestorInvestments { get; set; } = new List<InvestorInvestment>();
    }
}