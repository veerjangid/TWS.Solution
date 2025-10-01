namespace TWS.Core.DTOs.Response.Beneficiary
{
    public class BeneficiariesGroupedResponse
    {
        public List<BeneficiaryResponse> PrimaryBeneficiaries { get; set; } = new List<BeneficiaryResponse>();
        public decimal PrimaryTotalPercentage { get; set; }
        public List<BeneficiaryResponse> ContingentBeneficiaries { get; set; } = new List<BeneficiaryResponse>();
        public decimal ContingentTotalPercentage { get; set; }
    }
}