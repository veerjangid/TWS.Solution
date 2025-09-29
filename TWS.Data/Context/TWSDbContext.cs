using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TWS.Core.Constants;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.Identity;
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

        // Future DbSets - Will be added in subsequent phases:
        // public DbSet<GeneralInfo> GeneralInfos { get; set; }
        // public DbSet<JointAccountHolder> JointAccountHolders { get; set; }
        // public DbSet<Accreditation> Accreditations { get; set; }
        // public DbSet<InvestmentProfile> InvestmentProfiles { get; set; }
        // public DbSet<Beneficiary> Beneficiaries { get; set; }
        // public DbSet<FinancialTeamMember> FinancialTeamMembers { get; set; }
        // public DbSet<BankingInfo> BankingInfos { get; set; }
        // public DbSet<Document> Documents { get; set; }
        // public DbSet<Offering> Offerings { get; set; }
        // public DbSet<InvestorInvestment> InvestorInvestments { get; set; }

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