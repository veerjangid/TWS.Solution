# TWS Investment Platform - Business Requirements Document (Updated)

**Version**: 2.1 (Final with Clarifications)
**Date**: Current
**Status**: Final - Ready for Development
**Purpose**: Complete Business Requirements with all clarifications applied

---

## Document Control

### Changes in Version 2.1:
- **IRA Account Types**: Standardized to 5 types across all sections
- **Request Account Form**: Complete field specifications defined
- **Offering Structure**: Simplified to "Offering Name" only
- **Investment-Profile Relationship**: Confirmed many-to-many structure
- **All ambiguities resolved**: No pending clarifications

### Critical Clarifications (Final):
1. **IRA Types Standardization**:
   - Initial Selection: 5 types (Traditional IRA, Roth IRA, SEP IRA, Inherited IRA, Inherited Roth IRA)
   - General Info Section: Same 5 types
   - **Changed**: Removed 6-type system (no more Profit Sharing, Pension, 401K options)

2. **Offering Status Values**:
   - Raising (currently raising capital)
   - Closed (offering closed)
   - Coming Soon (future offering)
   - **Changed**: Replaced Active/Draft/Closed with new 3-status system

3. **Investment-Profile Relationship**:
   - Confirmed many-to-many via InvestorInvestments junction table
   - One investor can have multiple investments
   - One offering can have multiple investors

4. **Request Account Form**:
   - All fields specified and validated
   - No additional fields needed

---

## 1. Executive Summary

TWS Investment Platform is a comprehensive **investor onboarding and CRM system** for Tangible Wealth Solutions, a real estate investment firm working with broker-dealer Emerson Equity LLC.

### 1.1 Company Information
- **Company**: Tangible Wealth Solutions (TWS)
- **Type**: Real estate investment firm, wealth management firm focused solely on real estate
- **Broker-Dealer**: Emerson Equity LLC

### 1.2 Investment Strategies
TWS pursues three investment strategies:
1. **Private Placements**: Real Estate through Private Placements or Funds
2. **1031 Exchanges**: DSTs and Oil & Gas Royalties
3. **Direct Investment**: Buying Investment Properties directly

---

## 2. Core Business Entities

### 2.1 User Types
1. **Investors** - Individual clients investing in real estate
2. **Advisors** - TWS representatives managing client relationships
3. **Operations Team** - Internal team managing platform operations

### 2.2 Module Permissions
- **Investor Profiles**: Accessible to all user types
- **Investments/Offerings**: Accessible to all user types
- **Portal (CRM)**: Accessible to **Advisors and Operations Team ONLY**

### 2.3 Investor Types (5 Types - Finalized)
1. **Individual** - Single person investor
2. **Joint** - Multiple account holders (spouses, partners)
3. **Trust** - Revocable/Irrevocable trusts
4. **Entity** - LLCs, Corporations, Partnerships
5. **IRA** - Retirement accounts with **5 standardized subtypes**:
   - **Traditional IRA**
   - **Roth IRA**
   - **SEP IRA**
   - **Inherited IRA**
   - **Inherited Roth IRA**

### 2.4 Investment Types (5 Types)
1. **Private Placement 506(c)**
2. **1031 Exchange Investment**
3. **Universal Offering**
4. **Tax Strategy**
5. **Roth IRA Conversion**

---

## 3. Account Creation Process

### 3.1 Request Account Form (CLARIFIED)
**Form Fields** (Complete specification):

#### Required Fields:
- **Full Name** (Text, Max 200 characters)
- **Email** (Email validation required)
- **Phone Number** (Phone format validation)

#### Optional Fields:
- **Message/Comments** (Textarea, Max 2000 characters)
- **Preferred Contact Method** (Dropdown: Email/Phone/Both)
- **Investment Interest** (Textarea, Max 500 characters)

**Form Behavior**:
- Submit button with client-side validation
- Success message: *"Thank you for your interest. An advisor will contact you soon."*
- Store in `AccountRequests` table with status `'Pending'`
- Email notification to Operations Team

---

## 4. Investor Types and Initial Collection

### 4.1 Individual Investor
**Initial Fields**:
- First Name (Required)
- Last Name (Required)
- Are you a US Citizen or Resident Alien? (Yes/No)
- Are you an accredited investor? (Yes/No)
- If Yes, select accreditation type (1-6)

### 4.2 Joint Investor
**Initial Fields**:
- Same as Individual PLUS:
- Are you investing jointly or as 'a married person, as my sole and separate property'? (Yes/No)
- If Yes, select **Joint Account Type** (6 options):
  - Joint Tenants with Right of Survivorship
  - Joint Tenants in Common
  - Tenants by the Entirety
  - A Married Person, as my Sole and Separate Property
  - Husband and Wife, as Community Property with Rights of Survivorship
  - Husband and Wife, as Community Property

### 4.3 Trust Investor
**Initial Fields**:
- Trust Name (Required)
- US Trust? (Yes/No)
- If Yes, select accreditation type (1-6)

### 4.4 Entity Investor
**Initial Fields**:
- Company Name (Required)
- US Company? (Yes/No)
- If Yes, select accreditation type (1-6)

### 4.5 IRA Investor (STANDARDIZED)
**Initial Fields**:
- **IRA Type** (5 standardized options):
  - Traditional IRA
  - Roth IRA
  - SEP IRA
  - Inherited IRA
  - Inherited Roth IRA
- Name of IRA (Text field)
- First Name (Required)
- Last Name (Required)
- Are you a US Citizen or Resident Alien? (Yes/No)
- Are you an accredited investor? (Yes/No)
- If Yes, select accreditation type (1-6)

---

## 5. Accreditation Types

### 5.1 For Individual, Joint, and IRA (Types 1-6)
1. **Net Worth**: Individual net worth, or joint net worth with spouse, exceeding $1 million (excluding primary residence)
2. **Income**: Individual income over $200,000 or joint income over $300,000 for last two years
3. **License - Series 7**: General securities representative license in good standing
4. **License - Series 65**: Investment adviser representative license
5. **License - Series 82**: Private securities offerings representative license
6. **Professional Role**: SEC-registered broker-dealer, investment adviser, or exempt reporting adviser

### 5.2 For Trust and Entity (Types 1-6)
1. **All Equity Owners Accredited**: All equity owners/grantors/settlors are accredited investors
2. **Registered Institution**: Bank, insurance company, pension fund with assets exceeding $5 million
3. **Corporate Entity**: Corporation, partnership, charitable organization with $5+ million assets
4. **Employee Benefit Plan**: ERISA plan with bank/insurance oversight or $5+ million assets
5. **Sophisticated Trust**: Trust with $5+ million assets, not formed to acquire securities
6. **Family Office**: Family office with $5+ million AUM and sophisticated investment direction

---

## 6. Investor Profile - Nine Sections

All investor types complete the following nine sections:

### 6.1 Section 1: General Info

#### 6.1.1 Individual
- Name, Date of Birth, SSN (encrypted)
- Address, Phone, Email
- Driver's License (upload), W9 (upload)

#### 6.1.2 Joint
- Same as Individual for EACH account holder
- **"Add Account Holder"** functionality for unlimited holders
- Driver's License and W9 for each holder

#### 6.1.3 Trust
- Name, Date of Formation, TIN/EIN (encrypted)
- Address, Phone, Email
- Driver's License of each equity owner, W9 of Trust
- **Trust Certificate** (required upload)
- **Trust Agreement** (optional upload)
- **Trust Type** dropdown: Revocable Trust/Irrevocable Trust
- Purpose of Formation (text)

#### 6.1.4 Entity
- Name, Date of Formation, TIN/EIN (encrypted)
- Address, Phone, Email
- Driver's License of each equity owner, W9 of Entity
- **Entity Type** dropdown: LLC/Corporation/Partnership
- Purpose of Formation (text)
- **Conditional Documents**:
  - **If Corporation**: Certificate of Incorporation, Corporate Resolution
  - **If LLC**: Articles of Incorporation, LLC Operating Agreement, Certificate of Beneficial Owners, LLC Certificate (if no Operating Agreement)

#### 6.1.5 IRA (STANDARDIZED)
- Name, DOB, SSN (encrypted), Address, Phone, Email
- Driver's License (upload), W9 (upload)
- Custodian name
- **Account Type** dropdown (5 standardized types):
  - Traditional IRA
  - Roth IRA
  - SEP IRA
  - Inherited IRA
  - Inherited Roth IRA
- IRA Account Number
- **Rolling over to CNB Custodian?** (Yes/No)
- **If Rolling Over = Yes**:
  - Upload current IRA Statement
  - Custodian Phone Number, Custodian Fax Number
  - **Have you liquidated assets?** (Yes/No)
  - **If No**: Display message *"Please liquidate your accounts to set your account up for transfer to new custodian"*

### 6.2 Section 2: Primary Investor Information
**Universal for all investor types** - 16 personal fields including:
- Personal details, broker-dealer affiliation
- **Investment Experience Matrix** (10 asset classes × 3 experience levels)
- Income information, source of funds (7 multi-select options)
- Tax rate (5 multi-select ranges)

### 6.3 Section 3: Accreditation
**Conditional document requirements based on type selected**:
- **Type 1**: Real Estate documents, Bank Statements, IRA/401K, Stock, QI statements
- **Type 2**: Tax Returns (2 years), W2s (2 years)
- **Types 3-6**: Professional License, License Number, State

### 6.4 Section 4: Beneficiaries
- **Primary and Contingent beneficiaries**
- 9 required fields per beneficiary
- **Percentage validation**: Each type must total 100%

### 6.5 Section 5: Personal Financial Statement
- Fillable PDF form (`TWS PFS Fillable.pdf`)
- Export to PDF functionality

### 6.6 Section 6: Financial Goals
- Liquidity needs, investment timeline, objectives, risk tolerance
- 7 investment goals (multi-select)
- Free text field

### 6.7 Section 7: Documents
- Drag-and-drop upload for miscellaneous documents
- Document naming capability

### 6.8 Section 8: Financial Team
- Contact info for: CPA, Financial Advisor, Estate Attorney

### 6.9 Section 9: My Investments
- Investment gallery corresponding to investor profile
- **Many-to-many relationship**: Investors ↔ Offerings

---

## 7. Portal (CRM Module)

### 7.1 Dashboard Display (5 Columns)
1. **Offering Name** (SIMPLIFIED - no complex structure)
2. **Client Name**
3. **Status**
4. **Advisor**
5. **Investment Type**

### 7.2 Investment Status Options (13 Statuses)
1. Need DST to come out
2. Onboarding
3. Investment Paperwork
4. BD Approval
5. Docs done, need pprty to close
6. At custodian for signature
7. In Backup Status at Sponsor
8. Sponsor
9. QI
10. Wire Requested
11. Funded
12. Closed WON
13. Full Cycle Investment

### 7.3 Investment Tracker Popup (21 Fields)
When clicking offering name, display popup with:
- **Basic Info**: Offering Name, Status, Lead Owner, Relationship, Investment Type, Client Name
- **Investment Details**: Names held under, Close Date, Original Equity Amount
- **Revenue Tracking**: TWS, DST, Alt, Tax Strategy, O&G revenues
- **AUM Tracking**: Total TWS AUM, ALT AUM, DST AUM, O&G AUM, Tax Strategy AUM
- **Marketing**: Source, Referred By, Rep Commission
- **Notes**: Free text field

---

## 8. Key Data Relationships (CONFIRMED)

### 8.1 Core Relationships
- **User → InvestorProfiles**: One-to-Many (users can have multiple profiles)
- **InvestorProfile → Investments**: Many-to-Many (confirmed)
- **InvestorProfile → Documents**: One-to-Many
- **InvestorProfile → Beneficiaries**: One-to-Many

### 8.2 Offering Structure (SIMPLIFIED)
- **Offerings Table**: Simple structure with `OfferingName` field
- **InvestorInvestments**: Junction table for many-to-many relationship
- **Investment Tracker**: Portal view with all 21 tracking fields

---

## 9. Technical Requirements

### 9.1 Technology Stack
- **Backend**: ASP.NET Core (latest version)
- **Database**: MySQL with Entity Framework Core (Code-First)
- **Architecture**: Clean Architecture (Web, API, Core, Infrastructure, Data layers)
- **Authentication**: ASP.NET Identity with JWT
- **Logging**: Serilog (file output)
- **Documentation**: Swagger for API
- **Caching**: Memory caching (Web application only)

### 9.2 Security Requirements
- **Role-based access control** (3 roles)
- **Data encryption** for SSN, EIN, TIN, financial data
- **Secure document storage** with access logging
- **JWT authentication** with refresh tokens
- **Input validation** and XSS protection

---

## 10. Validation Rules

### 10.1 IRA Account Types (STANDARDIZED)
```csharp
public static readonly string[] ValidIRATypes = {
    "Traditional IRA",
    "Roth IRA",
    "SEP IRA",
    "Inherited IRA",
    "Inherited Roth IRA"
};
```

### 10.2 Business Rules
- **Profile Completion**: Track across all 9 sections
- **Beneficiary Percentages**: Must total 100% for each type (Primary/Contingent)
- **Document Requirements**: Conditional based on investor type and accreditation
- **File Uploads**: Type and size validation, secure storage paths

---

**END OF REQUIREMENTS DOCUMENT**

*This document contains ALL finalized requirements with every clarification applied. No ambiguities remain. Ready for complete development implementation.*