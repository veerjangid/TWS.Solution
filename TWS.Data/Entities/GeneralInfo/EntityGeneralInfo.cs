using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("EntityGeneralInfo")]
    public class EntityGeneralInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EntityInvestorDetailId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public bool IsUSCompany { get; set; }

        [Required]
        public EntityType EntityType { get; set; }

        [Required]
        public DateTime DateOfFormation { get; set; }

        [Required]
        [MaxLength(1000)]
        public string PurposeOfFormation { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string TINEIN { get; set; } = string.Empty; // Will be encrypted

        [Required]
        public bool HasOperatingAgreement { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(EntityInvestorDetailId))]
        public virtual EntityInvestorDetail EntityInvestorDetail { get; set; } = null!;

        public virtual ICollection<EntityEquityOwner> EntityEquityOwners { get; set; } = new List<EntityEquityOwner>();
    }
}