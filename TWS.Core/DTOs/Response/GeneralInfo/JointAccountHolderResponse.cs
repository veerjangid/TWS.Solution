namespace TWS.Core.DTOs.Response.GeneralInfo
{
    public class JointAccountHolderResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string SSN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}