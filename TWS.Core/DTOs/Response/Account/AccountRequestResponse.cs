namespace TWS.Core.DTOs.Response.Account
{
    /// <summary>
    /// DTO for account request response data
    /// Used when returning account request information from API
    /// Reference: DatabaseSchema.md Table 3, BusinessRequirement.md Section 3.1
    /// </summary>
    public class AccountRequestResponse
    {
        /// <summary>
        /// Unique identifier for the account request
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the requester
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the requester
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the requester
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the requester
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Optional message from the requester
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Date and time when the request was submitted (UTC)
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Indicates whether the request has been processed
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Date and time when the request was processed (UTC)
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// ID of the user who processed the request
        /// </summary>
        public string? ProcessedByUserId { get; set; }

        /// <summary>
        /// Full name of the user who processed the request
        /// Mapped from ProcessedByUser navigation property
        /// </summary>
        public string? ProcessedByUserName { get; set; }

        /// <summary>
        /// Optional notes about the request processing
        /// </summary>
        public string? Notes { get; set; }
    }
}