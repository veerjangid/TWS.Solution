using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.Identity;

namespace TWS.Data.Entities.Accreditation
{
    /// <summary>
    /// Represents investor accreditation information
    /// </summary>
    public class InvestorAccreditation
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile (One-to-One relationship)
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Type of accreditation
        /// </summary>
        [Required]
        public AccreditationType AccreditationType { get; set; }

        /// <summary>
        /// Whether accreditation has been verified by operations team
        /// </summary>
        [Required]
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Date when accreditation was verified
        /// </summary>
        public DateTime? VerificationDate { get; set; }

        /// <summary>
        /// Foreign key to ApplicationUser who verified the accreditation
        /// </summary>
        public string? VerifiedByUserId { get; set; }

        /// <summary>
        /// License number for LicensesAndCertifications type
        /// </summary>
        [MaxLength(100)]
        public string? LicenseNumber { get; set; }

        /// <summary>
        /// State where license is held for LicensesAndCertifications type
        /// </summary>
        [MaxLength(50)]
        public string? StateLicenseHeld { get; set; }

        /// <summary>
        /// Internal notes by operations team
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

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
        /// Navigation to InvestorProfile
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public InvestorProfile InvestorProfile { get; set; } = null!;

        /// <summary>
        /// Navigation to ApplicationUser who verified
        /// </summary>
        [ForeignKey(nameof(VerifiedByUserId))]
        public ApplicationUser? VerifiedByUser { get; set; }

        /// <summary>
        /// Collection of accreditation documents
        /// </summary>
        public ICollection<AccreditationDocument> AccreditationDocuments { get; set; } = new List<AccreditationDocument>();
    }
}