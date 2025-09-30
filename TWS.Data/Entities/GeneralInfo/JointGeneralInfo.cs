using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Entities.GeneralInfo
{
    [Table("JointGeneralInfo")]
    public class JointGeneralInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int JointInvestorDetailId { get; set; }

        [Required]
        public bool IsJointInvestment { get; set; }

        [Required]
        public JointAccountType JointAccountType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(JointInvestorDetailId))]
        public virtual JointInvestorDetail JointInvestorDetail { get; set; } = null!;

        public virtual ICollection<JointAccountHolder> JointAccountHolders { get; set; } = new List<JointAccountHolder>();
    }
}