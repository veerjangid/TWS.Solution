namespace TWS.Core.DTOs.Response.GeneralInfo
{
    public class GeneralInfoResponse
    {
        public int Id { get; set; }
        public int InvestorProfileId { get; set; }
        public int InvestorType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public object? TypeSpecificData { get; set; }
    }
}