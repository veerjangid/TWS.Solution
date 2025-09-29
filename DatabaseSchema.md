# TWS Investment Platform - Database Schema (Updated)

**Version**: 2.1 (Final with Clarifications)
**Date**: Current
**Status**: Final - Ready for Implementation
**Purpose**: Complete Database Schema with all clarifications applied, Always Look TWS.Data Layer For Entities

---

## Document Control

### Changes in Version 2.1:
- **IRA Account Types**: Standardized to exactly 5 types across all tables
- **AccountRequests Table**: Complete field specifications added
- **Offerings Table**: Simplified to basic "OfferingName" structure
- **Investment Relationships**: Many-to-many structure confirmed and detailed
- **All field specifications finalized**: No pending clarifications

### Critical Clarifications Applied:
1. **IRA Types Standardization**: Only 5 types used throughout (Traditional IRA, Roth IRA, SEP IRA, Inherited IRA, Inherited Roth IRA) - applies to both initial selection and General Info section
2. **Offering Status Values**: Three statuses only - "Raising", "Closed", "Coming Soon" (changed from Active/Draft)
3. **Investment-Profile Relationship**: Standard many-to-many via InvestorInvestments junction table
4. **Request Account Form**: All fields specified and implemented as documented

---

## Core Tables Structure

### 1. Users Table (ASP.NET Identity Extended)
```sql
CREATE TABLE Users (
    Id NVARCHAR(450) PRIMARY KEY, -- ASP.NET Identity GUID
    UserName NVARCHAR(256),
    NormalizedUserName NVARCHAR(256),
    Email NVARCHAR(256),
    NormalizedEmail NVARCHAR(256),
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    ConcurrencyStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnd DATETIMEOFFSET(7),
    LockoutEnabled BIT NOT NULL DEFAULT 0,
    AccessFailedCount INT NOT NULL DEFAULT 0,

    -- Custom TWS Fields
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2
);
```

### 2. Roles Table (ASP.NET Identity)
```sql
CREATE TABLE Roles (
    Id NVARCHAR(450) PRIMARY KEY,
    Name NVARCHAR(256),
    NormalizedName NVARCHAR(256),
    ConcurrencyStamp NVARCHAR(MAX)
);

-- Default Roles Data
INSERT INTO Roles (Id, Name, NormalizedName) VALUES
('1', 'Investor', 'INVESTOR'),
('2', 'Advisor', 'ADVISOR'),
('3', 'OperationsTeam', 'OPERATIONSTEAM');
```

### 3. AccountRequests Table (CLARIFIED - Complete Field Specification)
```sql
CREATE TABLE AccountRequests (
    Id INT PRIMARY KEY IDENTITY(1,1),

    -- Required Fields
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,

    -- Optional Fields
    Message NVARCHAR(2000) NULL,
    PreferredContactMethod NVARCHAR(20) NULL, -- 'Email', 'Phone', 'Both'
    InvestmentInterest NVARCHAR(500) NULL,

    -- System Fields
    Status NVARCHAR(50) DEFAULT 'Pending', -- 'Pending', 'Contacted', 'Converted', 'Closed'
    ProcessedBy NVARCHAR(450) NULL FOREIGN KEY REFERENCES Users(Id),
    ProcessedDate DATETIME2 NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_PreferredContactMethod CHECK (PreferredContactMethod IN ('Email', 'Phone', 'Both'))
);
```

### 4. Investors Table
```sql
CREATE TABLE Investors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES Users(Id),
    InvestorType NVARCHAR(50) NOT NULL, -- 'Individual','Joint','Trust','Entity','IRA'

    -- Basic Info
    IsUSCitizen BIT,
    IsAccredited BIT,
    AccreditationType INT, -- 1-6 based on type

    -- Profile Completion Tracking (9 sections)
    SectionCompletionFlags INT DEFAULT 0, -- Bit flags for 9 sections
    ProfileCompletionPercentage DECIMAL(5,2) DEFAULT 0.00,

    -- Status
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2,

    CONSTRAINT CK_InvestorType CHECK (InvestorType IN ('Individual','Joint','Trust','Entity','IRA')),
    CONSTRAINT CK_AccreditationType CHECK (AccreditationType BETWEEN 1 AND 6)
);
```

### 5. GeneralInfo Table
```sql
CREATE TABLE GeneralInfo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    -- Universal Fields
    Name NVARCHAR(300),
    Email NVARCHAR(256),
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(500),
    City NVARCHAR(100),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),

    -- Individual/Joint/IRA Fields
    DateOfBirth DATE,
    SSN NVARCHAR(MAX), -- Encrypted
    DriversLicensePath NVARCHAR(500),
    W9DocumentPath NVARCHAR(500),

    -- Trust/Entity Fields
    DateOfFormation DATE,
    TIN_EIN NVARCHAR(MAX), -- Encrypted
    PurposeOfFormation NVARCHAR(1000),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2
);
```

### 6. JointAccountHolders Table
```sql
CREATE TABLE JointAccountHolders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    -- Account Holder Details
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    SSN NVARCHAR(MAX), -- Encrypted
    Email NVARCHAR(256),
    PhoneNumber NVARCHAR(20),

    -- Documents
    DriversLicensePath NVARCHAR(500),
    W9DocumentPath NVARCHAR(500),

    -- Joint Account Type (6 options)
    JointAccountType NVARCHAR(100), -- See business requirements for 6 types

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_JointAccountType CHECK (
        JointAccountType IN (
            'Joint Tenants with Right of Survivorship',
            'Joint Tenants in Common',
            'Tenants by the Entirety',
            'A Married Person, as my Sole and Separate Property',
            'Husband and Wife, as Community Property with Rights of Survivorship',
            'Husband and Wife, as Community Property'
        )
    )
);
```

### 7. TrustInvestorDetail Table
```sql
CREATE TABLE TrustInvestorDetail (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    TrustType NVARCHAR(50), -- 'Revocable Trust', 'Irrevocable Trust'
    TrustCertificatePath NVARCHAR(500), -- Required
    TrustAgreementPath NVARCHAR(500), -- Optional

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_TrustType CHECK (TrustType IN ('Revocable Trust', 'Irrevocable Trust'))
);
```

### 8. EntityInvestorDetail Table
```sql
CREATE TABLE EntityInvestorDetail (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    EntityType NVARCHAR(50), -- 'LLC', 'Corporation', 'Partnership'

    -- Conditional Documents (based on EntityType)
    CertificateOfIncorporationPath NVARCHAR(500), -- Corporation
    CorporateResolutionPath NVARCHAR(500), -- Corporation
    ArticlesOfIncorporationPath NVARCHAR(500), -- LLC
    LLCOperatingAgreementPath NVARCHAR(500), -- LLC
    CertificateOfBeneficialOwnersPath NVARCHAR(500), -- LLC
    LLCCertificatePath NVARCHAR(500), -- LLC (if no Operating Agreement)

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_EntityType CHECK (EntityType IN ('LLC', 'Corporation', 'Partnership'))
);
```

### 9. IRAInvestorDetail Table (STANDARDIZED - 5 Types Only)
```sql
CREATE TABLE IRAInvestorDetail (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    IRAName NVARCHAR(300),
    AccountType NVARCHAR(50) NOT NULL, -- STANDARDIZED to 5 types only
    IRAAccountNumber NVARCHAR(100),

    -- Custodian Information
    CustodianName NVARCHAR(200),
    CustodianPhoneNumber NVARCHAR(20),
    CustodianFaxNumber NVARCHAR(20),

    -- Rollover Information
    IsRollingOverToCNB BIT DEFAULT 0,
    CurrentIRAStatementPath NVARCHAR(500),
    AssetsLiquidated BIT,

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    -- CONSTRAINT: Only 5 standardized IRA types allowed
    CONSTRAINT CK_IRAAccountType CHECK (
        AccountType IN (
            'Traditional IRA',
            'Roth IRA',
            'SEP IRA',
            'Inherited IRA',
            'Inherited Roth IRA'
        )
    )
);
```

### 10. EquityOwners Table (Trust/Entity)
```sql
CREATE TABLE EquityOwners (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    OwnershipPercentage DECIMAL(5,2),
    DriversLicensePath NVARCHAR(500),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 11. PrimaryInvestorInfo Table
```sql
CREATE TABLE PrimaryInvestorInfo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    -- Personal Information (16 fields)
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    LegalStreetAddress NVARCHAR(500),
    City NVARCHAR(100),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),
    Email NVARCHAR(256),
    CellPhoneNumber NVARCHAR(20),
    IsMarried BIT,
    SSN NVARCHAR(MAX), -- Encrypted
    DateOfBirth DATE,
    DriversLicenseNumber NVARCHAR(50),
    DriversLicenseExpirationDate DATE,
    Occupation NVARCHAR(200),
    EmployerName NVARCHAR(200),
    FormerProfession NVARCHAR(200), -- If retired
    HasAlternateAddress BIT,

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2
);
```

### 12. BrokerDealerAffiliation Table
```sql
CREATE TABLE BrokerDealerAffiliation (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    IsEmployeeOfBrokerDealer BIT,
    BrokerDealerName NVARCHAR(200),
    IsRelatedToEmployeeOfBrokerDealer BIT,
    RelatedBrokerDealerName NVARCHAR(200),
    RelatedEmployeeName NVARCHAR(200),
    Relationship NVARCHAR(100),
    IsSeniorOfficerDirectorShareholder BIT,
    IsManagerMemberExecutiveOfficer BIT,

    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 13. InvestmentExperience Table
```sql
CREATE TABLE InvestmentExperience (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    AssetClass NVARCHAR(100) NOT NULL, -- 10 asset classes
    ExperienceLevel NVARCHAR(20) NOT NULL, -- 'None', '1-5 Years', 'Over 5 Years'
    OtherSpecification NVARCHAR(200), -- For "Other Alternative Investments"

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_AssetClass CHECK (
        AssetClass IN (
            'Individual Stocks',
            'Fixed Income',
            'Mutual Funds/ETFs',
            'Direct Real Estate Holdings',
            'Real Estate Investments (REITs, RE Funds, DSTs/1031s)',
            'LPs and/or LLCs',
            'Private Equity/Debt',
            'Oil and Gas',
            'Tax Mitigation Investments',
            'Other Alternative Investments'
        )
    ),
    CONSTRAINT CK_ExperienceLevel CHECK (
        ExperienceLevel IN ('None', '1-5 Years', 'Over 5 Years')
    )
);
```

### 14. IncomeInformation Table
```sql
CREATE TABLE IncomeInformation (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    LowestIncomePreviousTwoYears DECIMAL(18,2),
    AnticipatedIncomeThisYear DECIMAL(18,2),
    RelyingOnJointIncomeForAccreditation BIT,

    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 15. SourceOfFunds Table (Multi-Select Support)
```sql
CREATE TABLE SourceOfFunds (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    FundingSource NVARCHAR(100) NOT NULL, -- 7 options

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_FundingSource CHECK (
        FundingSource IN (
            'Income From earnings',
            'Inheritance',
            'Insurance Proceeds',
            '1031 Exchange/Sale of Property',
            'Investment Proceeds',
            'Pension/IRA Savings',
            'Sale of Business'
        )
    )
);
```

### 16. TaxRates Table (Multi-Select Support)
```sql
CREATE TABLE TaxRates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    TaxRateRange NVARCHAR(20) NOT NULL, -- 5 ranges

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_TaxRateRange CHECK (
        TaxRateRange IN ('0-15%', '16-25%', '26-30%', '31-35%', 'Over 36%')
    )
);
```

### 17. InvestorAccreditation Table
```sql
CREATE TABLE InvestorAccreditation (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    AccreditationType INT NOT NULL, -- 1-6
    IsVerified BIT DEFAULT 0,
    VerificationDate DATETIME2,
    VerifiedBy NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),
    Notes NVARCHAR(1000),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_AccreditationType_Range CHECK (AccreditationType BETWEEN 1 AND 6)
);
```

### 18. AccreditationDocuments Table
```sql
CREATE TABLE AccreditationDocuments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorAccreditationId INT NOT NULL FOREIGN KEY REFERENCES InvestorAccreditation(Id) ON DELETE CASCADE,

    DocumentType NVARCHAR(100) NOT NULL, -- Based on accreditation type
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT,
    ContentType NVARCHAR(100),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 19. Beneficiaries Table
```sql
CREATE TABLE Beneficiaries (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    BeneficiaryType NVARCHAR(20) NOT NULL, -- 'Primary', 'Contingent'

    -- Required Fields (9 fields)
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100),
    LastName NVARCHAR(100) NOT NULL,
    SSN NVARCHAR(MAX), -- Encrypted
    DateOfBirth DATE NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    RelationshipToOwner NVARCHAR(100) NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    ZipCode NVARCHAR(10) NOT NULL,
    PercentageOfBenefit DECIMAL(5,2) NOT NULL, -- Must total 100% per type

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2,

    CONSTRAINT CK_BeneficiaryType CHECK (BeneficiaryType IN ('Primary', 'Contingent')),
    CONSTRAINT CK_PercentageRange CHECK (PercentageOfBenefit BETWEEN 0 AND 100)
);
```

### 20. PersonalFinancialStatements Table
```sql
CREATE TABLE PersonalFinancialStatements (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    PFSFilePath NVARCHAR(500),
    IsCompleted BIT DEFAULT 0,
    CompletedDate DATETIME2,

    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 21. FinancialGoals Table
```sql
CREATE TABLE FinancialGoals (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    -- Single Select Fields
    LiquidityNeeds NVARCHAR(20), -- 'Low', 'Medium', 'High'
    YearsToInvest NVARCHAR(20), -- '0-5 Years', '6-11 Years', '12+ Years'
    InvestmentObjective NVARCHAR(50), -- 'Income', 'Growth', 'Income & Growth'
    RiskTolerance NVARCHAR(20), -- 'Low Risk', 'Medium Risk', 'High Risk'

    -- Free Text
    TellUsMore NVARCHAR(2000),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_LiquidityNeeds CHECK (LiquidityNeeds IN ('Low', 'Medium', 'High')),
    CONSTRAINT CK_YearsToInvest CHECK (YearsToInvest IN ('0-5 Years', '6-11 Years', '12+ Years')),
    CONSTRAINT CK_InvestmentObjective CHECK (InvestmentObjective IN ('Income', 'Growth', 'Income & Growth')),
    CONSTRAINT CK_RiskTolerance CHECK (RiskTolerance IN ('Low Risk', 'Medium Risk', 'High Risk'))
);
```

### 22. InvestmentGoals Table (Multi-Select Support)
```sql
CREATE TABLE InvestmentGoals (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    Goal NVARCHAR(100) NOT NULL, -- 7 investment goals

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_InvestmentGoal CHECK (
        Goal IN (
            'Defer taxes',
            'Protect principal',
            'Grow principal',
            'Consistent cash flow',
            'Diversification',
            'Retirement',
            'Estate/legacy planning'
        )
    )
);
```

### 23. InvestorDocuments Table
```sql
CREATE TABLE InvestorDocuments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    DocumentName NVARCHAR(255) NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT,
    ContentType NVARCHAR(100),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### 24. FinancialTeamMembers Table
```sql
CREATE TABLE FinancialTeamMembers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id) ON DELETE CASCADE,

    MemberType NVARCHAR(50) NOT NULL, -- 'CPA', 'Financial Advisor', 'Estate Attorney'
    Name NVARCHAR(200) NOT NULL,
    Email NVARCHAR(256),
    PhoneNumber NVARCHAR(20),

    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT CK_MemberType CHECK (MemberType IN ('CPA', 'Financial Advisor', 'Estate Attorney'))
);
```

### 25. Offerings Table (SIMPLIFIED Structure)
```sql
CREATE TABLE Offerings (
    Id INT PRIMARY KEY IDENTITY(1,1),

    -- Simplified Structure (per clarification)
    OfferingName NVARCHAR(300) NOT NULL, -- Main field for Portal display
    OfferingType NVARCHAR(100), -- Optional categorization
    Description NVARCHAR(MAX),

    -- Optional Fields
    TotalValue DECIMAL(18,2),
    Status NVARCHAR(50) DEFAULT 'Raising', -- 'Raising', 'Closed', 'Coming Soon'

    -- Media
    ImagePath NVARCHAR(500),
    PDFPath NVARCHAR(500),

    -- Audit Fields
    CreatedBy NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedBy NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),
    ModifiedDate DATETIME2,
    IsActive BIT DEFAULT 1,

    CONSTRAINT CK_OfferingStatus CHECK (Status IN ('Raising', 'Closed', 'Coming Soon'))
);
```

### 26. InvestorInvestments Table (Many-to-Many Relationship - CONFIRMED)
```sql
CREATE TABLE InvestorInvestments (
    Id INT PRIMARY KEY IDENTITY(1,1),

    -- Many-to-Many Relationship
    InvestorId INT NOT NULL FOREIGN KEY REFERENCES Investors(Id),
    OfferingId INT NOT NULL FOREIGN KEY REFERENCES Offerings(Id),

    -- Investment Details
    InvestmentAmount DECIMAL(18,2),
    InvestmentDate DATE,
    Status NVARCHAR(50) DEFAULT 'Active',

    -- Audit Fields
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2,

    -- Prevent duplicate investments
    CONSTRAINT UQ_InvestorOffering UNIQUE (InvestorId, OfferingId)
);
```

### 27. InvestmentTracker Table (Portal CRM - 21 Fields)
```sql
CREATE TABLE InvestmentTracker (
    Id INT PRIMARY KEY IDENTITY(1,1),

    -- Basic Information
    OfferingName NVARCHAR(300) NOT NULL, -- Simplified field name
    ClientName NVARCHAR(200),
    Status NVARCHAR(100), -- 13 predefined statuses
    AdvisorId NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),
    InvestmentType NVARCHAR(100), -- 5 predefined types

    -- Investment Details
    LeadOwner NVARCHAR(200), -- Licensed Rep
    Relationship NVARCHAR(200), -- Ex: "Connor's Client"
    InvestmentHeldInNamesOf NVARCHAR(500),
    DateInvestmentClosed DATE,
    OriginalEquityInvestmentAmount DECIMAL(18,2),

    -- Revenue Tracking
    TotalTWSAUM DECIMAL(18,2),
    RepCommissionAmount DECIMAL(18,2),
    TWSRevenue DECIMAL(18,2),
    DSTRevenue DECIMAL(18,2),
    AltRevenue DECIMAL(18,2),
    TaxStrategyRevenue DECIMAL(18,2),
    OilAndGasRevenue DECIMAL(18,2),
    InitialVsRecurringRevenue DECIMAL(18,2),

    -- AUM Breakdown
    ALTAUM DECIMAL(18,2),
    DSTAUM DECIMAL(18,2),
    OilAndGasAUM DECIMAL(18,2),
    TaxStrategyAUM DECIMAL(18,2),

    -- Marketing
    MarketingSource NVARCHAR(200),
    ReferredBy NVARCHAR(200),

    -- Notes
    Notes NVARCHAR(MAX),

    -- Audit Fields
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2,
    ModifiedBy NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),

    -- Status Constraint (13 options)
    CONSTRAINT CK_InvestmentStatus CHECK (
        Status IN (
            'Need DST to come out',
            'Onboarding',
            'Investment Paperwork',
            'BD Approval',
            'Docs done, need pprty to close',
            'At custodian for signature',
            'In Backup Status at Sponsor',
            'Sponsor',
            'QI',
            'Wire Requested',
            'Funded',
            'Closed WON',
            'Full Cycle Investment'
        )
    ),

    -- Investment Type Constraint (5 options)
    CONSTRAINT CK_PortalInvestmentType CHECK (
        InvestmentType IN (
            'Private Placement 506(c)',
            '1031 Exchange Investment',
            'Universal Offering',
            'Tax Strategy',
            'Roth IRA Conversion'
        )
    )
);
```

---

## Indexes for Performance

### Critical Performance Indexes
```sql
-- User and Authentication
CREATE INDEX IX_Users_Email ON Users(NormalizedEmail);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);

-- Account Requests
CREATE INDEX IX_AccountRequests_Email ON AccountRequests(Email);
CREATE INDEX IX_AccountRequests_Status ON AccountRequests(Status);
CREATE INDEX IX_AccountRequests_CreatedDate ON AccountRequests(CreatedDate);

-- Investors
CREATE INDEX IX_Investors_UserId ON Investors(UserId);
CREATE INDEX IX_Investors_InvestorType ON Investors(InvestorType);
CREATE INDEX IX_Investors_IsActive ON Investors(IsActive);
CREATE INDEX IX_Investors_ProfileCompletion ON Investors(ProfileCompletionPercentage);

-- IRA Account Types (for filtering)
CREATE INDEX IX_IRAInvestorDetail_AccountType ON IRAInvestorDetail(AccountType);

-- Relationships
CREATE INDEX IX_InvestorInvestments_InvestorId ON InvestorInvestments(InvestorId);
CREATE INDEX IX_InvestorInvestments_OfferingId ON InvestorInvestments(OfferingId);
CREATE INDEX IX_InvestorInvestments_Status ON InvestorInvestments(Status);

-- Portal/CRM Performance
CREATE INDEX IX_InvestmentTracker_AdvisorId ON InvestmentTracker(AdvisorId);
CREATE INDEX IX_InvestmentTracker_Status ON InvestmentTracker(Status);
CREATE INDEX IX_InvestmentTracker_InvestmentType ON InvestmentTracker(InvestmentType);
CREATE INDEX IX_InvestmentTracker_OfferingName ON InvestmentTracker(OfferingName);

-- Beneficiaries
CREATE INDEX IX_Beneficiaries_InvestorId ON Beneficiaries(InvestorId);
CREATE INDEX IX_Beneficiaries_BeneficiaryType ON Beneficiaries(BeneficiaryType);

-- Documents
CREATE INDEX IX_InvestorDocuments_InvestorId ON InvestorDocuments(InvestorId);
CREATE INDEX IX_AccreditationDocuments_InvestorAccreditationId ON AccreditationDocuments(InvestorAccreditationId);

-- Financial Team
CREATE INDEX IX_FinancialTeamMembers_InvestorId ON FinancialTeamMembers(InvestorId);
CREATE INDEX IX_FinancialTeamMembers_MemberType ON FinancialTeamMembers(MemberType);
```

---

## Data Validation Rules

### 1. IRA Account Types (STANDARDIZED)
```sql
-- Ensure only 5 valid IRA types across all contexts
CONSTRAINT CK_ValidIRATypes CHECK (
    AccountType IN (
        'Traditional IRA',
        'Roth IRA',
        'SEP IRA',
        'Inherited IRA',
        'Inherited Roth IRA'
    )
);
```

### 2. Beneficiary Percentage Validation
```sql
-- Business Rule: Primary beneficiaries must total 100%
-- Business Rule: Contingent beneficiaries must total 100%
-- This will be enforced at application level with stored procedures
```

### 3. Profile Completion Tracking
```sql
-- Section completion flags (bit flags for 9 sections)
-- Bit 0: General Info
-- Bit 1: Primary Investor Info
-- Bit 2: Accreditation
-- Bit 3: Beneficiaries
-- Bit 4: Personal Financial Statement
-- Bit 5: Financial Goals
-- Bit 6: Documents
-- Bit 7: Financial Team
-- Bit 8: My Investments
```

---

## Stored Procedures for Business Rules

### 1. Update Profile Completion
```sql
CREATE PROCEDURE UpdateProfileCompletion
    @InvestorId INT,
    @CompletedSection INT -- 0-8 for sections 1-9
AS
BEGIN
    DECLARE @CurrentFlags INT;
    DECLARE @NewFlags INT;
    DECLARE @CompletionPercentage DECIMAL(5,2);

    -- Get current completion flags
    SELECT @CurrentFlags = SectionCompletionFlags FROM Investors WHERE Id = @InvestorId;

    -- Set the bit for completed section
    SET @NewFlags = @CurrentFlags | POWER(2, @CompletedSection);

    -- Calculate completion percentage
    SET @CompletionPercentage = (
        SELECT COUNT(*) * 100.0 / 9.0
        FROM (
            SELECT 0 AS BitPosition UNION SELECT 1 UNION SELECT 2 UNION
            SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION
            SELECT 6 UNION SELECT 7 UNION SELECT 8
        ) Bits
        WHERE (@NewFlags & POWER(2, BitPosition)) > 0
    );

    -- Update investor record
    UPDATE Investors
    SET SectionCompletionFlags = @NewFlags,
        ProfileCompletionPercentage = @CompletionPercentage,
        ModifiedDate = GETUTCDATE()
    WHERE Id = @InvestorId;
END;
```

### 2. Validate Beneficiary Percentages
```sql
CREATE PROCEDURE ValidateBeneficiaryPercentages
    @InvestorId INT,
    @BeneficiaryType NVARCHAR(20),
    @IsValid BIT OUTPUT
AS
BEGIN
    DECLARE @TotalPercentage DECIMAL(5,2);

    SELECT @TotalPercentage = SUM(PercentageOfBenefit)
    FROM Beneficiaries
    WHERE InvestorId = @InvestorId
    AND BeneficiaryType = @BeneficiaryType;

    SET @IsValid = CASE WHEN @TotalPercentage = 100.00 THEN 1 ELSE 0 END;
END;
```

---

**END OF DATABASE SCHEMA DOCUMENT**

*This schema contains ALL finalized table structures with every clarification applied. All relationships are properly defined with foreign keys and constraints. Ready for Entity Framework Code-First implementation.*