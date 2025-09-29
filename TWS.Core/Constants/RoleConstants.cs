namespace TWS.Core.Constants
{
    public static class RoleConstants
    {
        public const string Investor = "Investor";
        public const string Advisor = "Advisor";
        public const string OperationsTeam = "OperationsTeam";

        public static readonly string[] AllRoles = { Investor, Advisor, OperationsTeam };
        public static readonly string[] AdminRoles = { Advisor, OperationsTeam };
    }
}