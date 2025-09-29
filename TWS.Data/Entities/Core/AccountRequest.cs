using System.ComponentModel.DataAnnotations;
using TWS.Data.Entities.Identity;

namespace TWS.Data.Entities.Core
{
    /// <summary>
    /// Represents an account request from a potential investor
    /// Maps to AccountRequests table in database
    /// Reference: DatabaseSchema.md Table 3
    /// </summary>
    public class AccountRequest
    {
        /// <summary>
        /// Primary key for the account request
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// First name of the requester
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the requester
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the requester
        /// </summary>
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the requester
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Optional message from the requester
        /// </summary>
        [MaxLength(1000)]
        public string? Message { get; set; }

        /// <summary>
        /// Date and time when the request was submitted
        /// </summary>
        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the request has been processed
        /// </summary>
        [Required]
        public bool IsProcessed { get; set; } = false;

        /// <summary>
        /// Date and time when the request was processed
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Foreign key to ApplicationUser who processed the request
        /// </summary>
        public string? ProcessedByUserId { get; set; }

        /// <summary>
        /// Optional notes about the request processing
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to the user who processed this request
        /// </summary>
        public ApplicationUser? ProcessedByUser { get; set; }
    }
}