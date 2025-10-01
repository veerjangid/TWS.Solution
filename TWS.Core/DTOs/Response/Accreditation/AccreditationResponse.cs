namespace TWS.Core.DTOs.Response.Accreditation
{
    /// <summary>
    /// Response DTO for investor accreditation information
    /// </summary>
    public class AccreditationResponse
    {
        /// <summary>
        /// Accreditation ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The investor profile ID
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Accreditation type value (1-4)
        /// </summary>
        public int AccreditationType { get; set; }

        /// <summary>
        /// Accreditation type name (NotAccredited, IncomeTest, NetWorthTest, LicensesAndCertifications)
        /// </summary>
        public string AccreditationTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Whether accreditation has been verified by Operations Team
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Date when accreditation was verified
        /// </summary>
        public DateTime? VerificationDate { get; set; }

        /// <summary>
        /// User ID of the verifier (Operations Team member)
        /// </summary>
        public string? VerifiedByUserId { get; set; }

        /// <summary>
        /// Full name of the verifier
        /// </summary>
        public string? VerifiedByUserName { get; set; }

        /// <summary>
        /// License number (for LicensesAndCertifications type)
        /// </summary>
        public string? LicenseNumber { get; set; }

        /// <summary>
        /// State where license is held (for LicensesAndCertifications type)
        /// </summary>
        public string? StateLicenseHeld { get; set; }

        /// <summary>
        /// Verification notes from Operations Team
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// List of uploaded accreditation documents
        /// </summary>
        public List<AccreditationDocumentResponse> Documents { get; set; } = new List<AccreditationDocumentResponse>();

        /// <summary>
        /// Date when record was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}