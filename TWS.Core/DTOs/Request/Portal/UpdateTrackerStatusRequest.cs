using System.ComponentModel.DataAnnotations;
using TWS.Core.Enums;

namespace TWS.Core.DTOs.Request.Portal
{
    /// <summary>
    /// Request DTO for updating investment tracker status
    /// Reference: DatabaseSchema.md Table 32
    /// </summary>
    public class UpdateTrackerStatusRequest
    {
        /// <summary>
        /// Investment status to update to (13 status values)
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        public InvestmentStatus Status { get; set; }

        /// <summary>
        /// Additional notes for the status update (nullable)
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
        public string? Notes { get; set; }
    }
}