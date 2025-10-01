using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.Documents
{
    /// <summary>
    /// InvestorDocument entity - stores document uploads for investor profiles
    /// Maps to InvestorDocuments table (DatabaseSchema.md Table 23)
    /// One-to-many relationship with InvestorProfile
    /// Supports any file type (PDFs, images, Word docs, etc.)
    /// </summary>
    public class InvestorDocument
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile (one-to-many)
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Display name of the document (e.g., "Property Deed", "Insurance Policy")
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string DocumentName { get; set; } = string.Empty;

        /// <summary>
        /// Original file name with extension
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Full file path where document is stored
        /// Example: /wwwroot/uploads/documents/{investorProfileId}/{fileName}
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// MIME type of the file (e.g., "application/pdf", "image/jpeg")
        /// </summary>
        [MaxLength(100)]
        public string? ContentType { get; set; }

        /// <summary>
        /// Timestamp when document was uploaded
        /// </summary>
        [Required]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when record was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to InvestorProfile (many-to-one, required)
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public InvestorProfile InvestorProfile { get; set; } = null!;
    }
}