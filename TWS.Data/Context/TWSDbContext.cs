using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TWS.Core.Constants;
using TWS.Data.Entities.Accreditation;
using TWS.Data.Entities.Beneficiaries;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.Documents;
using TWS.Data.Entities.Financial;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Entities.Identity;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Data.Context
{
    /// <summary>
    /// Main database context for TWS Investment Platform
    /// Inherits from IdentityDbContext for ASP.NET Identity integration
    /// Reference: DatabaseSchema.md, Architecture.md
    /// </summary>
    public class TWSDbContext : IdentityDbContext<ApplicationUser>
    {
        public TWSDbContext(DbContextOptions<TWSDbContext> options) : base(options)
        {
        }

        // DbSet Properties

        /// <summary>
        /// ApplicationUsers table (extended ASP.NET Identity Users)
        /// </summary>
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>
        /// AccountRequests table - stores account requests from potential investors
        /// Reference: DatabaseSchema.md Table 3
        /// </summary>
        public DbSet<AccountRequest> AccountRequests { get; set; }

        /// <summary>
        /// RefreshTokens table - stores JWT refresh tokens for user authentication
        /// Reference: SecurityDesign.md
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// InvestorProfiles table - base investor profile linked to ApplicationUser
        /// Reference: DatabaseSchema.md Table 4
        /// </summary>
        public DbSet<InvestorProfile> InvestorProfiles { get; set; }

        /// <summary>
        /// IndividualInvestorDetails table - type-specific details for individual investors
        /// Reference: DatabaseSchema.md Table 5
        /// </summary>
        public DbSet<IndividualInvestorDetail> IndividualInvestorDetails { get; set; }

        /// <summary>
        /// JointInvestorDetails table - type-specific details for joint investors
        /// Reference: DatabaseSchema.md Table 6
        /// </summary>
        public DbSet<JointInvestorDetail> JointInvestorDetails { get; set; }

        /// <summary>
        /// IRAInvestorDetails table - type-specific details for IRA investors
        /// Reference: DatabaseSchema.md Table 7
        /// </summary>
        public DbSet<IRAInvestorDetail> IRAInvestorDetails { get; set; }

        /// <summary>
        /// TrustInvestorDetails table - type-specific details for trust investors
        /// Reference: DatabaseSchema.md Table 8
        /// </summary>
        public DbSet<TrustInvestorDetail> TrustInvestorDetails { get; set; }

        /// <summary>
        /// EntityInvestorDetails table - type-specific details for entity investors
        /// Reference: DatabaseSchema.md Table 9
        /// </summary>
        public DbSet<EntityInvestorDetail> EntityInvestorDetails { get; set; }

        /// <summary>
        /// IndividualGeneralInfo table - general info for individual investors
        /// Reference: DatabaseSchema.md Table 10
        /// </summary>
        public DbSet<IndividualGeneralInfo> IndividualGeneralInfos { get; set; }

        /// <summary>
        /// JointGeneralInfo table - general info for joint investors
        /// Reference: DatabaseSchema.md Table 11
        /// </summary>
        public DbSet<JointGeneralInfo> JointGeneralInfos { get; set; }

        /// <summary>
        /// JointAccountHolders table - account holders for joint investments
        /// Reference: DatabaseSchema.md Table 12
        /// </summary>
        public DbSet<JointAccountHolder> JointAccountHolders { get; set; }

        /// <summary>
        /// IRAGeneralInfo table - general info for IRA investors
        /// Reference: DatabaseSchema.md Table 13
        /// </summary>
        public DbSet<IRAGeneralInfo> IRAGeneralInfos { get; set; }

        /// <summary>
        /// TrustGeneralInfo table - general info for trust investors
        /// Reference: DatabaseSchema.md Table 14
        /// </summary>
        public DbSet<TrustGeneralInfo> TrustGeneralInfos { get; set; }

        /// <summary>
        /// TrustGrantors table - grantors for trust investors
        /// Reference: DatabaseSchema.md Table 15
        /// </summary>
        public DbSet<TrustGrantor> TrustGrantors { get; set; }

        /// <summary>
        /// EntityGeneralInfo table - general info for entity investors
        /// Reference: DatabaseSchema.md Table 16
        /// </summary>
        public DbSet<EntityGeneralInfo> EntityGeneralInfos { get; set; }

        /// <summary>
        /// EntityEquityOwners table - equity owners for entity investors
        /// Reference: DatabaseSchema.md Table 17
        /// </summary>
        public DbSet<EntityEquityOwner> EntityEquityOwners { get; set; }

        /// <summary>
        /// PrimaryInvestorInfos table - primary investor personal and financial information
        /// Reference: DatabaseSchema.md Table 18
        /// </summary>
        public DbSet<Entities.PrimaryInvestorInfo.PrimaryInvestorInfo> PrimaryInvestorInfos { get; set; }

        /// <summary>
        /// BrokerAffiliations table - broker-dealer affiliation information
        /// Reference: DatabaseSchema.md Table 19
        /// </summary>
        public DbSet<BrokerAffiliation> BrokerAffiliations { get; set; }

        /// <summary>
        /// InvestmentExperiences table - investment experience with various asset classes
        /// Reference: DatabaseSchema.md Table 20
        /// </summary>
        public DbSet<InvestmentExperience> InvestmentExperiences { get; set; }

        /// <summary>
        /// SourceOfFunds table - sources of investment funds
        /// Reference: DatabaseSchema.md Table 21
        /// </summary>
        public DbSet<SourceOfFunds> SourceOfFunds { get; set; }

        /// <summary>
        /// TaxRates table - tax rate information
        /// Reference: DatabaseSchema.md Table 22
        /// </summary>
        public DbSet<TaxRate> TaxRates { get; set; }

        /// <summary>
        /// InvestorAccreditations table - investor accreditation information
        /// Reference: DatabaseSchema.md Table 23
        /// </summary>
        public DbSet<InvestorAccreditation> InvestorAccreditations { get; set; }

        /// <summary>
        /// AccreditationDocuments table - documents uploaded as proof for accreditation
        /// Reference: DatabaseSchema.md Table 24
        /// </summary>
        public DbSet<AccreditationDocument> AccreditationDocuments { get; set; }

        /// <summary>
        /// Beneficiaries table - stores beneficiary information for investor profiles
        /// Reference: DatabaseSchema.md Table 19
        /// </summary>
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        /// <summary>
        /// PersonalFinancialStatements table - stores personal financial statement documents
        /// Reference: DatabaseSchema.md Table 26
        /// </summary>
        public DbSet<PersonalFinancialStatement> PersonalFinancialStatements { get; set; }

        /// <summary>
        /// FinancialGoals table - stores investor's investment objectives and risk preferences
        /// Reference: DatabaseSchema.md Table 27
        /// </summary>
        public DbSet<FinancialGoals> FinancialGoals { get; set; }

        /// <summary>
        /// InvestorDocuments table - stores document uploads for investor profiles
        /// Reference: DatabaseSchema.md Table 23
        /// </summary>
        public DbSet<InvestorDocument> InvestorDocuments { get; set; }

        /// <summary>
        /// FinancialTeamMembers table - stores financial team member information for investor profiles
        /// Reference: DatabaseSchema.md Table 29
        /// </summary>
        public DbSet<FinancialTeamMember> FinancialTeamMembers { get; set; }

        /// <summary>
        /// Offerings table - stores investment opportunities
        /// Reference: DatabaseSchema.md Table 30
        /// </summary>
        public DbSet<TWS.Data.Entities.Portal.Offering> Offerings { get; set; }

        /// <summary>
        /// OfferingDocuments table - stores documents associated with offerings
        /// </summary>
        public DbSet<TWS.Data.Entities.Portal.OfferingDocument> OfferingDocuments { get; set; }

        /// <summary>
        /// InvestorInvestments table - junction table for many-to-many relationship between InvestorProfile and Offering
        /// Reference: DatabaseSchema.md Table 31
        /// </summary>
        public DbSet<InvestorInvestment> InvestorInvestments { get; set; }

        /// <summary>
        /// InvestmentTrackers table - Portal/CRM tracking for investments with financial metrics
        /// Reference: DatabaseSchema.md Table 32
        /// </summary>
        public DbSet<TWS.Data.Entities.Portal.InvestmentTracker> InvestmentTrackers { get; set; }

        // Future DbSets - Will be added in subsequent phases:
        // public DbSet<InvestmentProfile> InvestmentProfiles { get; set; }
        // public DbSet<BankingInfo> BankingInfos { get; set; }

        /// <summary>
        /// Configure entity relationships and seed data
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base configuration for ASP.NET Identity tables
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TWSDbContext).Assembly);

            // Seed ASP.NET Identity Roles
            SeedRoles(modelBuilder);

            // Future entity configurations will be added here:
            // - ApplicationUser configurations
            // - Profile entity relationships
            // - Investment relationships
            // - Document relationships
            // - Banking info encryption configurations
        }

        /// <summary>
        /// Seeds the three default roles for TWS platform
        /// Roles: Investor, Advisor, OperationsTeam
        /// Reference: DatabaseSchema.md Table 2, RoleConstants.cs
        /// </summary>
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = RoleConstants.Investor,
                    NormalizedName = RoleConstants.Investor.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = RoleConstants.Advisor,
                    NormalizedName = RoleConstants.Advisor.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = RoleConstants.OperationsTeam,
                    NormalizedName = RoleConstants.OperationsTeam.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }
    }
}