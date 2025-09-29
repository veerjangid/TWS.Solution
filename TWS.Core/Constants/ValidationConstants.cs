namespace TWS.Core.Constants
{
    public static class ValidationConstants
    {
        // String Lengths
        public const int NameMaxLength = 100;
        public const int EmailMaxLength = 256;
        public const int PhoneMaxLength = 20;
        public const int AddressMaxLength = 500;
        public const int ZipMaxLength = 10;
        public const int NotesMaxLength = 2000;

        // SSN/EIN/TIN
        public const string SSNRegex = @"^\d{3}-\d{2}-\d{4}$";
        public const string EINRegex = @"^\d{2}-\d{7}$";

        // Phone
        public const string PhoneRegex = @"^\d{3}-\d{3}-\d{4}$";

        // Percentage
        public const decimal MinPercentage = 0;
        public const decimal MaxPercentage = 100;

        // Password
        public const int PasswordMinLength = 12;
        public const string PasswordRequiredChars = "Uppercase, lowercase, number, special character";
    }
}