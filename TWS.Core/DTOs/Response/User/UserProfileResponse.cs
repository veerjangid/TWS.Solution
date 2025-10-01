namespace TWS.Core.DTOs.Response.User
{
    /// <summary>
    /// Response DTO for user profile
    /// Contains user profile information for authenticated users
    /// </summary>
    public class UserProfileResponse
    {
        /// <summary>
        /// User ID
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Full name (FirstName + LastName)
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// User role (Investor, Advisor, OperationsTeam)
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}