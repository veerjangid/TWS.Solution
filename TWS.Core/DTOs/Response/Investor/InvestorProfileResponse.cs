namespace TWS.Core.DTOs.Response.Investor
{
    /// <summary>
    /// Response DTO for investor profile
    /// </summary>
    public class InvestorProfileResponse
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int InvestorType { get; set; }

        public string InvestorTypeName { get; set; } = string.Empty;

        public bool IsAccredited { get; set; }

        public int? AccreditationType { get; set; }

        public string? AccreditationTypeName { get; set; }

        public int ProfileCompletionPercentage { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public object? TypeSpecificDetails { get; set; }
    }
}