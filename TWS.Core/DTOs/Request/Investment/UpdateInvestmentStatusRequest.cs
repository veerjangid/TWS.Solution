using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investment
{
    /// <summary>
    /// Request DTO for updating investment status
    /// Used by operations team to track investment progress
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class UpdateInvestmentStatusRequest
    {
        /// <summary>
        /// New status for the investment
        /// Valid values: NeedDSTToComeOut, Onboarding, InvestmentPaperwork, BDApproval,
        /// DocsDoneNeedPropertyToClose, AtCustodianForSignature, InBackupStatusAtSponsor,
        /// Sponsor, QI, WireRequested, Funded, ClosedWON, FullCycleInvestment
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Optional notes about the status change
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }
    }
}