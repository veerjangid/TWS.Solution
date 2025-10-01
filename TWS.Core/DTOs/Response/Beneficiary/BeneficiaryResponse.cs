namespace TWS.Core.DTOs.Response.Beneficiary
{
    public class BeneficiaryResponse
    {
        public int Id { get; set; }
        public int InvestorProfileId { get; set; }
        public int BeneficiaryType { get; set; }
        public string BeneficiaryTypeName { get; set; } = string.Empty;
        public string FirstMiddleLastName { get; set; } = string.Empty;
        public string SocialSecurityNumber { get; set; } = string.Empty; // Masked (***-**-1234)
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string RelationshipToOwner { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public decimal PercentageOfBenefit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}