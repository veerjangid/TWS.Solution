using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investor
{
    /// <summary>
    /// Request DTO for updating investor accreditation status
    /// </summary>
    public class UpdateAccreditationRequest
    {
        /// <summary>
        /// Indicates whether the investor is accredited
        /// </summary>
        [Required(ErrorMessage = "IsAccredited field is required")]
        public bool IsAccredited { get; set; }

        /// <summary>
        /// Type of accreditation (required if IsAccredited is true)
        /// </summary>
        public int? AccreditationType { get; set; }
    }
}