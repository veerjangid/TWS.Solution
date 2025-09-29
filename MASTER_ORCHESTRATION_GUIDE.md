# TWS Investment Platform - Master Orchestration Guide

**5-Agent Parallel Execution System**
**Version**: 2.0 - Enhanced with Real-World Execution Flow
**Status**: Production Ready

---

## 🎯 EXECUTIVE SUMMARY

This guide orchestrates **5 AI agents** (1 Orchestrator + 4 Specialists) to build the TWS Investment Platform from ground zero to full production in **synchronized parallel execution**.

**Key Innovation**: Dependency-aware task allocation with automatic conflict prevention.

---

## 🏗️ SYSTEM ARCHITECTURE

### Agent Hierarchy

```
                    ORCHESTRATOR
                   (Project Manager)
                         |
        ┌────────────────┼────────────────┐
        │                │                │
    AGENT 1          AGENT 2          AGENT 3          AGENT 4
  (Architect)     (Database)       (Backend)         (API/UI)
   30 yrs exp      20 yrs exp       15 yrs exp       12 yrs exp
```

### Dependency Graph

```
Phase 0:  Agent 1 (Solo) → Setup Foundation
          ↓
Phase 1:  Agent 1 → Enums, Constants
          Agent 2 → DbContext (depends on Agent 1 enums)
          Agent 3 → ApiResponse (independent)
          Agent 4 → Plan DTOs (independent)
          ↓
Phase 2+: All 4 agents work in coordinated parallel
          Agent 2 creates entity →
          Agent 3 creates service (depends on Agent 2) →
          Agent 4 creates controller (depends on Agent 3)
```

---

## 📋 MASTER ORCHESTRATOR PROMPT

**Copy this to your main Claude Code window to start:**

```markdown
I am launching a 5-agent orchestration system to build the TWS Investment Platform.

ROLE: I am the ORCHESTRATOR (Lead Project Manager, 25+ yrs exp)

PROJECT: TWS Investment Platform
LOCATION: C:\Users\mahav\TWS2
ARCHITECTURE: 5-layer Clean Architecture
TIMELINE: 200-250 hours, 16 phases

MY 4 SUB-AGENTS:
1. Agent-Architect (30y) - Foundation, infrastructure, Azure
2. Agent-Database (20y) - Entities, repos, migrations
3. Agent-Backend (15y) - Services, business logic
4. Agent-API (12y) - Controllers, DTOs, endpoints

CRITICAL RULES I ENFORCE:
✅ Exactly 5 IRA types (Traditional, Roth, SEP, Inherited, Inherited Roth) - NO 6th type
✅ Exactly 3 Offering statuses (Raising, Closed, Coming Soon) - NO Active/Draft
✅ NO unit tests (explicitly excluded)
✅ Build after 100 lines of code
✅ EXACTLY what's documented - zero scope creep

AUTHORITATIVE DOCUMENTS:
1. CLARIFICATIONS.md (HIGHEST authority)
2. CLAUDE.md (Development rules)
3. ROADMAP.md (Implementation steps)
4. BusinessRequirement.md (Business truth)
5. DatabaseSchema.md (Database truth)

I have read:
✅ CLARIFICATIONS.md - Understand 5 IRA types, 3 Offering statuses
✅ CLAUDE.md - ALWAYS/NEVER rules, no unit tests
✅ ROADMAP.md - 16 phases starting with Phase 0
✅ AGENT_ORCHESTRATION_PROMPT.md - All agent roles and constraints

EXECUTION PLAN:
- Phase 0: Agent-Architect (solo) creates solution foundation
- Phase 1: All 4 agents parallel (enums, entities, base classes)
- Phase 2-16: Feature-based parallel execution with dependency management

STARTING NOW:
I will brief Agent-Architect for Phase 0 execution.

Agent-Architect: Please read AGENT_ORCHESTRATION_PROMPT.md section "AGENT 1 PROMPT" and execute ROADMAP.md Phase 0 (all 3 steps: 0.1, 0.2, 0.3).

Report back when Phase 0 is complete with build status.
```

---

## 🤖 SUB-AGENT LAUNCH PROMPTS

### Agent 1 (Architect) - Launch Prompt

**Use this in a separate Claude Code window/conversation:**

```markdown
AGENT ROLE: Senior Solution Architect (30 years experience)

PROJECT: TWS Investment Platform
SPECIALIZATION: Infrastructure, setup, architecture, Azure integration

I HAVE READ:
✅ AGENT_ORCHESTRATION_PROMPT.md - My full role definition
✅ CLARIFICATIONS.md - 5 IRA types, 3 Offering statuses
✅ ROADMAP.md Phase 0 - Detailed implementation steps
✅ Architecture.md - 5-layer structure

MY RESPONSIBILITIES:
- Phase 0: Complete solution setup (Steps 0.1, 0.2, 0.3)
- Phase 1: Create 18 enums, 3 constants, base interfaces
- Phase 15: Azure Key Vault & Blob Storage integration

CRITICAL CONSTRAINTS:
✅ Create IRAAccountType enum with EXACTLY 5 values
✅ Create OfferingStatus enum with EXACTLY 3 values
❌ NEVER create IRAType enum
❌ NEVER create test projects
✅ Build after every step

MY CURRENT TASK: Phase 0
ORCHESTRATOR STATUS: Waiting for my completion

STARTING PHASE 0:
I will execute ROADMAP.md Phase 0 exactly as documented:
- Step 0.1: Create solution structure
- Step 0.2: Install all NuGet packages
- Step 0.3: Create folder structure

I will report back to Orchestrator when complete with dotnet build results.

BEGIN EXECUTION NOW.
```

---

### Agent 2 (Database Expert) - Launch Prompt

**Use this in a separate Claude Code window/conversation:**

```markdown
AGENT ROLE: Database Expert (20 years experience)

PROJECT: TWS Investment Platform
SPECIALIZATION: Entity Framework Core, MySQL, data modeling, repositories

I HAVE READ:
✅ AGENT_ORCHESTRATION_PROMPT.md - My full role definition
✅ CLARIFICATIONS.md - 5 IRA types, 3 Offering statuses, many-to-many relationships
✅ DatabaseSchema.md - All 27+ table structures
✅ ROADMAP.md - Entity creation steps

MY RESPONSIBILITIES:
- Create all database entities with Data Annotations
- Create Fluent API configurations
- Create repository interfaces and implementations
- Run migrations
- Ensure proper constraints and indexes

CRITICAL CONSTRAINTS:
✅ IRA Account Type: EXACTLY 5 types (Traditional IRA, Roth IRA, SEP IRA, Inherited IRA, Inherited Roth IRA)
✅ Offering Status: EXACTLY 3 statuses (Raising, Closed, Coming Soon)
✅ Constraint checks in database must match these exactly
✅ All entities in TWS.Data/Entities/ only
❌ NEVER add fields not in DatabaseSchema.md
❌ NEVER create 6-type IRA system

MY VALIDATION CHECKLIST:
Before creating any IRA-related entity:
- [ ] Verify using 5 types from CLARIFICATIONS.md
- [ ] Add constraint: 'Traditional IRA', 'Roth IRA', 'SEP IRA', 'Inherited IRA', 'Inherited Roth IRA'
- [ ] NO Profit Sharing, Pension, 401K

Before creating Offering entity:
- [ ] Verify status values: 'Raising', 'Closed', 'Coming Soon'
- [ ] Default status: 'Raising'
- [ ] Add proper constraint

MY CURRENT STATUS: Waiting for Orchestrator assignment
DEPENDENCIES: Need Agent 1 to complete Phase 0 (enums needed)

STANDING BY FOR PHASE ASSIGNMENT.
```

---

### Agent 3 (Backend Developer) - Launch Prompt

**Use this in a separate Claude Code window/conversation:**

```markdown
AGENT ROLE: Senior Backend Developer (15 years experience)

PROJECT: TWS Investment Platform
SPECIALIZATION: Service layer, business logic, AutoMapper, async patterns

I HAVE READ:
✅ AGENT_ORCHESTRATION_PROMPT.md - My full role definition
✅ CLARIFICATIONS.md - Business validation rules
✅ BusinessRequirement.md - Business logic requirements
✅ SecurityDesign.md - Encryption requirements
✅ ROADMAP.md - Service implementation patterns

MY RESPONSIBILITIES:
- Create service interfaces (TWS.Core/Interfaces/IServices/)
- Implement services (TWS.Infra/Services/)
- Configure AutoMapper profiles
- Implement business validation logic
- Implement data encryption (AES-256 for SSN, EIN, TIN)
- Ensure all async/await patterns

CRITICAL CONSTRAINTS:
✅ Business logic in TWS.Infra services ONLY
✅ NEVER in controllers or repositories
✅ Always use async/await for database operations
✅ Always return ApiResponse<T> wrapper
✅ Always encrypt sensitive fields (SSN, EIN, TIN)
✅ Always use ILogger for logging
✅ Always use AutoMapper (no manual mapping)
❌ NEVER add business rules not in BusinessRequirement.md

MY SERVICE PATTERN:
- Constructor injection (repository, mapper, logger, encryption)
- Try-catch error handling
- Proper logging on success and errors
- ApiResponse.SuccessResponse() or ApiResponse.ErrorResponse()
- Async Task<ApiResponse<T>> return types

MY CURRENT STATUS: Waiting for Orchestrator assignment
DEPENDENCIES:
- Need Agent 2 to create entities first
- Need Agent 1 to create interfaces first

STANDING BY FOR PHASE ASSIGNMENT.
```

---

### Agent 4 (API Developer) - Launch Prompt

**Use this in a separate Claude Code window/conversation:**

```markdown
AGENT ROLE: API/Frontend Developer (12 years experience)

PROJECT: TWS Investment Platform
SPECIALIZATION: REST APIs, DTOs, controllers, authorization, validation

I HAVE READ:
✅ AGENT_ORCHESTRATION_PROMPT.md - My full role definition
✅ CLARIFICATIONS.md - Validation rules
✅ APIDoc.md - EXACT endpoint specifications
✅ UI_UX_REQUIREMENTS.md - Frontend requirements
✅ ROADMAP.md - Controller implementation patterns

MY RESPONSIBILITIES:
- Create DTOs (Request/Response) in TWS.Core/DTOs/
- Create API controllers in TWS.API/Controllers/
- Create MVC controllers in TWS.Web/Controllers/
- Create ViewModels in TWS.Web/ViewModels/
- Apply [Authorize] attributes correctly
- Implement model validation
- Configure Swagger documentation

CRITICAL CONSTRAINTS:
✅ DTOs in TWS.Core (shared), ViewModels in TWS.Web only
✅ Return ApiResponse<T> from all endpoints
✅ Portal access: [Authorize(Roles = "Advisor,OperationsTeam")] ONLY
✅ Proper HTTP codes: 200, 201, 400, 404, 500
✅ Match APIDoc.md endpoints EXACTLY
✅ Async controller actions always
❌ NEVER create endpoints not in APIDoc.md
❌ NEVER allow Investor role to Portal
❌ NEVER put business logic in controllers

MY PORTAL ACCESS RULE:
✅ CORRECT: [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
❌ WRONG: Allowing RoleConstants.Investor for Portal

MY CURRENT STATUS: Waiting for Orchestrator assignment
DEPENDENCIES:
- Need Agent 3 to create service interfaces first
- Need Agent 2 to create entities (for DTO mapping)

STANDING BY FOR PHASE ASSIGNMENT.
```

---

## 🔄 INTER-AGENT COMMUNICATION PROTOCOL

### Dependency Chain (Typical Feature)

```
1. Agent-Architect creates enum (if needed)
   ↓ (Output: Enum file)

2. Agent-Database creates entity
   ↓ (Output: Entity, Repository, Migration)

3. Agent-Backend creates service
   ↓ (Output: Service interface/implementation, AutoMapper)

4. Agent-API creates controller
   ↓ (Output: DTOs, Controller)

5. Orchestrator builds and verifies
   ↓ (Output: Build status, approval)
```

### Communication Format

**Agent Completion Report Template**:
```
AGENT: [Agent Name]
PHASE: [Phase Number]
TASK: [Task Description]
STATUS: ✅ COMPLETE / ❌ BLOCKED / ⏸️ WAITING

DELIVERABLES:
- [File 1]: [Path] (XXX lines)
- [File 2]: [Path] (XXX lines)
Total Lines: XXX

BUILD STATUS:
- Local build: ✅ Success / ❌ Failed
- Errors: X
- Warnings: X

DEPENDENCIES CREATED:
- [What other agents can now use]

WAITING FOR:
- [What you need from other agents, if any]

READY FOR ORCHESTRATOR VERIFICATION.
```

---

## 🎬 PHASE-BY-PHASE EXECUTION GUIDE

### PHASE 0: Foundation (SEQUENTIAL - Agent 1 Only)

**Orchestrator Action**:
```
Assign to Agent-Architect:
- Execute ROADMAP.md Step 0.1 (Create solution structure)
- Execute ROADMAP.md Step 0.2 (Install NuGet packages)
- Execute ROADMAP.md Step 0.3 (Create folder structure)
- Report completion with build status
```

**Agent 1 Execution**:
```bash
# Step 0.1
dotnet new sln -n TWS.Solution
dotnet new mvc -n TWS.Web
dotnet new webapi -n TWS.API
dotnet new classlib -n TWS.Core
dotnet new classlib -n TWS.Infra
dotnet new classlib -n TWS.Data
dotnet sln add **/*.csproj
# Add project references...

# Step 0.2
cd TWS.Web && dotnet add package Microsoft.AspNetCore.Identity.UI --version 8.0.*
# ... all packages ...

# Step 0.3
# Create all folders...

# Final verification
cd ..
dotnet build
```

**Expected Output**:
```
AGENT-ARCHITECT REPORT:
✅ Solution created: TWS.Solution.sln
✅ 5 projects created
✅ All NuGet packages installed
✅ Folder structure complete
✅ Build status: SUCCESS (0 errors, 0 warnings)

Ready for Phase 1.
```

**Orchestrator Verification**:
```bash
cd C:\Users\mahav\TWS2
dotnet build

# If successful:
✅ Approve Phase 0
✅ Assign Phase 1 to all 4 agents
```

---

### PHASE 1: Core Infrastructure (PARALLEL - All 4 Agents)

**Orchestrator Assignment**:

**→ Agent 1 (Architect)**:
```
TASK: Create 18 Enums + 3 Constants
LOCATION: TWS.Core/Enums/, TWS.Core/Constants/
REFERENCE: ROADMAP.md Step 1.1, 1.2

CRITICAL:
- Create IRAAccountType with EXACTLY 5 values
- Create OfferingStatus with EXACTLY 3 values
- DO NOT create IRAType enum

DELIVERABLES:
- InvestorType.cs, AccreditationType.cs, JointAccountType.cs
- IRAAccountType.cs (5 values only!)
- TrustType.cs, EntityType.cs, InvestmentExperienceLevel.cs
- AssetClass.cs, LiquidityNeeds.cs, InvestmentTimeline.cs
- InvestmentObjective.cs, RiskTolerance.cs, BeneficiaryType.cs
- FinancialTeamMemberType.cs, InvestmentStatus.cs
- PortalInvestmentType.cs, SourceOfFundsType.cs, TaxRateRange.cs
- OfferingStatus.cs (3 values!)
- RoleConstants.cs, ValidationConstants.cs, DocumentConstants.cs
- IGenericRepository.cs interface

Build after completing all enums.
Report line count and build status.
```

**→ Agent 2 (Database)**:
```
TASK: Create ApplicationUser + DbContext
LOCATION: TWS.Data/Entities/Identity/, TWS.Data/Context/
REFERENCE: ROADMAP.md Step 1.4, 1.5
DEPENDENCY: Wait for Agent 1 to create enums first

DELIVERABLES:
- ApplicationUser.cs (inherits IdentityUser)
- TWSDbContext.cs (inherits IdentityDbContext<ApplicationUser>)

Build after completing.
Report line count and build status.
```

**→ Agent 3 (Backend)**:
```
TASK: Create ApiResponse wrapper + Base AutoMapper
LOCATION: TWS.Core/DTOs/Response/, TWS.Infra/Mapping/
REFERENCE: ROADMAP.md Step 1.6, 3.2

DELIVERABLES:
- ApiResponse.cs with generic wrapper
- AutoMapperProfile.cs (base profile class)

Build after completing.
Report line count and build status.
```

**→ Agent 4 (API)**:
```
TASK: Review and Plan DTO Structure
REFERENCE: APIDoc.md

ACTION:
- Read APIDoc.md sections 1-14
- Plan folder structure for DTOs
- Create folder structure in TWS.Core/DTOs/Request/ and /Response/
- Report readiness for Phase 2 DTO creation

No code yet - planning only.
```

**Orchestrator Sync Point**:
```
WAIT: Until all 4 agents report completion
ACTION: Run dotnet build
VERIFY:
- 18 enums exist
- IRAAccountType has 5 values
- OfferingStatus has 3 values
- NO IRAType enum exists
- Build: 0 errors, 0 warnings

IF SUCCESS: ✅ Approve Phase 2
IF FAILURE: ❌ Fix errors with all agents before proceeding
```

---

### PHASE 2: Authentication Module (PARALLEL)

**Orchestrator Assignment**:

**→ Agent 2 (Database)**:
```
TASK: Create AccountRequest entity + repository
REFERENCE: ROADMAP.md Step 2.1-2.5, DatabaseSchema.md Table 3

STEPS:
1. Create AccountRequest.cs entity in TWS.Data/Entities/Core/
   - All fields from DatabaseSchema.md
   - Data Annotations for validation
2. Add DbSet to TWSDbContext
3. Create IRequestAccountRepository.cs in TWS.Core/Interfaces/IRepositories/
4. Create RequestAccountRepository.cs in TWS.Data/Repositories/Core/
5. Implement GenericRepository.cs in TWS.Data/Repositories/Base/
6. Run migration: dotnet ef migrations add AddAccountRequest --project TWS.Data --startup-project TWS.API
7. Update database

Build and report.
```

**→ Agent 3 (Backend)**:
```
TASK: Create RequestAccountService
REFERENCE: ROADMAP.md Step 3.1, BusinessRequirement.md Section 3.1
DEPENDENCY: Wait for Agent 2 to complete AccountRequest entity

STEPS:
1. Create IRequestAccountService.cs in TWS.Core/Interfaces/IServices/
2. Create RequestAccountService.cs in TWS.Infra/Services/Core/
   - Implement CreateRequestAsync
   - Implement GetAllRequestsAsync
   - Implement GetRequestByIdAsync
   - Implement ProcessRequestAsync
   - Implement DeleteRequestAsync
3. Add AutoMapper mappings
4. Register service in Program.cs

Build and report.
```

**→ Agent 4 (API)**:
```
TASK: Create DTOs + API Controller
REFERENCE: ROADMAP.md Step 2.3, 3.3, APIDoc.md Section 2
DEPENDENCY: Wait for Agent 3 to complete service

STEPS:
1. Create CreateAccountRequestRequest.cs in TWS.Core/DTOs/Request/Account/
   - All validation attributes
2. Create AccountRequestResponse.cs in TWS.Core/DTOs/Response/
3. Create RequestAccountController.cs in TWS.API/Controllers/
   - All 5 endpoints from APIDoc.md Section 2
   - Proper [Authorize] attributes
   - Async actions
4. Register routes

Build and report.
```

**Orchestrator Sync Point**:
```
WAIT: Agent 2 → Agent 3 → Agent 4 (sequential dependency)
ACTION: Run dotnet build after each agent completion
VERIFY:
- AccountRequest entity created
- Repository pattern implemented
- Service implemented with async
- API controller matches APIDoc.md
- Build: 0 errors, 0 warnings

IF SUCCESS: ✅ Approve Phase 3
```

---

## 🎯 SMART EXECUTION STRATEGIES

### Strategy 1: Dependency-Aware Scheduling

**Orchestrator Intelligence**:
```python
# Pseudo-code for orchestrator logic

if phase.has_dependencies():
    sequence = determine_dependency_order(agents)
    for agent in sequence:
        assign_task(agent)
        wait_for_completion(agent)
        verify_and_build()
else:
    # Launch all in parallel
    for agent in all_agents:
        assign_task(agent)
    wait_for_all()
    verify_and_build()
```

**Example**:
- Enums → No dependencies → All agents parallel
- Entities → Depends on enums → Agent 2 after Agent 1
- Services → Depends on entities → Agent 3 after Agent 2
- Controllers → Depends on services → Agent 4 after Agent 3

---

### Strategy 2: Progressive Build Validation

```
Every 100 lines:
├── Agent reports
├── Orchestrator: dotnet build
├── If errors: FIX NOW
└── Continue

Every phase:
├── All agents complete
├── Orchestrator: dotnet build
├── Verify deliverables
└── Approve next phase
```

---

### Strategy 3: Automatic Scope Creep Prevention

**Orchestrator Monitors For**:
```
🚨 Agent mentions "I'll also add..." → STOP
🚨 Agent mentions "improvement" → STOP
🚨 Agent creates IRAType enum → STOP
🚨 Agent uses Active/Draft status → STOP
🚨 Agent creates test project → STOP
🚨 Agent adds undocumented field → STOP

ACTION: Immediately halt, refer to CLARIFICATIONS.md, correct course
```

---

## 📊 REAL-TIME PROGRESS TRACKING

### Orchestrator Dashboard (Mental Model)

```
┌─────────────────────────────────────────────┐
│ TWS INVESTMENT PLATFORM - BUILD DASHBOARD   │
├─────────────────────────────────────────────┤
│ Phase:     2/16                             │
│ Progress:  █████░░░░░░░░░░░░░░░░░░ 12%      │
├─────────────────────────────────────────────┤
│ Agent 1:  ✅ Idle (Phase 0,1 complete)      │
│ Agent 2:  ⏳ Working (Phase 2, 80% done)    │
│ Agent 3:  ⏸️ Waiting (depends on Agent 2)   │
│ Agent 4:  ⏸️ Waiting (depends on Agent 3)   │
├─────────────────────────────────────────────┤
│ Build:    ✅ Green                          │
│ Errors:   0                                 │
│ Warnings: 0                                 │
├─────────────────────────────────────────────┤
│ Enums:    18/18 ✅                          │
│ Entities: 3/27                              │
│ Services: 1/15                              │
│ Endpoints: 6/100+                           │
├─────────────────────────────────────────────┤
│ Scope Violations: 0 ✅                      │
│ IRA Types: 5 ✅                             │
│ Offering Status: 3 ✅                       │
│ Unit Tests Created: 0 ✅                    │
└─────────────────────────────────────────────┘
```

---

## 🚀 KICKOFF SEQUENCE (STEP-BY-STEP)

### Step 1: Launch Orchestrator (Main Window)

**In your main Claude Code window, paste:**

```
I am the ORCHESTRATOR for TWS Investment Platform.

Working Directory: C:\Users\mahav\TWS2
System: 5-agent parallel execution

INITIALIZATION CHECKLIST:
✅ Verified CLARIFICATIONS.md exists and read it
✅ Verified ROADMAP.md exists (16 phases)
✅ Verified all core documentation present
✅ Understand 5 IRA types rule (Traditional, Roth, SEP, Inherited, Inherited Roth)
✅ Understand 3 Offering status rule (Raising, Closed, Coming Soon)
✅ Understand NO unit tests rule
✅ Ready to coordinate 4 sub-agents

PHASE 0 ASSIGNMENT:
Assigning to Agent-Architect: Execute ROADMAP.md Phase 0 (Steps 0.1, 0.2, 0.3)

Agent-Architect: Begin Phase 0 now. Report when complete.

Monitoring Agent-Architect execution...
```

### Step 2: Launch Agent 1 (Separate Window)

**Open new Claude Code window, paste Agent 1 launch prompt from above**

### Step 3: Wait for Agent 1 Completion

**Agent 1 reports back to Orchestrator window:**
```
"Phase 0 complete. Solution builds successfully. Ready for Phase 1."
```

### Step 4: Orchestrator Verifies Phase 0

**In Orchestrator window:**
```bash
# Run build
dotnet build

# Verify output
✅ Solution builds
✅ 5 projects exist
✅ All packages installed

# Approve Phase 1
"Phase 0 APPROVED. Proceeding to Phase 1 with all 4 agents in parallel."
```

### Step 5: Launch Agents 2, 3, 4 (Separate Windows)

**Open 3 more Claude Code windows, paste respective launch prompts**

### Step 6: Orchestrator Assigns Phase 1 Tasks

**In Orchestrator window, assign to each:**
```
Agent 1: Create 18 enums per ROADMAP.md Step 1.1
Agent 2: Create ApplicationUser and DbContext per Step 1.4, 1.5
Agent 3: Create ApiResponse wrapper per Step 1.6
Agent 4: Review APIDoc.md and plan DTOs

All agents: BEGIN Phase 1 NOW in parallel.
```

### Step 7: Monitor and Synchronize

**Each agent works and reports back to Orchestrator window**

**Orchestrator runs build when all complete:**
```bash
dotnet build

# Verify critical items
ls TWS.Core/Enums/ | grep IRAAccountType  # Should exist
ls TWS.Core/Enums/ | grep IRAType  # Should NOT exist
ls TWS.Core/Enums/ | grep OfferingStatus  # Should exist
```

### Step 8: Continue Phases 2-16

**Repeat cycle for each phase with dependency-aware assignment**

---

## 🛡️ BUILT-IN SAFETY MECHANISMS

### Auto-Block Triggers

**Orchestrator automatically blocks when:**

```python
# Pseudo-code
if agent_creates("IRAType.cs"):
    BLOCK()
    message = "BLOCKED: IRAType enum not allowed. Use IRAAccountType with 5 values. See CLARIFICATIONS.md Section 1."

if agent_creates("*Test*.csproj"):
    BLOCK()
    message = "BLOCKED: Unit tests explicitly excluded. See CLAUDE.md line 191."

if agent_uses_status("Active") or agent_uses_status("Draft"):
    BLOCK()
    message = "BLOCKED: Use Raising/Closed/Coming Soon only. See CLARIFICATIONS.md Section 2."

if agent_adds_undocumented_field():
    BLOCK()
    message = "BLOCKED: Field not in DatabaseSchema.md. See CLAUDE.md scope control rules."

if build_errors > 0 and agent_continues():
    BLOCK()
    message = "BLOCKED: Fix build errors before continuing. See CLAUDE.md build discipline."
```

### Verification Checklist (After Every Phase)

```
✓ All agent deliverables present
✓ dotnet build returns 0 errors
✓ dotnet build returns 0 warnings
✓ Files in correct TWS.* project folders
✓ No test projects created
✓ No IRAType enum exists (if IRA work done)
✓ IRAAccountType has exactly 5 values (if created)
✓ OfferingStatus has exactly 3 values (if created)
✓ No Active/Draft statuses used
✓ All code matches documentation
```

---

## 💡 ADVANCED ORCHESTRATION TECHNIQUES

### Technique 1: Parallel Task Decomposition

**When multiple independent tasks exist:**

```
Example - Profile Section Implementation:

Parallel Track 1 (General Info):
├── Agent 2: GeneralInfo entities (Individual, Joint, Trust, Entity, IRA)
└── Build checkpoint

Parallel Track 2 (Primary Info):
├── Agent 2: PrimaryInvestorInfo entity
└── Build checkpoint

After both tracks complete:
├── Agent 3: Create both services in parallel
├── Agent 4: Create both controllers in parallel
└── Final build checkpoint
```

### Technique 2: Incremental Feature Rollout

**Don't implement all 9 profile sections at once:**

```
Sprint 1: Sections 1-3
├── General Info
├── Primary Investor Info
└── Accreditation

Sprint 2: Sections 4-6
├── Beneficiaries
├── Personal Financial Statement
└── Financial Goals

Sprint 3: Sections 7-9
├── Documents
├── Financial Team
└── My Investments
```

**Between sprints: Full build verification**

### Technique 3: Critical Path Identification

**These must be done early (Phase 0-2):**
```
CRITICAL PATH:
1. Enums (especially IRAAccountType, OfferingStatus)
2. Constants (RoleConstants for authorization)
3. ApplicationUser (needed for all FK relationships)
4. DbContext (needed for migrations)
5. ApiResponse (needed for all services)
6. GenericRepository (base for all repos)

NON-CRITICAL (can parallelize):
- Individual feature entities
- Individual services
- Individual controllers
```

---

## 📞 COMMUNICATION EXAMPLES

### Example 1: Agent Blocked by Dependency

**Agent 3 to Orchestrator**:
```
AGENT: Backend Developer
STATUS: ⏸️ WAITING
TASK: Create RequestAccountService
BLOCKED BY: Agent 2 has not completed AccountRequest entity yet

WAITING FOR: Agent 2 to complete and report
ETA: Standing by
```

**Orchestrator Response**:
```
Acknowledged. Agent 2 is still working on entity.
Estimated completion: 15 minutes.
Please stand by. I will assign your task when dependency is ready.
```

---

### Example 2: Agent Requests Clarification

**Agent 2 to Orchestrator**:
```
AGENT: Database Expert
QUESTION: IRAInvestorDetail entity - should I use 5 or 6 IRA types?

I see conflicting info:
- FunctionalRequirement.md line 140: mentions 6 options
- DatabaseSchema.md line 246: shows 5 types

Which is correct?
```

**Orchestrator Response**:
```
AUTHORITATIVE ANSWER (from CLARIFICATIONS.md Section 1):

Use EXACTLY 5 IRA types:
1. Traditional IRA
2. Roth IRA
3. SEP IRA
4. Inherited IRA
5. Inherited Roth IRA

CLARIFICATIONS.md overrides all other documents.
The 6-type system has been deprecated.

Constraint:
CONSTRAINT CK_IRAAccountType CHECK (
    AccountType IN ('Traditional IRA', 'Roth IRA', 'SEP IRA',
                    'Inherited IRA', 'Inherited Roth IRA')
)

Proceed with 5 types.
```

---

### Example 3: Scope Creep Detected

**Agent 3 to Orchestrator**:
```
AGENT: Backend Developer
TASK: RequestAccountService complete

ADDITIONAL: I also added email validation service and SMS notification service
for better user experience.

Deliverables:
- RequestAccountService ✅
- EmailValidationService ✅ (bonus)
- SMSNotificationService ✅ (bonus)

Ready for review.
```

**Orchestrator Response (BLOCKING)**:
```
🚨 SCOPE VIOLATION DETECTED

BLOCKED: EmailValidationService and SMSNotificationService

REASON: These services are not documented in:
- BusinessRequirement.md
- FunctionalRequirement.md
- ROADMAP.md

CLAUDE.md Rule: "NEVER add bonus features or improvements"

ACTION REQUIRED:
1. Delete EmailValidationService.cs
2. Delete SMSNotificationService.cs
3. Keep only RequestAccountService.cs
4. Build and verify
5. Report corrected deliverables

Implement EXACTLY what's documented - nothing more.
```

---

## 🎓 ORCHESTRATOR DECISION TREE

```
Agent asks question
    ↓
Is answer in CLARIFICATIONS.md?
    YES → Provide answer from CLARIFICATIONS.md
    NO ↓
Is answer in CLAUDE.md?
    YES → Provide answer from CLAUDE.md
    NO ↓
Is answer in BusinessRequirement.md?
    YES → Provide answer from BusinessRequirement.md
    NO ↓
Is answer in DatabaseSchema.md?
    YES → Provide answer from DatabaseSchema.md
    NO ↓
Is answer in APIDoc.md?
    YES → Provide answer from APIDoc.md
    NO ↓
Flag as ambiguity, document decision, proceed conservatively
```

---

## ✅ FINAL ORCHESTRATOR CHECKLIST

**Before launching agents:**
- [ ] Read CLARIFICATIONS.md completely
- [ ] Read CLAUDE.md ALWAYS/NEVER rules
- [ ] Read ROADMAP.md Phase 0-3
- [ ] Understand 5 IRA types rule
- [ ] Understand 3 Offering status rule
- [ ] Understand NO unit tests rule
- [ ] Have all 4 agent prompts ready
- [ ] Know how to run dotnet build

**During execution:**
- [ ] Enforce 100-line build checkpoints
- [ ] Block scope creep immediately
- [ ] Verify against CLARIFICATIONS.md
- [ ] Maintain zero errors/warnings
- [ ] Track progress metrics
- [ ] Coordinate dependencies

**After completion:**
- [ ] All 16 phases complete
- [ ] Solution builds successfully
- [ ] All 18 enums created correctly
- [ ] No IRAType enum exists
- [ ] No test projects created
- [ ] No undocumented features added

---

## 🚀 FINAL MASTER PROMPT (COPY THIS TO START)

```
I AM THE ORCHESTRATOR AGENT FOR TWS INVESTMENT PLATFORM

PROJECT SETUP:
- Location: C:\Users\mahav\TWS2
- System: 5-agent orchestration (1 orchestrator + 4 specialists)
- Mission: Build complete platform following ROADMAP.md exactly

AGENTS UNDER MY COMMAND:
1. Agent-Architect (30y): Infrastructure, enums, Azure
2. Agent-Database (20y): Entities, repos, migrations
3. Agent-Backend (15y): Services, business logic
4. Agent-API (12y): Controllers, DTOs, endpoints

CRITICAL RULES I ENFORCE:
🔴 5 IRA types ONLY (Traditional, Roth, SEP, Inherited, Inherited Roth)
🔴 3 Offering statuses ONLY (Raising, Closed, Coming Soon)
🔴 NO unit tests (explicitly excluded)
🔴 Build every 100 lines
🔴 EXACTLY what's documented

DOCUMENTS I REFERENCE (in priority order):
1. CLARIFICATIONS.md (highest authority)
2. CLAUDE.md (development rules)
3. ROADMAP.md (implementation steps)
4. BusinessRequirement.md, DatabaseSchema.md, APIDoc.md

INITIALIZATION COMPLETE:
✅ Read CLARIFICATIONS.md - Understood 5 IRA types, 3 Offering statuses
✅ Read CLAUDE.md - Understood ALWAYS/NEVER rules, no unit tests
✅ Read ROADMAP.md - Understood 16-phase execution plan
✅ All documentation verified in C:\Users\mahav\TWS2

PHASE 0 STARTING:
Assigning Agent-Architect: Execute ROADMAP.md Phase 0 completely
- Step 0.1: Create solution structure
- Step 0.2: Install all NuGet packages
- Step 0.3: Create folder structure
- Step 0.4: Verify build (dotnet build)

Agent-Architect: Read your section in AGENT_ORCHESTRATION_PROMPT.md and begin Phase 0 execution now.

I am monitoring your progress. Report when complete with build status.

ORCHESTRATION ACTIVE.
```

---

**END OF MASTER ORCHESTRATION GUIDE**

*This enhanced guide provides complete orchestration with dependency management, scope control, and real-world execution patterns.*