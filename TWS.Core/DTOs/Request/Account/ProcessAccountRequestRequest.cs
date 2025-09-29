using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Account
{
    /// <summary>
    /// DTO for processing an existing account request
    /// Used when operations team or advisor marks a request as processed
    /// Reference: BusinessRequirement.md Section 3.1, FunctionalRequirement.md
    /// </summary>
    public class ProcessAccountRequestRequest
    {
        /// <summary>
        /// Optional notes about the request processing
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }
    }
}