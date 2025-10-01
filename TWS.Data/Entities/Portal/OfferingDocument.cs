using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWS.Data.Entities.Portal
{
    /// <summary>
    /// OfferingDocument entity representing documents associated with offerings
    /// Stores multiple documents per offering
    /// </summary>
    public class OfferingDocument
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to Offering
        /// </summary>
        [Required]
        public int OfferingId { get; set; }

        /// <summary>
        /// Name of the document
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string DocumentName { get; set; } = string.Empty;

        /// <summary>
        /// Path to the document file
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Date when document was uploaded
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
        /// Navigation property to Offering
        /// </summary>
        [Required]
        public Offering Offering { get; set; } = null!;
    }
}