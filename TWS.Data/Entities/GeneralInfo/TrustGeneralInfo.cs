using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("TrustGeneralInfo")]
    public class TrustGeneralInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TrustInvestorDetailId { get; set; }

        [Required]
        [MaxLength(200)]
        public string TrustName { get; set; } = string.Empty;

        [Required]
        public bool IsUSTrust { get; set; }

        [Required]
        public TrustType TrustType { get; set; }

        [Required]
        public DateTime DateOfFormation { get; set; }

        [Required]
        [MaxLength(1000)]
        public string PurposeOfFormation { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string TINEIN { get; set; } = string.Empty; // Will be encrypted

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(TrustInvestorDetailId))]
        public virtual TrustInvestorDetail TrustInvestorDetail { get; set; } = null!;

        public virtual ICollection<TrustGrantor> TrustGrantors { get; set; } = new List<TrustGrantor>();
    }
}