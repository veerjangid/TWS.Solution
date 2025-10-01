using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.PrimaryInvestorInfo
{
    /// <summary>
    /// Request DTO for saving Source of Funds information
    /// </summary>
    public class SaveSourceOfFundsRequest
    {
        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        [Required(ErrorMessage = "Primary Investor Info ID is required")]
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets the list of source type identifiers (enum values)
        /// </summary>
        [Required(ErrorMessage = "At least one source of funds is required")]
        [MinLength(1, ErrorMessage = "At least one source of funds is required")]
        public List<int> SourceTypes { get; set; } = new List<int>();
    }
}