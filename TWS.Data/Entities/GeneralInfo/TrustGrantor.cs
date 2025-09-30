using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("TrustGrantors")]
    public class TrustGrantor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TrustGeneralInfoId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(TrustGeneralInfoId))]
        public virtual TrustGeneralInfo TrustGeneralInfo { get; set; } = null!;
    }
}