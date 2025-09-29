TWS.Solution/
│
├── TWS.Web/                                    # MVC Web Application
│   ├── Controllers/
│   │   ├── HomeController.cs                  # Homepage with Request Account button
│   │   ├── AccountController.cs               # Identity authentication
│   │   ├── RequestAccountController.cs        # Handle account requests
│   │   ├── InvestorController.cs              # Investor type selection
│   │   ├── InvestorProfileController.cs       # Profile management
│   │   ├── GeneralInfoController.cs           
│   │   ├── PrimaryInvestorInfoController.cs   
│   │   ├── AccreditationController.cs         
│   │   ├── BeneficiaryController.cs           
│   │   ├── PersonalFinancialStatementController.cs
│   │   ├── FinancialGoalsController.cs        
│   │   ├── DocumentController.cs              
│   │   ├── FinancialTeamController.cs         
│   │   ├── MyInvestmentsController.cs         
│   │   └── PortalController.cs                # CRM (Advisors/Operations only)
│   │
│   ├── Views/
│   │   ├── Home/
│   │   │   └── Index.cshtml
│   │   ├── Account/
│   │   │   ├── Login.cshtml
│   │   │   ├── Register.cshtml
│   │   │   ├── ForgotPassword.cshtml
│   │   │   ├── ResetPassword.cshtml
│   │   │   └── AccessDenied.cshtml
│   │   ├── RequestAccount/
│   │   │   ├── Create.cshtml
│   │   │   └── Success.cshtml
│   │   ├── Investor/
│   │   │   └── SelectType.cshtml
│   │   ├── InvestorProfile/
│   │   │   └── Index.cshtml
│   │   ├── GeneralInfo/
│   │   │   ├── Individual.cshtml
│   │   │   ├── Joint.cshtml
│   │   │   ├── Trust.cshtml
│   │   │   ├── Entity.cshtml
│   │   │   └── IRA.cshtml
│   │   ├── PrimaryInvestorInfo/
│   │   │   ├── Index.cshtml
│   │   │   ├── _BrokerAffiliation.cshtml
│   │   │   ├── _InvestmentExperience.cshtml
│   │   │   └── _IncomeInfo.cshtml
│   │   ├── Accreditation/
│   │   │   ├── Index.cshtml
│   │   │   └── _DocumentUpload.cshtml
│   │   ├── Beneficiary/
│   │   │   ├── Index.cshtml
│   │   │   └── _BeneficiaryForm.cshtml
│   │   ├── PersonalFinancialStatement/
│   │   │   └── Index.cshtml
│   │   ├── FinancialGoals/
│   │   │   └── Index.cshtml
│   │   ├── Document/
│   │   │   └── Index.cshtml
│   │   ├── FinancialTeam/
│   │   │   └── Index.cshtml
│   │   ├── MyInvestments/
│   │   │   └── Gallery.cshtml
│   │   ├── Portal/
│   │   │   ├── Dashboard.cshtml
│   │   │   └── _TrackerPopup.cshtml
│   │   └── Shared/
│   │       ├── _Layout.cshtml
│   │       ├── _LoginPartial.cshtml
│   │       ├── _ValidationScriptsPartial.cshtml
│   │       └── Error.cshtml
│   │
│   ├── ViewModels/
│   │   ├── InvestorTypeViewModel.cs
│   │   ├── GeneralInfoViewModel.cs
│   │   ├── PrimaryInvestorInfoViewModel.cs
│   │   ├── AccreditationViewModel.cs
│   │   ├── BeneficiaryViewModel.cs
│   │   ├── FinancialGoalsViewModel.cs
│   │   ├── DocumentViewModel.cs
│   │   ├── PortalDashboardViewModel.cs
│   │   └── InvestmentTrackerViewModel.cs
│   │
│   ├── Services/
│   │   ├── HttpServices/
│   │   │   ├── BaseHttpService.cs
│   │   │   ├── InvestorHttpService.cs
│   │   │   ├── ProfileHttpService.cs
│   │   │   └── PortalHttpService.cs
│   │   └── CacheServices/
│   │       └── InvestorCacheService.cs
│   │
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── site.css
│   │   ├── js/
│   │   │   ├── site.js
│   │   │   ├── investor-profile.js
│   │   │   └── portal.js
│   │   ├── lib/                               # Bootstrap, jQuery, etc.
│   │   └── uploads/                           # Document storage
│   │       └── investors/
│   │           └── {investorId}/
│   │
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   └── Startup.cs
│
├── TWS.API/                                    # WebAPI Project
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── RequestAccountController.cs
│   │   ├── InvestorController.cs
│   │   ├── GeneralInfoController.cs
│   │   ├── PrimaryInvestorInfoController.cs
│   │   ├── AccreditationController.cs
│   │   ├── BeneficiaryController.cs
│   │   ├── PersonalFinancialStatementController.cs
│   │   ├── FinancialGoalsController.cs
│   │   ├── DocumentController.cs
│   │   ├── FinancialTeamController.cs
│   │   ├── MyInvestmentsController.cs
│   │   ├── PortalController.cs
│   │   └── OfferingController.cs
│   │
│   ├── Filters/
│   │   ├── ModelValidationFilter.cs
│   │   └── AuthorizeRoleFilter.cs
│   │
│   ├── Middleware/
│   │   └── ErrorHandlingMiddleware.cs
│   │
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   └── Startup.cs
│
├── TWS.Core/                                   # Domain Layer (Business Logic Only)
│   ├── Interfaces/                            # Repository and Service Interfaces
│   │   ├── IRepositories/
│   │   │   ├── IGenericRepository.cs
│   │   │   ├── IInvestorRepository.cs
│   │   │   ├── IGeneralInfoRepository.cs
│   │   │   ├── IPrimaryInvestorInfoRepository.cs
│   │   │   ├── IAccreditationRepository.cs
│   │   │   ├── IBeneficiaryRepository.cs
│   │   │   ├── IPersonalFinancialStatementRepository.cs
│   │   │   ├── IFinancialGoalsRepository.cs
│   │   │   ├── IDocumentRepository.cs
│   │   │   ├── IFinancialTeamRepository.cs
│   │   │   ├── IPortalRepository.cs
│   │   │   ├── IOfferingRepository.cs
│   │   │   ├── IInvestmentTrackerRepository.cs
│   │   │   ├── IRequestAccountRepository.cs
│   │   │   ├── IJointInvestorDetailRepository.cs
│   │   │   ├── IJointAccountHolderRepository.cs
│   │   │   ├── IIRAInvestorDetailRepository.cs
│   │   │   ├── ITrustInvestorDetailRepository.cs
│   │   │   ├── ITrustGrantorRepository.cs
│   │   │   ├── IEntityInvestorDetailRepository.cs
│   │   │   ├── IEntityEquityOwnerRepository.cs
│   │   │   ├── IBrokerDealerAffiliationRepository.cs
│   │   │   ├── IInvestmentExperienceRepository.cs
│   │   │   ├── ISourceOfFundsRepository.cs
│   │   │   ├── ITaxRateRepository.cs
│   │   │   ├── IAccreditationDocumentRepository.cs
│   │   │   ├── IInvestorInvestmentRepository.cs
│   │   │   └── IUserRepository.cs
│   │   │
│   │   └── IServices/
│   │       ├── IInvestorService.cs
│   │       ├── IGeneralInfoService.cs
│   │       ├── IPrimaryInvestorInfoService.cs
│   │       ├── IAccreditationService.cs
│   │       ├── IBeneficiaryService.cs
│   │       ├── IPersonalFinancialStatementService.cs
│   │       ├── IFinancialGoalsService.cs
│   │       ├── IDocumentService.cs
│   │       ├── IFinancialTeamService.cs
│   │       ├── IPortalService.cs
│   │       ├── IOfferingService.cs
│   │       ├── IMyInvestmentsService.cs
│   │       ├── IRequestAccountService.cs
│   │       ├── IAuthenticationService.cs
│   │       ├── IEmailService.cs
│   │       ├── IFileStorageService.cs
│   │       ├── IDataProtectionService.cs
│   │       ├── IPDFService.cs
│   │       ├── IValidationService.cs
│   │       ├── IProfileCompletionService.cs
│   │       ├── INotificationService.cs
│   │       ├── IReportingService.cs
│   │       └── IAuditService.cs
│   │
│   ├── Enums/                                 # Shared Enumerations
│   │   ├── InvestorType.cs                    # 5 types
│   │   ├── AccreditationType.cs               # 6 types
│   │   ├── JointAccountType.cs                # 6 types
│   │   ├── IRAType.cs                         # 6 initial types for selection
│   │   ├── IRAAccountType.cs                  # EXACTLY 5 types for General Info
│   │   ├── TrustType.cs                       # 2 types
│   │   ├── EntityType.cs                      # 3 types
│   │   ├── InvestmentExperienceLevel.cs       # 3 levels
│   │   ├── AssetClass.cs                      # 9 types
│   │   ├── LiquidityNeeds.cs                  # 3 levels
│   │   ├── InvestmentTimeline.cs              # 3 ranges
│   │   ├── InvestmentObjective.cs             # 3 types
│   │   ├── RiskTolerance.cs                   # 3 levels
│   │   ├── BeneficiaryType.cs                 # 2 types
│   │   ├── FinancialTeamMemberType.cs         # 3 types
│   │   ├── InvestmentStatus.cs                # 13 statuses
│   │   ├── PortalInvestmentType.cs            # 5 types
│   │   ├── SourceOfFundsType.cs               # 7 types
│   │   └── TaxRateRange.cs                    # 5 ranges
│   │
│   ├── Constants/                             # Shared Constants
│   │   ├── RoleConstants.cs
│   │   ├── ValidationConstants.cs
│   │   └── DocumentConstants.cs
│   │
│   └── DTOs/                                  # Data Transfer Objects
│       ├── Request/
│       │   ├── Account/
│       │   │   ├── LoginRequest.cs
│       │   │   ├── RegisterRequest.cs
│       │   │   ├── ForgotPasswordRequest.cs
│       │   │   └── ResetPasswordRequest.cs
│       │   ├── Investor/
│       │   │   ├── CreateInvestorRequest.cs
│       │   │   ├── UpdateInvestorRequest.cs
│       │   │   └── SelectInvestorTypeRequest.cs
│       │   ├── Profile/
│       │   │   ├── GeneralInfoRequest.cs
│       │   │   ├── JointGeneralInfoRequest.cs
│       │   │   ├── TrustGeneralInfoRequest.cs
│       │   │   ├── EntityGeneralInfoRequest.cs
│       │   │   ├── IRAGeneralInfoRequest.cs
│       │   │   └── PrimaryInvestorInfoRequest.cs
│       │   ├── Accreditation/
│       │   │   ├── AccreditationRequest.cs
│       │   │   └── AccreditationDocumentRequest.cs
│       │   ├── Beneficiary/
│       │   │   ├── BeneficiaryRequest.cs
│       │   │   └── BulkBeneficiaryRequest.cs
│       │   ├── Financial/
│       │   │   ├── FinancialGoalsRequest.cs
│       │   │   └── PersonalFinancialStatementRequest.cs
│       │   └── Portal/
│       │       ├── InvestmentTrackerRequest.cs
│       │       └── UpdateTrackerStatusRequest.cs
│       │
│       └── Response/
│           ├── ApiResponse.cs
│           ├── LoginResponse.cs
│           ├── InvestorResponse.cs
│           ├── GeneralInfoResponse.cs
│           ├── AccreditationResponse.cs
│           ├── BeneficiaryResponse.cs
│           ├── FinancialGoalsResponse.cs
│           ├── InvestmentTrackerResponse.cs
│           ├── OfferingResponse.cs
│           └── PortalDashboardResponse.cs
│
├── TWS.Infra/                                 # Infrastructure Layer (Service Implementations)
│   ├── Services/
│   │   ├── Core/
│   │   │   ├── InvestorService.cs
│   │   │   ├── GeneralInfoService.cs
│   │   │   ├── PrimaryInvestorInfoService.cs
│   │   │   ├── AccreditationService.cs
│   │   │   ├── BeneficiaryService.cs
│   │   │   ├── PersonalFinancialStatementService.cs
│   │   │   ├── FinancialGoalsService.cs
│   │   │   ├── DocumentService.cs
│   │   │   ├── FinancialTeamService.cs
│   │   │   ├── PortalService.cs
│   │   │   ├── OfferingService.cs
│   │   │   ├── MyInvestmentsService.cs
│   │   │   └── RequestAccountService.cs
│   │   │
│   │   ├── Security/
│   │   │   ├── AuthenticationService.cs       # JWT token generation
│   │   │   ├── DataProtectionService.cs       # Encryption/Decryption
│   │   │   └── AuditService.cs               # Audit logging
│   │   │
│   │   ├── Storage/
│   │   │   ├── AzureBlobStorageService.cs    # Azure Blob integration
│   │   │   └── FileStorageService.cs         # Local file storage
│   │   │
│   │   ├── Communication/
│   │   │   ├── EmailService.cs               # Email notifications
│   │   │   └── NotificationService.cs        # System notifications
│   │   │
│   │   └── Utilities/
│   │       ├── PDFService.cs                 # PDF generation
│   │       ├── ValidationService.cs          # Business validation
│   │       ├── ProfileCompletionService.cs   # Profile % calculation
│   │       └── ReportingService.cs          # Report generation
│   │
│   ├── Mapping/
│   │   ├── AutoMapperProfile.cs
│   │   ├── InvestorMappingProfile.cs
│   │   ├── GeneralInfoMappingProfile.cs
│   │   ├── AccreditationMappingProfile.cs
│   │   └── PortalMappingProfile.cs
│   │
│   └── Helpers/
│       ├── FileHelper.cs
│       ├── EncryptionHelper.cs
│       └── ValidationHelper.cs
│
└── TWS.Data/                                  # Data Access Layer
    ├── Entities/                              # DATABASE ENTITIES WITH EF ANNOTATIONS
    │   ├── Identity/
    │   │   └── ApplicationUser.cs
    │   ├── Core/
    │   │   ├── Investor.cs
    │   │   ├── GeneralInfo.cs
    │   │   ├── PrimaryInvestorInformation.cs
    │   │   ├── BrokerDealerAffiliation.cs
    │   │   ├── InvestmentExperience.cs
    │   │   ├── SourceOfFunds.cs
    │   │   ├── TaxRates.cs
    │   │   └── AccountRequest.cs
    │   ├── TypeSpecific/
    │   │   ├── JointInvestorDetail.cs
    │   │   ├── JointAccountHolder.cs
    │   │   ├── IRAInvestorDetail.cs
    │   │   ├── TrustInvestorDetail.cs
    │   │   ├── TrustGrantor.cs
    │   │   ├── EntityInvestorDetail.cs
    │   │   └── EntityEquityOwner.cs
    │   ├── Accreditation/
    │   │   ├── InvestorAccreditation.cs
    │   │   └── AccreditationDocument.cs
    │   ├── Financial/
    │   │   ├── PersonalFinancialStatement.cs
    │   │   ├── FinancialGoals.cs
    │   │   ├── Beneficiary.cs
    │   │   └── FinancialTeamMember.cs
    │   ├── Documents/
    │   │   └── InvestorDocument.cs
    │   └── Portal/
    │       ├── Offering.cs
    │       ├── InvestmentTracker.cs
    │       └── InvestorInvestment.cs
    │
    ├── Context/
    │   └── TWSDbContext.cs                   # Inherits IdentityDbContext<ApplicationUser>
    │
    ├── Repositories/                          # Repository Implementations
    │   ├── Base/
    │   │   └── GenericRepository.cs
    │   ├── Core/
    │   │   ├── InvestorRepository.cs
    │   │   ├── GeneralInfoRepository.cs
    │   │   ├── PrimaryInvestorInfoRepository.cs
    │   │   ├── RequestAccountRepository.cs
    │   │   ├── UserRepository.cs
    │   │   ├── BrokerDealerAffiliationRepository.cs
    │   │   ├── InvestmentExperienceRepository.cs
    │   │   ├── SourceOfFundsRepository.cs
    │   │   └── TaxRateRepository.cs
    │   ├── TypeSpecific/
    │   │   ├── JointInvestorDetailRepository.cs
    │   │   ├── JointAccountHolderRepository.cs
    │   │   ├── IRAInvestorDetailRepository.cs
    │   │   ├── TrustInvestorDetailRepository.cs
    │   │   ├── TrustGrantorRepository.cs
    │   │   ├── EntityInvestorDetailRepository.cs
    │   │   └── EntityEquityOwnerRepository.cs
    │   ├── Financial/
    │   │   ├── AccreditationRepository.cs
    │   │   ├── AccreditationDocumentRepository.cs
    │   │   ├── BeneficiaryRepository.cs
    │   │   ├── PersonalFinancialStatementRepository.cs
    │   │   ├── FinancialGoalsRepository.cs
    │   │   ├── FinancialTeamRepository.cs
    │   │   └── DocumentRepository.cs
    │   └── Portal/
    │       ├── OfferingRepository.cs
    │       ├── InvestmentTrackerRepository.cs
    │       └── InvestorInvestmentRepository.cs
    │
    ├── Configurations/                        # Entity Framework Configurations
    │   ├── Identity/
    │   │   └── ApplicationUserConfiguration.cs
    │   ├── Core/
    │   │   ├── InvestorConfiguration.cs
    │   │   ├── GeneralInfoConfiguration.cs
    │   │   └── PrimaryInvestorInfoConfiguration.cs
    │   ├── TypeSpecific/
    │   │   ├── JointInvestorConfiguration.cs
    │   │   ├── IRAInvestorConfiguration.cs
    │   │   ├── TrustInvestorConfiguration.cs
    │   │   └── EntityInvestorConfiguration.cs
    │   ├── Financial/
    │   │   ├── AccreditationConfiguration.cs
    │   │   ├── BeneficiaryConfiguration.cs
    │   │   └── FinancialGoalsConfiguration.cs
    │   └── Portal/
    │       ├── OfferingConfiguration.cs
    │       └── InvestmentTrackerConfiguration.cs
    │
    ├── Seeds/
    │   └── RoleSeeder.cs                     # Seed 3 roles
    │
    └── Migrations/                            # EF Migrations






NuGet Package Requirements
TWS.Web 

<PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="8.0.*" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.*" />
<PackageReference Include="Microsoft.AspNetCore.Mvc" Version="8.0.*" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="13.*" />
<PackageReference Include="Serilog.AspNetCore" Version="8.*" />
<PackageReference Include="Serilog.Sinks.File" Version="5.*" />
TWS.API

<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.*" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.*" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.*" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="13.*" />
<PackageReference Include="Serilog.AspNetCore" Version="8.*" />
<PackageReference Include="Serilog.Sinks.File" Version="5.*" />
TWS.Core

<PackageReference Include="Microsoft.Extensions.Identity.Stores" Version="8.0.*" />
TWS.Infra

<PackageReference Include="AutoMapper" Version="13.*" />
<PackageReference Include="iTextSharp" Version="5.*" />
TWS.Data

<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.*" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.*" />


Key Implementation Notes
Authentication: ASP.NET  Identity with 3 roles (Investor, Advisor, OperationsTeam)
Portal Access: Authorize attribute with roles "Advisor,OperationsTeam"
Logging: Serilog to Logs/ folder in each project
Caching: IMemoryCache in Web project only
API Response: Generic ApiResponse<T> wrapper
Validation: Model validation using DataAnnotations