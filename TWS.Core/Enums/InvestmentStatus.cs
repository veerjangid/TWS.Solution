namespace TWS.Core.Enums
{
    public enum InvestmentStatus
    {
        NeedDSTToComeOut = 1,
        Onboarding = 2,
        InvestmentPaperwork = 3,
        BDApproval = 4,
        DocsDoneNeedPropertyToClose = 5,
        AtCustodianForSignature = 6,
        InBackupStatusAtSponsor = 7,
        Sponsor = 8,
        QI = 9,
        WireRequested = 10,
        Funded = 11,
        ClosedWON = 12,
        FullCycleInvestment = 13
    }
}