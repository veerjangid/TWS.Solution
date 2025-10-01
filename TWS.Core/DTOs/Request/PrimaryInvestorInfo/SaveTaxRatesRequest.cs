using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.PrimaryInvestorInfo
{
    /// <summary>
    /// Request DTO for saving Tax Rates information
    /// </summary>
    public class SaveTaxRatesRequest
    {
        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        [Required(ErrorMessage = "Primary Investor Info ID is required")]
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets the list of tax rate range identifiers (enum values)
        /// </summary>
        [Required(ErrorMessage = "At least one tax rate is required")]
        [MinLength(1, ErrorMessage = "At least one tax rate is required")]
        public List<int> TaxRateRanges { get; set; } = new List<int>();
    }
}