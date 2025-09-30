using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.GeneralInfo
{
    public class AddJointAccountHolderRequest
    {
        [Required(ErrorMessage = "Joint General Info ID is required")]
        public int JointGeneralInfoId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "SSN is required")]
        [MaxLength(11, ErrorMessage = "SSN cannot exceed 11 characters")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "SSN must be in format XXX-XX-XXXX")]
        public string SSN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Order Index is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Order Index must be 1 or greater")]
        public int OrderIndex { get; set; }
    }
}