# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

TWS Investment Platform - A comprehensive investor onboarding and CRM system for Tangible Wealth Solutions, a real estate investment firm.

**Stack**: ASP.NET Core 8.0 MVC + Web API, Entity Framework Core with MySQL (Pomelo), ASP.NET Identity, Azure services

## Critical Clarifications (MUST READ)

**These clarifications override any contradictory information in documents:**

1. **IRA Types Standardization** (CRITICAL):
   - Only **5 IRA types** exist: Traditional IRA, Roth IRA, SEP IRA, Inherited IRA, Inherited Roth IRA
   - Use the SAME 5 types for both: (a) Initial investor type selection, (b) General Info section
   - DO NOT implement: Profit Sharing, Pension, or 401K types
   - Single enum: `IRAAccountType` (removed old `IRAType` enum)

2. **Offering Status Values**:
   - Only 3 statuses: **Raising**, **Closed**, **Coming Soon**
   - Default status: "Raising"
   - DO NOT use: Active, Draft, or any other status values

3. **Investment-Profile Relationship**:
   - Many-to-many relationship via `InvestorInvestments` junction table
   - One investor → multiple investments
   - One offering → multiple investors

4. **Request Account Form**:
   - Fields are complete as specified in BusinessRequirement.md
   - No additional fields needed beyond documented ones

## Architecture

Clean Architecture with 5-layer structure:
- **TWS.Web**: MVC application (investor-facing UI)
- **TWS.API**: RESTful Web API
- **TWS.Core**: Domain layer (interfaces, DTOs, enums, constants)
- **TWS.Infra**: Infrastructure layer (service implementations, AutoMapper profiles)
- **TWS.Data**: Data access layer (entities, DbContext, repositories, migrations)

## Essential Commands

### Build and Run
```bash
# Build entire solution
dotnet build

# Run Web project
dotnet run --project TWS.Web

# Run API project
dotnet run --project TWS.API

# Build specific project
dotnet build TWS.Data
```

### Database Migrations
```bash
# Add new migration (run from solution root)
dotnet ef migrations add MigrationName --project TWS.Data --startup-project TWS.API

# Update database
dotnet ef database update --project TWS.Data --startup-project TWS.API

# Remove last migration
dotnet ef migrations remove --project TWS.Data --startup-project TWS.API
```

### Package Management
```bash
# Add package to specific project
dotnet add TWS.Data package Pomelo.EntityFrameworkCore.MySql

# Restore all packages
dotnet restore
```

## Authoritative Documents (READ THESE FIRST)

1. **BusinessRequirement.md** - Ultimate business truth, defines ALL features
2. **FunctionalRequirement.md** - Exact implementation specifications
3. **DatabaseSchema.md** - ONLY source for table structures and relationships
4. **Architecture.md** - Folder structure and layer separation rules
5. **APIDoc.md** - API endpoint specifications with exact request/response payloads
6. **SecurityDesign.md** - Encryption and security requirements
7. **AzureImplmentation.md** - Azure service integration methods

## Critical Development Rules

### ALWAYS Rules (Mandatory)

#### Layer Separation & Architecture
- Always create database entities in TWS.Data layer exclusively
- Always use TWS.Data layer for database entities, DbContext, repositories, and migrations - never put database logic elsewhere
- Always keep business logic in TWS.Infra services layer, never in controllers or repositories
- Always create shared DTOs in TWS.Core layer that both API and Web projects can use - avoid duplication
- Always separate ViewModels (Web layer) from DTOs (Core layer) - don't mix presentation with data transfer
- Always use dependency injection for all services, repositories, and dependencies - no direct instantiation
- Always follow SOLID and DRY principles while developing

#### Code Quality & Reusability
- Always check for duplicate classes, methods, and properties before creating new ones - reuse existing code
- Always use meaningful variable and method names following C# naming conventions
- Always use Constants for String Literals, maintain Constant Classes for URLs in Web Project

#### Async Patterns
- Always make all database operations and API calls async with proper async/await patterns
- Repository methods must return Task<T> or Task<IEnumerable<T>>
- Service methods must implement async patterns throughout

#### Database & Entity Framework
- Always use Entity Framework Core Code-First approach with MySQL provider (Pomelo.EntityFrameworkCore.MySql)
- Always create proper indexes on foreign keys and frequently queried columns for performance
- Always create entities in TWS.Data/Entities with EF annotations
- Always use Fluent API configurations in TWS.Data/Configurations

#### Security
- Always encrypt sensitive data (SSN, EIN, TIN, bank details) at field level using AES-256 before storing
- Always implement proper authorization with role-based access (Investor, Advisor, OperationsTeam)
- Always restrict Portal access to only Advisor and OperationsTeam roles using [Authorize] attribute
- Never expose encrypted data in API responses without decryption

#### API Standards
- Always return standardized ApiResponse<T> wrapper with success, message, data, and statusCode fields
- Always implement proper HTTP status codes (200 OK, 201 Created, 400 Bad Request, 404 Not Found)
- Always use AutoMapper for entity to DTO conversions - configure mappings in profiles
- Always document APIs with Swagger/OpenAPI including request/response examples

#### Error Handling & Validation
- Always implement comprehensive error handling middleware to catch and log all exceptions
- Always validate models using DataAnnotations and return detailed validation errors
- Always use Serilog for structured logging to file system in Logs/ folder

#### Configuration
- Always use configuration files for environment-specific settings - never hardcode values

#### Build Discipline (CRITICAL)
- **ALWAYS build the solution after writing 100+ lines of code - stop and compile immediately**
- **ALWAYS fix all build errors in the same context before continuing with any new code**
- **ALWAYS resolve build warnings along with errors - maintain zero warning policy**
- **ALWAYS test that the solution builds after fixing errors before moving forward**
- **ALWAYS run dotnet build for the entire solution, not just individual projects**
- **ALWAYS fix reference issues, missing usings, and type mismatches immediately**
- **ALWAYS validate that all projects in the solution build together after changes**
- Run solution-wide build after any project reference or package changes
- **If build fails: Stop all new development until build is green**

#### View Organization
- All long views (pages exceeding ~200 lines of Razor markup or containing multiple UI sections) must be broken down into partial views
- Any view content that is reused across modules (headers, forms, tables, lists, widgets) must be implemented as partial views
- Keep view logic minimal - use ViewModels for data preparation

#### Document Adherence (STRICT)
- Always consult BusinessRequirement.md FIRST before implementing any feature - it is the ultimate business truth
- Always follow FunctionalRequirement.md exactly for feature implementation - no assumptions or additions
- Always use DatabaseSchema.md as the ONLY source for table structures, relationships, and indexes
- Always implement Architecture.md's folder structure and layer separation exactly as specified
- Always match APIDoc.md endpoints, request/response payloads, and status codes precisely
- Always implement SecurityDesign.md's encryption and security measures without deviation
- Always follow AzureImplmentation.md for all Azure service integrations and authentication methods

### NEVER Rules (Absolute Prohibitions)

#### Scope Control
- **NEVER add bonus features, extra functionality, or "nice-to-have" improvements beyond what's in the authoritative docs**
- **NEVER write additional code that isn't explicitly required in the documents**
- **NEVER suggest or implement features not found in BusinessRequirement.md or FunctionalRequirement.md**
- **NEVER add extra database fields, tables, or relationships beyond DatabaseSchema.md**
- **NEVER create additional API endpoints not listed in APIDoc.md**
- Never implement features not explicitly defined in the authoritative documents
- If a feature seems missing but isn't documented - DON'T implement it
- If the documents have gaps or ambiguities - flag them, don't fill them with assumptions
- If something seems like it would be helpful but isn't specified - DON'T add it
- Every line of code must trace back to a specific requirement in the documents
- When in doubt, implement less rather than more - stick to documented scope only

#### Schema & API Control
- Never modify database schema without updating DatabaseSchema.md first
- Never create API endpoints that don't exist in APIDoc.md specification

#### Build & Quality
- **NEVER proceed to next feature if current code has compilation errors**
- **NEVER commit code that doesn't build successfully**
- **NEVER accumulate multiple features worth of code without building**

#### Testing
- Never write unit test cases for this project (explicitly excluded)

#### Configuration & Security
- Never hardcode configuration values - use appsettings.json
- Never skip hooks (--no-verify, --no-gpg-sign)
- Never use git commands with -i flag (not supported in non-interactive mode)

### Implementation Principle
**ALWAYS implement EXACTLY what the documents specify - nothing more, nothing less**

## Key Technologies

- **ORM**: Entity Framework Core 8.0 with Pomelo.EntityFrameworkCore.MySql
- **Authentication**: ASP.NET Identity with JWT tokens (API) and Cookie auth (Web)
- **Logging**: Serilog to file system (Logs/ folder)
- **Mapping**: AutoMapper for entity-DTO conversions
- **Validation**: DataAnnotations with model validation filters
- **Storage**: Azure Blob Storage for documents
- **Email**: SendGrid or SMTP integration
- **PDF**: iTextSharp for report generation

## Project Status

Currently in **planning phase** - no code implementation yet. All specification documents are complete and ready for development.