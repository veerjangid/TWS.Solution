using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("EntityEquityOwners")]
    public class EntityEquityOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EntityGeneralInfoId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(EntityGeneralInfoId))]
        public virtual EntityGeneralInfo EntityGeneralInfo { get; set; } = null!;
    }
}