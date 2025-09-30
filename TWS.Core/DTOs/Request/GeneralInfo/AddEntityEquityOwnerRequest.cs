using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.GeneralInfo
{
    public class AddEntityEquityOwnerRequest
    {
        [Required(ErrorMessage = "Entity General Info ID is required")]
        public int EntityGeneralInfoId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;
    }
}