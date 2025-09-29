namespace TWS.Core.Constants
{
    public static class DocumentConstants
    {
        // File Size Limits (in bytes)
        public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB

        // Allowed File Extensions
        public static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };

        // Document Types
        public const string DriversLicense = "DriversLicense";
        public const string W9 = "W9";
        public const string TrustCertificate = "TrustCertificate";
        public const string TrustAgreement = "TrustAgreement";
        public const string BankStatement = "BankStatement";
        public const string TaxReturn = "TaxReturn";
        public const string ProfessionalLicense = "ProfessionalLicense";

        // Storage Paths
        public const string InvestorDocumentsPath = "investors/{0}/documents";
        public const string AccreditationDocumentsPath = "investors/{0}/accreditation";
        public const string GeneralInfoDocumentsPath = "investors/{0}/general-info";
        public const string PFSDocumentsPath = "investors/{0}/pfs";
    }
}