# TWS Investment Platform - Implementation Roadmap

**Version**: 1.0
**Purpose**: Step-by-step implementation guide with small, manageable chunks
**Status**: Ready for Implementation

---

## Table of Contents

1. [Phase 0: Project Foundation & Setup](#phase-0-project-foundation--setup)
2. [Phase 1: Core Infrastructure](#phase-1-core-infrastructure)
3. [Phase 2: Authentication & User Management](#phase-2-authentication--user-management)
4. [Phase 3: Request Account Module](#phase-3-request-account-module)
5. [Phase 4: Investor Type Selection](#phase-4-investor-type-selection)
6. [Phase 5: Profile Section 1 - General Info (All 5 Types)](#phase-5-profile-section-1---general-info-all-5-types)
7. [Phase 6: Profile Section 2 - Primary Investor Information](#phase-6-profile-section-2---primary-investor-information)
8. [Phase 7: Profile Section 3 - Accreditation](#phase-7-profile-section-3---accreditation)
9. [Phase 8: Profile Section 4 - Beneficiaries](#phase-8-profile-section-4---beneficiaries)
10. [Phase 9: Profile Section 5 - Personal Financial Statement](#phase-9-profile-section-5---personal-financial-statement)
11. [Phase 10: Profile Section 6 - Financial Goals](#phase-10-profile-section-6---financial-goals)
12. [Phase 11: Profile Section 7 - Documents](#phase-11-profile-section-7---documents)
13. [Phase 12: Profile Section 8 - Financial Team](#phase-12-profile-section-8---financial-team)
14. [Phase 13: Profile Section 9 - My Investments](#phase-13-profile-section-9---my-investments)
15. [Phase 14: Portal (CRM) Module](#phase-14-portal-crm-module)
16. [Phase 15: Azure Integration & Security](#phase-15-azure-integration--security)
17. [Phase 16: Testing & Deployment](#phase-16-testing--deployment)

---

## Phase 0: Project Foundation & Setup

### Step 0.1: Create Solution Structure
**Time Estimate**: 30 minutes

#### Tasks:
1. Create solution file `TWS.Solution.sln`
2. Create project folders:
   - `TWS.Web` (ASP.NET Core MVC)
   - `TWS.API` (ASP.NET Core Web API)
   - `TWS.Core` (Class Library)
   - `TWS.Infra` (Class Library)
   - `TWS.Data` (Class Library)

**Commands**:
```bash
# Create solution
dotnet new sln -n TWS.Solution

# Create projects
dotnet new mvc -n TWS.Web
dotnet new webapi -n TWS.API
dotnet new classlib -n TWS.Core
dotnet new classlib -n TWS.Infra
dotnet new classlib -n TWS.Data

# Add projects to solution
dotnet sln add TWS.Web/TWS.Web.csproj
dotnet sln add TWS.API/TWS.API.csproj
dotnet sln add TWS.Core/TWS.Core.csproj
dotnet sln add TWS.Infra/TWS.Infra.csproj
dotnet sln add TWS.Data/TWS.Data.csproj

# Add project references
dotnet add TWS.Web reference TWS.Core
dotnet add TWS.Web reference TWS.Infra
dotnet add TWS.API reference TWS.Core
dotnet add TWS.API reference TWS.Infra
dotnet add TWS.Infra reference TWS.Core
dotnet add TWS.Infra reference TWS.Data
dotnet add TWS.Data reference TWS.Core
```

**Build & Verify**:
```bash
dotnet build
```

---

### Step 0.2: Install NuGet Packages
**Time Estimate**: 20 minutes

#### TWS.Web Packages:
```bash
cd TWS.Web
dotnet add package Microsoft.AspNetCore.Identity.UI --version 8.0.*
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.*
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 13.*
dotnet add package Serilog.AspNetCore --version 8.*
dotnet add package Serilog.Sinks.File --version 5.*
```

#### TWS.API Packages:
```bash
cd ../TWS.API
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.*
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.*
dotnet add package Swashbuckle.AspNetCore --version 6.*
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.*
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 13.*
dotnet add package Serilog.AspNetCore --version 8.*
dotnet add package Serilog.Sinks.File --version 5.*
dotnet add package Azure.Identity --version 1.10.*
dotnet add package Azure.Security.KeyVault.Secrets --version 4.5.*
dotnet add package Azure.Storage.Blobs --version 12.19.*
```

#### TWS.Core Packages:
```bash
cd ../TWS.Core
dotnet add package Microsoft.Extensions.Identity.Stores --version 8.0.*
```

#### TWS.Infra Packages:
```bash
cd ../TWS.Infra
dotnet add package AutoMapper --version 13.*
dotnet add package iTextSharp --version 5.*
```

#### TWS.Data Packages:
```bash
cd ../TWS.Data
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.*
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.*
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.*
```

**Build & Verify**:
```bash
cd ..
dotnet build
```

---

### Step 0.3: Create Folder Structure
**Time Estimate**: 15 minutes

#### TWS.Core Folders:
```
TWS.Core/
├── Constants/
├── DTOs/
│   ├── Request/
│   └── Response/
├── Enums/
└── Interfaces/
    ├── IRepositories/
    └── IServices/
```

#### TWS.Data Folders:
```
TWS.Data/
├── Configurations/
│   ├── Core/
│   ├── Financial/
│   ├── Identity/
│   ├── Portal/
│   └── TypeSpecific/
├── Context/
├── Entities/
│   ├── Accreditation/
│   ├── Core/
│   ├── Documents/
│   ├── Financial/
│   ├── Identity/
│   ├── Portal/
│   └── TypeSpecific/
├── Migrations/
├── Repositories/
│   ├── Base/
│   ├── Core/
│   ├── Financial/
│   ├── Portal/
│   └── TypeSpecific/
└── Seeds/
```

#### TWS.Infra Folders:
```
TWS.Infra/
├── Helpers/
├── Mapping/
└── Services/
    ├── Communication/
    ├── Core/
    ├── Security/
    ├── Storage/
    └── Utilities/
```

#### TWS.API Folders:
```
TWS.API/
├── Controllers/
├── Filters/
└── Middleware/
```

#### TWS.Web Folders:
```
TWS.Web/
├── Constants/
├── Controllers/
├── Services/
│   ├── CacheServices/
│   └── HttpServices/
├── ViewModels/
├── Views/
│   ├── Account/
│   ├── Home/
│   ├── RequestAccount/
│   ├── Investor/
│   ├── Portal/
│   └── Shared/
│       └── Partials/
└── wwwroot/
    ├── css/
    ├── js/
    └── uploads/
```

---

## Phase 1: Core Infrastructure

### Step 1.1: Create Enums
**Time Estimate**: 1 hour
**Location**: `TWS.Core/Enums/`

Create the following enum files (18 enums total):

#### InvestorType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum InvestorType
    {
        Individual = 1,
        Joint = 2,
        Trust = 3,
        Entity = 4,
        IRA = 5
    }
}
```

#### AccreditationType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum AccreditationType
    {
        NetWorth = 1,           // $1M net worth
        Income = 2,             // $200K/$300K income
        Series7 = 3,            // License
        Series65 = 4,           // License
        Series82 = 5,           // License
        ProfessionalRole = 6    // SEC-registered
    }
}
```

#### JointAccountType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum JointAccountType
    {
        JointTenantsWithRightOfSurvivorship = 1,
        JointTenantsInCommon = 2,
        TenantsByTheEntirety = 3,
        MarriedPersonSoleProperty = 4,
        HusbandWifeCommunityPropertyWithRights = 5,
        HusbandWifeCommunityProperty = 6
    }
}
```

#### IRAAccountType.cs
```csharp
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
```

**Note**: Previous IRAType enum has been removed. Only IRAAccountType is used throughout the application.

#### TrustType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum TrustType
    {
        RevocableTrust = 1,
        IrrevocableTrust = 2
    }
}
```

#### EntityType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum EntityType
    {
        LLC = 1,
        Corporation = 2,
        Partnership = 3
    }
}
```

#### InvestmentExperienceLevel.cs
```csharp
namespace TWS.Core.Enums
{
    public enum InvestmentExperienceLevel
    {
        None = 0,
        OneToFiveYears = 1,
        OverFiveYears = 2
    }
}
```

#### AssetClass.cs
```csharp
namespace TWS.Core.Enums
{
    public enum AssetClass
    {
        IndividualStocks = 1,
        FixedIncome = 2,
        MutualFundsETFs = 3,
        DirectRealEstateHoldings = 4,
        RealEstateInvestments = 5,  // REITs, RE Funds, DSTs/1031s
        LPsAndLLCs = 6,
        PrivateEquityDebt = 7,
        OilAndGas = 8,
        TaxMitigationInvestments = 9,
        OtherAlternativeInvestments = 10
    }
}
```

#### LiquidityNeeds.cs
```csharp
namespace TWS.Core.Enums
{
    public enum LiquidityNeeds
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
```

#### InvestmentTimeline.cs
```csharp
namespace TWS.Core.Enums
{
    public enum InvestmentTimeline
    {
        ZeroToFiveYears = 1,
        SixToElevenYears = 2,
        TwelvePlusYears = 3
    }
}
```

#### InvestmentObjective.cs
```csharp
namespace TWS.Core.Enums
{
    public enum InvestmentObjective
    {
        Income = 1,
        Growth = 2,
        IncomeAndGrowth = 3
    }
}
```

#### RiskTolerance.cs
```csharp
namespace TWS.Core.Enums
{
    public enum RiskTolerance
    {
        LowRisk = 1,
        MediumRisk = 2,
        HighRisk = 3
    }
}
```

#### BeneficiaryType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum BeneficiaryType
    {
        Primary = 1,
        Contingent = 2
    }
}
```

#### FinancialTeamMemberType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum FinancialTeamMemberType
    {
        CPA = 1,
        FinancialAdvisor = 2,
        EstateAttorney = 3
    }
}
```

#### InvestmentStatus.cs
```csharp
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
```

#### PortalInvestmentType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum PortalInvestmentType
    {
        PrivatePlacement506c = 1,
        Exchange1031Investment = 2,
        UniversalOffering = 3,
        TaxStrategy = 4,
        RothIRAConversion = 5
    }
}
```

#### SourceOfFundsType.cs
```csharp
namespace TWS.Core.Enums
{
    public enum SourceOfFundsType
    {
        IncomeFromEarnings = 1,
        Inheritance = 2,
        InsuranceProceeds = 3,
        Exchange1031SaleOfProperty = 4,
        InvestmentProceeds = 5,
        PensionIRASavings = 6,
        SaleOfBusiness = 7
    }
}
```

#### TaxRateRange.cs
```csharp
namespace TWS.Core.Enums
{
    public enum TaxRateRange
    {
        ZeroTo15 = 1,      // 0-15%
        SixteenTo25 = 2,   // 16-25%
        TwentySixTo30 = 3, // 26-30%
        ThirtyOneTo35 = 4, // 31-35%
        Over36 = 5          // Over 36%
    }
}
```

#### OfferingStatus.cs
```csharp
namespace TWS.Core.Enums
{
    public enum OfferingStatus
    {
        Raising = 1,      // Currently raising capital
        Closed = 2,       // Offering closed
        ComingSoon = 3    // Coming soon
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Core
```

---

### Step 1.2: Create Constants
**Time Estimate**: 45 minutes
**Location**: `TWS.Core/Constants/`

#### RoleConstants.cs
```csharp
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
```

#### ValidationConstants.cs
```csharp
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
```

#### DocumentConstants.cs
```csharp
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
```

**Build & Verify**:
```bash
dotnet build TWS.Core
```

---

### Step 1.3: Create Base Entity and Repository Interface
**Time Estimate**: 30 minutes

#### IGenericRepository.cs
**Location**: `TWS.Core/Interfaces/IRepositories/IGenericRepository.cs`

```csharp
using System.Linq.Expressions;

namespace TWS.Core.Interfaces.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<bool> SaveChangesAsync();
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Core
```

---

### Step 1.4: Create ApplicationUser Entity
**Time Estimate**: 20 minutes
**Location**: `TWS.Data/Entities/Identity/ApplicationUser.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TWS.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Data
```

---

### Step 1.5: Create DbContext
**Time Estimate**: 30 minutes
**Location**: `TWS.Data/Context/TWSDbContext.cs`

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TWS.Data.Entities.Identity;

namespace TWS.Data.Context
{
    public class TWSDbContext : IdentityDbContext<ApplicationUser>
    {
        public TWSDbContext(DbContextOptions<TWSDbContext> options) : base(options)
        {
        }

        // DbSets will be added progressively in subsequent phases

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply configurations
            // Configurations will be added progressively
        }
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Data
```

---

### Step 1.6: Create ApiResponse Wrapper
**Time Estimate**: 15 minutes
**Location**: `TWS.Core/DTOs/Response/ApiResponse.cs`

```csharp
namespace TWS.Core.DTOs.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(T? data, string message = "", int statusCode = 200)
        {
            Success = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        public ApiResponse(string message, int statusCode = 400, List<string>? errors = null)
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            Errors = errors ?? new List<string>();
        }

        public static ApiResponse<T> SuccessResponse(T? data, string message = "Operation successful", int statusCode = 200)
        {
            return new ApiResponse<T>(data, message, statusCode);
        }

        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400, List<string>? errors = null)
        {
            return new ApiResponse<T>(message, statusCode, errors);
        }
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Core
```

---

### Step 1.7: Configure appsettings.json
**Time Estimate**: 20 minutes

#### TWS.API/appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TWS_Dev;User=root;Password=your_password;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyWithAtLeast32Characters123456",
    "Issuer": "TWS_API",
    "Audience": "TWS_Web",
    "ExpiryMinutes": 60
  },
  "Azure": {
    "TenantId": "",
    "ClientId": "",
    "ClientSecret": ""
  },
  "KeyVault": {
    "Uri": ""
  },
  "BlobStorage": {
    "AccountName": "",
    "ConnectionString": ""
  }
}
```

**Build Solution**:
```bash
dotnet build
```

---

## Phase 2: Authentication & User Management

### Step 2.1: Create User-Related Entities
**Time Estimate**: 30 minutes
**Location**: `TWS.Data/Entities/Core/`

#### AccountRequest.cs
```csharp
using System.ComponentModel.DataAnnotations;

namespace TWS.Data.Entities.Core
{
    public class AccountRequest
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Message { get; set; }

        [MaxLength(20)]
        public string? PreferredContactMethod { get; set; }  // Email, Phone, Both

        [MaxLength(500)]
        public string? InvestmentInterest { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";  // Pending, Contacted, Converted, Closed

        public string? ProcessedBy { get; set; }  // FK to Users

        public DateTime? ProcessedDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Data
```

---

### Step 2.2: Add AccountRequest DbSet to DbContext
**Time Estimate**: 5 minutes
**Location**: `TWS.Data/Context/TWSDbContext.cs`

Add to DbContext:
```csharp
public DbSet<AccountRequest> AccountRequests { get; set; }
```

**Build & Verify**:
```bash
dotnet build TWS.Data
```

---

### Step 2.3: Create Account Request DTOs
**Time Estimate**: 30 minutes

#### CreateAccountRequestRequest.cs
**Location**: `TWS.Core/DTOs/Request/Account/CreateAccountRequestRequest.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Account
{
    public class CreateAccountRequestRequest
    {
        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(20)]
        [Phone(ErrorMessage = "Invalid phone format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Message { get; set; }

        [MaxLength(20)]
        public string? PreferredContactMethod { get; set; }

        [MaxLength(500)]
        public string? InvestmentInterest { get; set; }
    }
}
```

#### AccountRequestResponse.cs
**Location**: `TWS.Core/DTOs/Response/AccountRequestResponse.cs`

```csharp
namespace TWS.Core.DTOs.Response
{
    public class AccountRequestResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? PreferredContactMethod { get; set; }
        public string? InvestmentInterest { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? ProcessedByUserId { get; set; }
    }
}
```

**Build & Verify**:
```bash
dotnet build TWS.Core
```

---

### Step 2.4: Create Account Request Repository Interface and Implementation
**Time Estimate**: 45 minutes

#### IRequestAccountRepository.cs
**Location**: `TWS.Core/Interfaces/IRepositories/IRequestAccountRepository.cs`

```csharp
using TWS.Data.Entities.Core;

namespace TWS.Core.Interfaces.IRepositories
{
    public interface IRequestAccountRepository : IGenericRepository<AccountRequest>
    {
        Task<IEnumerable<AccountRequest>> GetPendingRequestsAsync();
        Task<IEnumerable<AccountRequest>> GetProcessedRequestsAsync();
        Task<AccountRequest?> GetByEmailAsync(string email);
    }
}
```

#### RequestAccountRepository.cs
**Location**: `TWS.Data/Repositories/Core/RequestAccountRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Core;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Core
{
    public class RequestAccountRepository : GenericRepository<AccountRequest>, IRequestAccountRepository
    {
        public RequestAccountRepository(TWSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AccountRequest>> GetPendingRequestsAsync()
        {
            return await _context.AccountRequests
                .Where(ar => ar.Status == "Pending")
                .OrderByDescending(ar => ar.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccountRequest>> GetProcessedRequestsAsync()
        {
            return await _context.AccountRequests
                .Where(ar => ar.Status != "Pending")
                .OrderByDescending(ar => ar.ProcessedDate)
                .ToListAsync();
        }

        public async Task<AccountRequest?> GetByEmailAsync(string email)
        {
            return await _context.AccountRequests
                .FirstOrDefaultAsync(ar => ar.Email == email);
        }
    }
}
```

---

### Step 2.5: Create GenericRepository Base Implementation
**Time Estimate**: 1 hour
**Location**: `TWS.Data/Repositories/Base/GenericRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;

namespace TWS.Data.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TWSDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(TWSDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
```

**Build & Verify**:
```bash
dotnet build
```

---

### Step 2.6: Configure Database Connection and Run Initial Migration
**Time Estimate**: 30 minutes

#### Update TWS.API/Program.cs
Add DbContext configuration:

```csharp
using Microsoft.EntityFrameworkCore;
using TWS.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<TWSDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Rest of configuration...
```

#### Create Initial Migration
```bash
dotnet ef migrations add InitialCreate --project TWS.Data --startup-project TWS.API

# Update database
dotnet ef database update --project TWS.Data --startup-project TWS.API
```

**Build & Verify**:
```bash
dotnet build
```

---

## Summary of Phase 0-2

**What We've Built:**
1. ✅ Solution structure with 5 projects
2. ✅ All NuGet packages installed
3. ✅ Folder structure created
4. ✅ 18 Enum types (including OfferingStatus, standardized IRAAccountType)
5. ✅ 3 Constants classes
6. ✅ Base repository interface and implementation
7. ✅ ApplicationUser entity
8. ✅ DbContext with Identity
9. ✅ ApiResponse wrapper
10. ✅ AccountRequest entity with repository
11. ✅ DTOs for Account Requests
12. ✅ Initial database migration

**Important Clarifications Applied:**
- ✅ IRA Types: Standardized to 5 types (Traditional, Roth, SEP, Inherited, Inherited Roth) - used in both initial selection and General Info
- ✅ Offering Status: Three values (Raising, Closed, Coming Soon)
- ✅ Many-to-many relationship: InvestorInvestments junction table confirmed

**Next Steps:**
Continue with Phase 3: Request Account Module (API and Web implementation)

---

## Phase 3: Request Account Module

### Step 3.1: Create Request Account Service Interface and Implementation
**Time Estimate**: 1 hour

#### IRequestAccountService.cs
**Location**: `TWS.Core/Interfaces/IServices/IRequestAccountService.cs`

```csharp
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response;

namespace TWS.Core.Interfaces.IServices
{
    public interface IRequestAccountService
    {
        Task<ApiResponse<AccountRequestResponse>> CreateRequestAsync(CreateAccountRequestRequest request);
        Task<ApiResponse<List<AccountRequestResponse>>> GetAllRequestsAsync();
        Task<ApiResponse<AccountRequestResponse>> GetRequestByIdAsync(int id);
        Task<ApiResponse<AccountRequestResponse>> ProcessRequestAsync(int id, string processedBy, string? notes);
        Task<ApiResponse<bool>> DeleteRequestAsync(int id);
    }
}
```

#### RequestAccountService.cs
**Location**: `TWS.Infra/Services/Core/RequestAccountService.cs`

```csharp
using AutoMapper;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Core;

namespace TWS.Infra.Services.Core
{
    public class RequestAccountService : IRequestAccountService
    {
        private readonly IRequestAccountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RequestAccountService> _logger;

        public RequestAccountService(
            IRequestAccountRepository repository,
            IMapper mapper,
            ILogger<RequestAccountService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<AccountRequestResponse>> CreateRequestAsync(CreateAccountRequestRequest request)
        {
            try
            {
                // Check for duplicate email
                var existing = await _repository.GetByEmailAsync(request.Email);
                if (existing != null && existing.Status == "Pending")
                {
                    return ApiResponse<AccountRequestResponse>.ErrorResponse(
                        "A request with this email already exists and is pending",
                        409);
                }

                var accountRequest = new AccountRequest
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Message = request.Message,
                    PreferredContactMethod = request.PreferredContactMethod,
                    InvestmentInterest = request.InvestmentInterest,
                    Status = "Pending",
                    CreatedDate = DateTime.UtcNow
                };

                var created = await _repository.AddAsync(accountRequest);
                var response = _mapper.Map<AccountRequestResponse>(created);

                _logger.LogInformation("Account request created: {Id} - {Email}", created.Id, created.Email);

                return ApiResponse<AccountRequestResponse>.SuccessResponse(
                    response,
                    "Account request submitted successfully",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account request for {Email}", request.Email);
                return ApiResponse<AccountRequestResponse>.ErrorResponse(
                    "An error occurred while processing your request",
                    500);
            }
        }

        public async Task<ApiResponse<List<AccountRequestResponse>>> GetAllRequestsAsync()
        {
            try
            {
                var requests = await _repository.GetAllAsync();
                var response = _mapper.Map<List<AccountRequestResponse>>(requests);

                return ApiResponse<List<AccountRequestResponse>>.SuccessResponse(
                    response,
                    "Account requests retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account requests");
                return ApiResponse<List<AccountRequestResponse>>.ErrorResponse(
                    "An error occurred while retrieving account requests",
                    500);
            }
        }

        public async Task<ApiResponse<AccountRequestResponse>> GetRequestByIdAsync(int id)
        {
            try
            {
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                {
                    return ApiResponse<AccountRequestResponse>.ErrorResponse(
                        $"Account request with ID {id} not found",
                        404);
                }

                var response = _mapper.Map<AccountRequestResponse>(request);
                return ApiResponse<AccountRequestResponse>.SuccessResponse(
                    response,
                    "Account request retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account request {Id}", id);
                return ApiResponse<AccountRequestResponse>.ErrorResponse(
                    "An error occurred while retrieving the account request",
                    500);
            }
        }

        public async Task<ApiResponse<AccountRequestResponse>> ProcessRequestAsync(int id, string processedBy, string? notes)
        {
            try
            {
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                {
                    return ApiResponse<AccountRequestResponse>.ErrorResponse(
                        $"Account request with ID {id} not found",
                        404);
                }

                request.Status = "Contacted";
                request.ProcessedBy = processedBy;
                request.ProcessedDate = DateTime.UtcNow;
                request.Message = notes ?? request.Message;

                await _repository.UpdateAsync(request);

                var response = _mapper.Map<AccountRequestResponse>(request);
                _logger.LogInformation("Account request processed: {Id} by {ProcessedBy}", id, processedBy);

                return ApiResponse<AccountRequestResponse>.SuccessResponse(
                    response,
                    "Account request processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing account request {Id}", id);
                return ApiResponse<AccountRequestResponse>.ErrorResponse(
                    "An error occurred while processing the account request",
                    500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteRequestAsync(int id)
        {
            try
            {
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        $"Account request with ID {id} not found",
                        404);
                }

                await _repository.DeleteAsync(request);
                _logger.LogInformation("Account request deleted: {Id}", id);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Account request deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account request {Id}", id);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting the account request",
                    500);
            }
        }
    }
}
```

**Build & Verify**:
```bash
dotnet build
```

---

### Step 3.2: Create AutoMapper Profile
**Time Estimate**: 30 minutes
**Location**: `TWS.Infra/Mapping/AutoMapperProfile.cs`

```csharp
using AutoMapper;
using TWS.Core.DTOs.Response;
using TWS.Data.Entities.Core;

namespace TWS.Infra.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // AccountRequest mappings
            CreateMap<AccountRequest, AccountRequestResponse>()
                .ForMember(dest => dest.IsProcessed,
                    opt => opt.MapFrom(src => src.Status != "Pending"))
                .ForMember(dest => dest.ProcessedByUserId,
                    opt => opt.MapFrom(src => src.ProcessedBy));
        }
    }
}
```

**Build & Verify**:
```bash
dotnet build
```

---

### Step 3.3: Create Request Account API Controller
**Time Estimate**: 45 minutes
**Location**: `TWS.API/Controllers/RequestAccountController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.Constants;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestAccountController : ControllerBase
    {
        private readonly IRequestAccountService _requestAccountService;
        private readonly ILogger<RequestAccountController> _logger;

        public RequestAccountController(
            IRequestAccountService requestAccountService,
            ILogger<RequestAccountController> logger)
        {
            _requestAccountService = requestAccountService;
            _logger = logger;
        }

        /// <summary>
        /// Submit a new account request (Public endpoint)
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRequest([FromBody] CreateAccountRequestRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestAccountService.CreateRequestAsync(request);

            return result.Success
                ? StatusCode(result.StatusCode, result)
                : StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get all account requests (Advisor/Operations only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _requestAccountService.GetAllRequestsAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get account request by ID (Advisor/Operations only)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var result = await _requestAccountService.GetRequestByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Process account request (Advisor/Operations only)
        /// </summary>
        [HttpPut("{id}/process")]
        [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
        public async Task<IActionResult> ProcessRequest(int id, [FromBody] ProcessRequestRequest request)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
            var result = await _requestAccountService.ProcessRequestAsync(id, userId, request.Notes);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Delete account request (Operations only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.OperationsTeam)]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var result = await _requestAccountService.DeleteRequestAsync(id);
            return result.Success ? NoContent() : StatusCode(result.StatusCode, result);
        }
    }

    public class ProcessRequestRequest
    {
        public string? Notes { get; set; }
    }
}
```

**Build & Verify**:
```bash
dotnet build
```

---

## Checkpoint: Build Entire Solution

**Time Estimate**: 5 minutes

```bash
# Navigate to solution root
cd C:\Users\mahav\TWS2

# Build entire solution
dotnet build

# Verify no errors
```

**Expected Result**: Solution builds successfully with 0 errors, 0 warnings.

---

## Continuation Instructions

This roadmap continues with:

- **Phase 4**: Investor Type Selection (Create Investor entity, DTOs, services, controllers)
- **Phase 5-13**: Nine Profile Sections (Each section builds incrementally)
- **Phase 14**: Portal/CRM Module
- **Phase 15**: Azure Integration & Security
- **Phase 16**: Testing & Deployment

**Total Estimated Time**: 200-250 hours of development

**Note**: Each phase follows the same pattern:
1. Create Entities with Data Annotations
2. Add DbSets to DbContext
3. Create Entity Configurations (Fluent API)
4. Create Repository Interfaces and Implementations
5. Create DTOs (Request/Response)
6. Create Service Interfaces and Implementations
7. Create API Controllers
8. Create Web Controllers and Views
9. Add AutoMapper Profiles
10. Run Migration
11. Build and Test

**Would you like me to continue with Phase 4 and beyond?**