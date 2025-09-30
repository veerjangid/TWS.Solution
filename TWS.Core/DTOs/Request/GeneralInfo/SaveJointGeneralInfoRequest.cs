using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.GeneralInfo
{
    public class SaveJointGeneralInfoRequest
    {
        [Required(ErrorMessage = "Investor Profile ID is required")]
        public int InvestorProfileId { get; set; }

        [Required(ErrorMessage = "IsJointInvestment flag is required")]
        public bool IsJointInvestment { get; set; }

        [Required(ErrorMessage = "Joint Account Type is required")]
        public int JointAccountType { get; set; }
    }
}