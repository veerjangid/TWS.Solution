using System;
using System.ComponentModel.DataAnnotations;
using TWS.Core.Enums;

namespace TWS.Data.Entities.PrimaryInvestorInfo
{
    /// <summary>
    /// Tax Rate entity containing investor's tax rate information
    /// Table 22 in DatabaseSchema.md
    /// </summary>
    public class TaxRate
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
        /// Tax rate range
        /// </summary>
        [Required]
        public TaxRateRange TaxRateRange { get; set; }

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