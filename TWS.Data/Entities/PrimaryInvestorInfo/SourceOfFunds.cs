using System;
using System.ComponentModel.DataAnnotations;
using TWS.Core.Enums;

namespace TWS.Data.Entities.PrimaryInvestorInfo
{
    /// <summary>
    /// Source of Funds entity containing investor's funding sources
    /// Table 21 in DatabaseSchema.md
    /// </summary>
    public class SourceOfFunds
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
        /// Type of funding source
        /// </summary>
        [Required]
        public SourceOfFundsType SourceType { get; set; }

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