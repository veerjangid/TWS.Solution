namespace TWS.Web.Constants;

/// <summary>
/// Web application constants for URLs, session keys, and other web-specific values
/// </summary>
public static class WebConstants
{
    /// <summary>
    /// API endpoint URLs
    /// </summary>
    public static class ApiEndpoints
    {
        // Authentication endpoints
        public const string Login = "/auth/login";
        public const string Register = "/auth/register";
        public const string ForgotPassword = "/auth/forgot-password";
        public const string ResetPassword = "/auth/reset-password";
        public const string ChangePassword = "/auth/change-password";

        // User profile endpoints
        public const string UserProfile = "/user/profile";

        // Investor profile endpoints
        public const string InvestorProfiles = "/investor-profile";
        public const string InvestorProfileById = "/investor-profile/{0}";
        public const string CreateInvestorProfile = "/investor-profile";
        public const string UpdateInvestorProfile = "/investor-profile/{0}";

        // Offering endpoints
        public const string Offerings = "/offering";
        public const string OfferingById = "/offering/{0}";
        public const string CreateOffering = "/offering";
        public const string UpdateOffering = "/offering/{0}";

        // Investment endpoints
        public const string Investments = "/investment";
        public const string InvestmentById = "/investment/{0}";
        public const string InvestorInvestments = "/investment/investor/{0}";

        // Document endpoints
        public const string Documents = "/document";
        public const string DocumentById = "/document/{0}";
        public const string UploadDocument = "/document/upload";
        public const string DownloadDocument = "/document/{0}/download";

        // Portal endpoints (for Advisors and Operations Team)
        public const string PortalDashboard = "/portal/dashboard";
        public const string PortalInvestors = "/portal/investors";
        public const string PortalInvestorDetails = "/portal/investors/{0}";
    }

    /// <summary>
    /// Session keys for storing user data
    /// </summary>
    public static class SessionKeys
    {
        public const string JwtToken = "JwtToken";
        public const string UserEmail = "UserEmail";
        public const string UserRole = "UserRole";
        public const string UserId = "UserId";
        public const string UserFirstName = "UserFirstName";
        public const string UserLastName = "UserLastName";
    }

    /// <summary>
    /// Route names for named routing
    /// </summary>
    public static class RouteNames
    {
        public const string Home = "Home";
        public const string Login = "Login";
        public const string Register = "Register";
        public const string Dashboard = "Dashboard";
        public const string MyAccount = "MyAccount";
        public const string InvestorProfiles = "InvestorProfiles";
        public const string Portal = "Portal";
    }

    /// <summary>
    /// TempData keys for flash messages
    /// </summary>
    public static class TempDataKeys
    {
        public const string SuccessMessage = "SuccessMessage";
        public const string ErrorMessage = "ErrorMessage";
        public const string WarningMessage = "WarningMessage";
        public const string InfoMessage = "InfoMessage";
    }

    /// <summary>
    /// User roles
    /// </summary>
    public static class Roles
    {
        public const string Investor = "Investor";
        public const string Advisor = "Advisor";
        public const string OperationsTeam = "OperationsTeam";
    }

    /// <summary>
    /// Cookie names
    /// </summary>
    public static class Cookies
    {
        public const string AuthenticationCookie = "CookieAuth";
        public const string AntiForgeryCookie = "X-CSRF-TOKEN";
    }

    /// <summary>
    /// Common validation messages
    /// </summary>
    public static class ValidationMessages
    {
        public const string Required = "This field is required";
        public const string InvalidEmail = "Please enter a valid email address";
        public const string PasswordMismatch = "Passwords do not match";
        public const string PasswordRequirements = "Password must be between 8-24 characters and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character";
        public const string InvalidFormat = "Invalid format";
        public const string MaxLengthExceeded = "Maximum length exceeded";
    }

    /// <summary>
    /// Common success messages
    /// </summary>
    public static class SuccessMessages
    {
        public const string LoginSuccess = "Login successful! Welcome back.";
        public const string RegisterSuccess = "Registration successful! Please login with your credentials.";
        public const string ProfileUpdateSuccess = "Profile updated successfully!";
        public const string PasswordChangeSuccess = "Password changed successfully!";
        public const string PasswordResetSuccess = "Password reset successful! Please login with your new password.";
        public const string PasswordResetEmailSent = "Password reset link has been sent to your email.";
        public const string RecordCreated = "Record created successfully!";
        public const string RecordUpdated = "Record updated successfully!";
        public const string RecordDeleted = "Record deleted successfully!";
    }

    /// <summary>
    /// Common error messages
    /// </summary>
    public static class ErrorMessages
    {
        public const string LoginFailed = "Invalid email or password. Please try again.";
        public const string RegisterFailed = "Registration failed. Please try again.";
        public const string GenericError = "An error occurred. Please try again.";
        public const string Unauthorized = "You are not authorized to access this resource.";
        public const string NotFound = "The requested resource was not found.";
        public const string InvalidData = "The submitted data is invalid.";
        public const string ApiError = "An error occurred while communicating with the server.";
    }

    /// <summary>
    /// Date formats
    /// </summary>
    public static class DateFormats
    {
        public const string ShortDate = "MM/dd/yyyy";
        public const string LongDate = "MMMM dd, yyyy";
        public const string ShortDateTime = "MM/dd/yyyy hh:mm tt";
        public const string LongDateTime = "MMMM dd, yyyy hh:mm:ss tt";
        public const string IsoDate = "yyyy-MM-dd";
        public const string IsoDateTime = "yyyy-MM-ddTHH:mm:ss";
    }

    /// <summary>
    /// Pagination defaults
    /// </summary>
    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
    }
}
