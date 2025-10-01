using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWS.Data.Entities.Accreditation
{
    /// <summary>
    /// Represents documents uploaded as proof for accreditation
    /// </summary>
    public class AccreditationDocument
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorAccreditation
        /// </summary>
        [Required]
        public int InvestorAccreditationId { get; set; }

        /// <summary>
        /// Type of document (e.g., "Tax Return", "Pay Stub", "Bank Statement", "CPA Letter", "License")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// File storage path
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string DocumentPath { get; set; } = string.Empty;

        /// <summary>
        /// Date when document was uploaded
        /// </summary>
        [Required]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

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
        /// Navigation to InvestorAccreditation
        /// </summary>
        [ForeignKey(nameof(InvestorAccreditationId))]
        public InvestorAccreditation InvestorAccreditation { get; set; } = null!;
    }
}