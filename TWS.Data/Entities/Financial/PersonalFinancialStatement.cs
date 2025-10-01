using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.Financial
{
    /// <summary>
    /// Represents the Personal Financial Statement document for an investor.
    /// One-to-one relationship with InvestorProfile.
    /// </summary>
    [Table("PersonalFinancialStatements")]
    public class PersonalFinancialStatement
    {
        /// <summary>
        /// Primary key for the Personal Financial Statement record.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile (One-to-One relationship).
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// File path to the uploaded PDF document.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the document was initially uploaded.
        /// </summary>
        [Required]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the document was last modified/replaced.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Record creation timestamp.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Record update timestamp.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to the associated InvestorProfile.
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public virtual InvestorProfile InvestorProfile { get; set; } = null!;
    }
}