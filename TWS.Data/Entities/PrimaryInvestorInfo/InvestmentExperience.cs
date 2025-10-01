using System;
using System.ComponentModel.DataAnnotations;
using TWS.Core.Enums;

namespace TWS.Data.Entities.PrimaryInvestorInfo
{
    /// <summary>
    /// Investment Experience entity containing investor's experience with various asset classes
    /// Table 20 in DatabaseSchema.md
    /// </summary>
    public class InvestmentExperience
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to PrimaryInvestorInfo
        /// </summary>
        [Required]
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Asset class type
        /// </summary>
        [Required]
        public AssetClass AssetClass { get; set; }

        /// <summary>
        /// Level of investment experience
        /// </summary>
        [Required]
        public InvestmentExperienceLevel ExperienceLevel { get; set; }

        /// <summary>
        /// Description for 'Other' asset class
        /// </summary>
        [MaxLength(500)]
        public string? OtherDescription { get; set; }

        /// <summary>
        /// Record creation timestamp
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Record last update timestamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to PrimaryInvestorInfo
        /// </summary>
        public PrimaryInvestorInfo PrimaryInvestorInfo { get; set; } = null!;
    }
}