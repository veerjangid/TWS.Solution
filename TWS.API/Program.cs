using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Core.Settings;
using TWS.Data.Context;
using TWS.Data.Entities.Identity;
using TWS.Data.Repositories.Accreditation;
using TWS.Data.Repositories.Beneficiaries;
using TWS.Data.Repositories.Core;
using TWS.Data.Repositories.Documents;
using TWS.Data.Repositories.Financial;
using TWS.Data.Repositories.PrimaryInvestorInfo;
using TWS.Data.Repositories.TypeSpecific;
using TWS.Data.Repositories.Portal;
using TWS.Infra.Mapping;
using TWS.Infra.Services.Core;
using TWS.Infra.Services.Financial;
using TWS.Infra.Services.Security;
using TWS.Infra.Services.Investment;
using TWS.Infra.Services.Storage;
using TWS.API.Middleware;
using TWS.API.Filters;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/tws-api-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TWS Investment Platform API",
        Version = "v1",
        Description = "RESTful API for TWS Investment Platform - Investor Onboarding & CRM System",
        Contact = new OpenApiContact
        {
            Name = "TWS Development Team",
            Email = "dev@tangiblewealthsolutions.com"
        }
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token. Example: 'Bearer eyJhbGc...'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments for better documentation (optional - requires XML generation in csproj)
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // options.IncludeXmlComments(xmlPath);
});

// Add Controllers with filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});

// Add DbContext with MySQL provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TWSDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AzureSettings>(builder.Configuration.GetSection("AzureSettings"));
builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("EncryptionSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Configure ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings - Client Requirements: 8-24 chars, uppercase, number, special character
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true; // Changed: Special character now required
    options.Password.RequiredLength = 8; // Changed: Minimum 8 characters
    options.Password.RequiredUniqueChars = 1;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Sign-in settings
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<TWSDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? string.Empty)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add CORS
var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("TWSCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TWSDbContext>(name: "database", tags: new[] { "db", "sql" });

// Add Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Register Repositories
builder.Services.AddScoped<IRequestAccountRepository, RequestAccountRepository>();
builder.Services.AddScoped<IInvestorProfileRepository, InvestorProfileRepository>();
builder.Services.AddScoped<IndividualInvestorDetailRepository>();
builder.Services.AddScoped<JointInvestorDetailRepository>();
builder.Services.AddScoped<IRAInvestorDetailRepository>();
builder.Services.AddScoped<TrustInvestorDetailRepository>();
builder.Services.AddScoped<EntityInvestorDetailRepository>();
builder.Services.AddScoped<IPrimaryInvestorInfoRepository, PrimaryInvestorInfoRepository>();
builder.Services.AddScoped<IBrokerAffiliationRepository, BrokerAffiliationRepository>();
builder.Services.AddScoped<IInvestmentExperienceRepository, InvestmentExperienceRepository>();
builder.Services.AddScoped<ISourceOfFundsRepository, SourceOfFundsRepository>();
builder.Services.AddScoped<ITaxRateRepository, TaxRateRepository>();
builder.Services.AddScoped<IInvestorAccreditationRepository, InvestorAccreditationRepository>();
builder.Services.AddScoped<IAccreditationDocumentRepository, AccreditationDocumentRepository>();
builder.Services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
builder.Services.AddScoped<IPersonalFinancialStatementRepository, PersonalFinancialStatementRepository>();
builder.Services.AddScoped<IFinancialGoalsRepository, FinancialGoalsRepository>();
builder.Services.AddScoped<IFinancialTeamMemberRepository, FinancialTeamMemberRepository>();
builder.Services.AddScoped<IInvestorDocumentRepository, InvestorDocumentRepository>();
builder.Services.AddScoped<IOfferingRepository, OfferingRepository>();
builder.Services.AddScoped<IInvestorInvestmentRepository, InvestorInvestmentRepository>();
builder.Services.AddScoped<IInvestmentTrackerRepository, InvestmentTrackerRepository>();

// Register Azure Services
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();
builder.Services.AddSingleton<IKeyVaultService, KeyVaultService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();

// Register Services
builder.Services.AddScoped<IRequestAccountService, RequestAccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInvestorService, InvestorService>();
builder.Services.AddScoped<IGeneralInfoService, GeneralInfoService>();
builder.Services.AddScoped<IPrimaryInvestorInfoService, PrimaryInvestorInfoService>();
builder.Services.AddScoped<IAccreditationService, AccreditationService>();
builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
builder.Services.AddScoped<IPersonalFinancialStatementService, PersonalFinancialStatementService>();
builder.Services.AddScoped<IFinancialGoalsService, FinancialGoalsService>();
builder.Services.AddScoped<IFinancialTeamService, FinancialTeamService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IOfferingService, OfferingService>();
builder.Services.AddScoped<IInvestmentService, InvestmentService>();
builder.Services.AddScoped<IPortalService, TWS.Infra.Services.Portal.PortalService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add Serilog request logging
app.UseSerilogRequestLogging();

// Configure exception handling middleware (must be first)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TWS Investment Platform API v1");
        options.RoutePrefix = "swagger"; // Access Swagger at /swagger
        options.DocumentTitle = "TWS Investment Platform API Documentation";
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.ShowExtensions();
        options.EnableValidator();
    });
}

app.UseHttpsRedirection();

// Add Response Compression
app.UseResponseCompression();

// Add CORS
app.UseCors("TWSCorsPolicy");

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Add Health Check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

// Map Controllers
app.MapControllers();

try
{
    Log.Information("Starting TWS Investment Platform API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
