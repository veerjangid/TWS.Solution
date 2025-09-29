namespace TWS.Core.Enums
{
    /// <summary>
    /// IRA Account types - STANDARDIZED to 5 types (used in both initial selection and General Info)
    /// CLARIFICATION: Both initial selection and General Info use the same 5 types
    /// </summary>
    public enum IRAAccountType
    {
        TraditionalIRA = 1,
        RothIRA = 2,
        SEPIRA = 3,
        InheritedIRA = 4,
        InheritedRothIRA = 5
    }
}