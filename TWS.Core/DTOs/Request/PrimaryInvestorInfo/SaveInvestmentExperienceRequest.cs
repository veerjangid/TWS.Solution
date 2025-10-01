using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.PrimaryInvestorInfo
{
    /// <summary>
    /// Request DTO for saving Investment Experience information
    /// </summary>
    public class SaveInvestmentExperienceRequest
    {
        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        [Required(ErrorMessage = "Primary Investor Info ID is required")]
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets the list of investment experiences
        /// </summary>
        [Required(ErrorMessage = "At least one investment experience is required")]
        [MinLength(1, ErrorMessage = "At least one investment experience is required")]
        public List<InvestmentExperienceItem> Experiences { get; set; } = new List<InvestmentExperienceItem>();
    }

    /// <summary>
    /// Represents a single investment experience item
    /// </summary>
    public class InvestmentExperienceItem
    {
        /// <summary>
        /// Gets or sets the asset class (enum value)
        /// </summary>
        [Required(ErrorMessage = "Asset class is required")]
        public int AssetClass { get; set; }

        /// <summary>
        /// Gets or sets the experience level (enum value)
        /// </summary>
        [Required(ErrorMessage = "Experience level is required")]
        public int ExperienceLevel { get; set; }

        /// <summary>
        /// Gets or sets the description for "Other" asset class (optional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Other description cannot exceed 500 characters")]
        public string? OtherDescription { get; set; }
    }
}