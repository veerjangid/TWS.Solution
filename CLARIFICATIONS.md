# TWS Investment Platform - Critical Clarifications

**Version**: 1.0
**Date**: Current
**Status**: Final - These clarifications are authoritative

---

## Overview

This document contains critical clarifications that resolve ambiguities in the original requirements. **These clarifications override any contradictory information found in other documents.**

---

## 1. IRA Account Types Standardization (CRITICAL)

### Issue
Original documentation had inconsistency:
- Initial selection mentioned 6 IRA types
- General Info section had 5 different types

### Resolution ✅
**Use EXACTLY 5 standardized IRA types throughout the entire application:**

1. Traditional IRA
2. Roth IRA
3. SEP IRA
4. Inherited IRA
5. Inherited Roth IRA

### Implementation Details
- **Initial Investor Type Selection**: Use these 5 types
- **General Info Section**: Use these SAME 5 types
- **Database Constraint**: Enum with exactly 5 values
- **Enum Name**: `IRAAccountType` (single enum, removed old `IRAType`)

### What NOT to Implement
❌ Profit Sharing
❌ Pension
❌ 401K
❌ Any 6-type system

### Code Example
```csharp
public enum IRAAccountType
{
    TraditionalIRA = 1,
    RothIRA = 2,
    SEPIRA = 3,
    InheritedIRA = 4,
    InheritedRothIRA = 5
}
```

### Database Constraint
```sql
CONSTRAINT CK_IRAAccountType CHECK (
    AccountType IN (
        'Traditional IRA',
        'Roth IRA',
        'SEP IRA',
        'Inherited IRA',
        'Inherited Roth IRA'
    )
)
```

---

## 2. Offering Status Values

### Issue
Original documentation mentioned "Active", "Closed", "Draft" statuses

### Resolution ✅
**Use EXACTLY 3 offering statuses:**

1. **Raising** - Currently raising capital (Default)
2. **Closed** - Offering closed
3. **Coming Soon** - Future offering

### Implementation Details
- **Default Status**: "Raising"
- **Database Constraint**: Enum with exactly 3 values
- **Enum Name**: `OfferingStatus`

### What NOT to Implement
❌ Active
❌ Draft
❌ Any other status values

### Code Example
```csharp
public enum OfferingStatus
{
    Raising = 1,
    Closed = 2,
    ComingSoon = 3
}
```

### Database Constraint
```sql
CONSTRAINT CK_OfferingStatus CHECK (
    Status IN ('Raising', 'Closed', 'Coming Soon')
)
```

---

## 3. Offering Entity Structure

### Issue
Original documentation had complex offering structure with multiple fields

### Resolution ✅
**Simplified Offering Structure:**

Primary field: **OfferingName** (not "Offering")

### Implementation Details
```csharp
public class Offering
{
    public int Id { get; set; }
    public string OfferingName { get; set; }  // Main field
    public string? OfferingType { get; set; }
    public string? Description { get; set; }
    public decimal? TotalValue { get; set; }
    public string Status { get; set; } = "Raising";
    public string? ImagePath { get; set; }
    public string? PDFPath { get; set; }
    // Audit fields...
}
```

---

## 4. Investment-Profile Relationship

### Issue
Needed confirmation on relationship structure

### Resolution ✅
**Standard many-to-many relationship:**

- **Pattern**: Investor ↔ InvestorInvestments ↔ Offering
- **Junction Table**: `InvestorInvestments`
- **Relationship**: One investor can have multiple investments, one offering can have multiple investors

### Implementation Details
```sql
CREATE TABLE InvestorInvestments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id),
    OfferingId INT NOT NULL FOREIGN KEY REFERENCES Offerings(Id),
    InvestmentAmount DECIMAL(18,2),
    InvestmentDate DATE,
    Status NVARCHAR(50) DEFAULT 'Active',
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2,
    CONSTRAINT UQ_InvestorOffering UNIQUE (InvestorId, OfferingId)
);
```

---

## 5. Request Account Form Fields

### Issue
Form fields needed confirmation and UI flow clarification

### Resolution ✅
**Request Account Form - SIMPLIFIED for UI:**

From Pages.md (authoritative for UI):
- **Single field**: Email textbox
- Submit button
- Purpose: Send email to advisor for account creation

From BusinessRequirement.md (authoritative for backend):
- Full set of fields stored when advisor processes the request

### Implementation Strategy
**Frontend (Initial Request)**:
- Collect only **Email** on public request form
- Simple, fast user experience
- Backend stores minimal AccountRequest record

**Backend (Advisor Processing)**:
- Advisor collects full information during account creation
- Full Name, Phone, Message, etc. collected by advisor
- Or use full form if business prefers richer initial data

### Recommended Approach
**Option 1** (Matches Pages.md UI):
- Public form: Email only
- Advisor form: Full details

**Option 2** (Matches BusinessRequirement.md):
- Public form: Full details (Full Name, Email, Phone, Message, etc.)
- More information upfront

**Decision**: Implement Option 2 (full form) as BusinessRequirement.md is more detailed and recent

### Fields (Final)
#### Required:
- Full Name
- Email
- Phone Number

#### Optional:
- Message
- Preferred Contact Method
- Investment Interest

---

## 6. Password Requirements (From Pages.md)

### Specification ✅
**Password must be 8-24 characters** and include:
- At least 1 uppercase letter
- At least 1 number
- At least 1 special character

### Implementation
- Display requirements on password creation/reset screens
- Real-time validation feedback
- Prevent submission if requirements not met

### Code Example
```csharp
// ValidationConstants.cs
public const int PasswordMinLength = 8;
public const int PasswordMaxLength = 24;
public const string PasswordRequirementsMessage =
    "Password must be 8-24 characters and include: " +
    "at least 1 uppercase letter, 1 number, and 1 special character";

// Regex for validation
public const string PasswordRegex =
    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,24}$";
```

---

## 7. User Role Identification (From Pages.md)

### Email-Based Role Assignment ✅

**Rule**:
1. **TWS Admin**: Manually assigned (developers/operations)
2. **Advisor/Rep**: Email ends with `@yourtws.com`
3. **Investor**: All other email addresses

### Implementation
```csharp
public static string DetermineRoleFromEmail(string email)
{
    if (email.EndsWith("@yourtws.com", StringComparison.OrdinalIgnoreCase))
    {
        return RoleConstants.Advisor;
    }
    return RoleConstants.Investor;
}
```

### Clarification
- Operations Team role is manually assigned by admin
- Advisor role can be auto-assigned based on email domain
- Override mechanism should exist for exceptions

---

## 8. Offering Status Display (From Pages.md)

### UI Display ✅
**Status Badge on Thumbnails**:
- "Raising" - Currently accepting investments
- "Closed" - No longer accepting investments
- "Coming Soon" - Future offering (from clarification)

### Color Coding (Recommended)
- Raising: Blue/Green
- Closed: Gray
- Coming Soon: Orange/Yellow

---

## 9. Password Reset Link Expiry

### Specification ✅
- Password reset links expire after **3 hours**
- Email template provided in Pages.md

### Implementation
```csharp
public const int PasswordResetTokenExpiryHours = 3;
```

---

## Document Update Status

### Updated Documents ✅
1. ✅ `BusinessRequirement.md` - Added clarifications section
2. ✅ `DatabaseSchema.md` - Added clarifications section, updated constraints
3. ✅ `FunctionalRequirement.md` - Updated IRA types to 5 types
4. ✅ `ROADMAP.md` - Updated enums, removed IRAType enum, added OfferingStatus
5. ✅ `CLAUDE.md` - Added critical clarifications at top
6. ✅ `CLARIFICATIONS.md` - Created this document (updated with Pages.md requirements)
7. ✅ `UI_UX_REQUIREMENTS.md` - Created from Pages.md specifications

### Documents That Already Were Correct ✅
1. ✅ `APIDoc.md` - Already correct
2. ✅ `SecurityDesign.md` - No changes needed
3. ✅ `AzureImplmentation.md` - No changes needed
4. ✅ `Pages.md` - UI/UX source document (extracted to UI_UX_REQUIREMENTS.md)

---

## Quick Reference Checklist

When implementing IRA-related features:
- [ ] Using exactly 5 IRA types
- [ ] Same types for initial selection and General Info
- [ ] Using IRAAccountType enum (not IRAType)
- [ ] No Profit Sharing, Pension, or 401K

When implementing Offering features:
- [ ] Using exactly 3 statuses (Raising, Closed, Coming Soon)
- [ ] Default status is "Raising"
- [ ] Primary field is "OfferingName"
- [ ] Using OfferingStatus enum

When implementing Investment relationships:
- [ ] Many-to-many via InvestorInvestments junction table
- [ ] Unique constraint on (InvestorId, OfferingId)

---

## Questions or Conflicts?

If you find any conflicting information:
1. **This document (CLARIFICATIONS.md) takes precedence**
2. Next priority: BusinessRequirement.md with clarifications section
3. Next priority: DatabaseSchema.md with clarifications section
4. All other documents follow

---

**END OF CLARIFICATIONS DOCUMENT**

*These clarifications are final and authoritative. All implementation must follow these specifications exactly.*