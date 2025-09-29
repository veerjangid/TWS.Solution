TWS Investment Platform - Functional Requirements Specification (FRS)
=====================================================================

Document Information
--------------------

*   **Version**: 1.0
*   **Date**: Current
*   **Status**: Draft
*   **Purpose**: Define functional requirements for TWS Investment Platform

* * *

1. System Overview
------------------

### 1.1 Purpose

The TWS Investment Platform enables investor account creation, profile management, accreditation verification, and investment tracking for real estate investments.

### 1.2 Scope

*   Five investor types (Individual, Joint, Trust, Entity, IRA)
*   Nine profile sections per investor type
*   Portal CRM for advisors
*   Investment offerings gallery

### 1.3 Users

*   Investors
*   Advisors
*   Operations Team

* * *

2. Functional Requirements by Module
------------------------------------

Module 1: Account Request
-------------------------

### FR1.1 Request Account Form

**Description**: Homepage button to request account
**Functional Requirements**:
*   FR1.1.1: System shall display "Request Account" button on homepage
*   FR1.1.2: System shall open request form when button clicked
*   FR1.1.3: System shall collect contact information (fields TBD by business)
*   FR1.1.4: System shall save request to AccountRequests table
*   FR1.1.5: System shall display success message after submission
**User Story**: As a potential investor, I want to request an account so that an advisor can contact me
**Acceptance Criteria**:
*   Request saved in database
*   Success message displayed
*   Request visible to advisors

* * *

Module 2: Authentication
------------------------

### FR2.1 User Login

**Functional Requirements**:
*   FR2.1.1: System shall authenticate using email and password
*   FR2.1.2: System shall support three roles: Investor, Advisor, OperationsTeam
*   FR2.1.3: System shall redirect to appropriate dashboard based on role

### FR2.2 User Registration

**Functional Requirements**:
*   FR2.2.1: System shall allow advisors to create investor accounts
*   FR2.2.2: System shall send credentials to investor email
*   FR2.2.3: System shall enforce password requirements

### FR2.3 Password Management

**Functional Requirements**:
*   FR2.3.1: System shall support forgot password functionality
*   FR2.3.2: System shall support password reset via email link

* * *

Module 3: Investor Type Selection
---------------------------------

### FR3.1 Type Selection

**Functional Requirements**:
*   FR3.1.1: System shall display five investor type options:
    *   Individual
    *   Joint
    *   Trust
    *   Entity
    *   IRA
*   FR3.1.2: System shall save selected type to Investors table
*   FR3.1.3: System shall direct to appropriate initial information collection

### FR3.2 Initial Information Collection

#### FR3.2.1 Individual Investor

**Fields Required**:
*   First Name (text)
*   Last Name (text)
*   US Citizen or Resident Alien? (Yes/No)
*   Accredited Investor? (Yes/No)
*   If Yes to accredited, select type (1-6)

#### FR3.2.2 Joint Investor

**Fields Required**:
*   Same as Individual PLUS:
*   Are you investing jointly? (Yes/No)
*   If Yes, Joint Account Type (6 options):
    *   Joint Tenants with Right of Survivorship
    *   Joint Tenants in Common
    *   Tenants by the Entirety
    *   A Married Person, as my Sole and Separate Property
    *   Husband and Wife, as Community Property with Rights of Survivorship
    *   Husband and Wife, as Community Property

#### FR3.2.3 Trust Investor

**Fields Required**:
*   Trust Name (text)
*   US Trust? (Yes/No)
*   If Yes, select accreditation type (1-6)

#### FR3.2.4 Entity Investor

**Fields Required**:
*   Company Name (text)
*   US Company? (Yes/No)
*   If Yes, select accreditation type (1-6)

#### FR3.2.5 IRA Investor

**Fields Required**:
*   IRA Type selection (5 standardized options):
    *   Traditional IRA
    *   Roth IRA
    *   SEP IRA
    *   Inherited IRA
    *   Inherited Roth IRA
*   Name of IRA (text)
*   First Name (text)
*   Last Name (text)
*   US Citizen or Resident Alien? (Yes/No)
*   Accredited Investor? (Yes/No)
*   If Yes, select accreditation type (1-6)

**CLARIFICATION**: Both initial selection and General Info use the same 5 IRA types

* * *

Module 4: Investor Profile - Nine Sections
------------------------------------------

### FR4.1 Section 1: General Info

#### FR4.1.1 Individual General Info

**Fields**:
*   Name
*   Date of Birth
*   SSN
*   Address
*   Phone
*   Email
*   Driver's License (upload)
*   W9 (upload)

#### FR4.1.2 Joint General Info

**Fields**:
*   All Individual fields for EACH account holder
*   "Add Account Holder" button functionality
*   System shall allow unlimited account holders

#### FR4.1.3 Trust General Info

**Fields**:
*   Name
*   Date of Formation
*   TIN/EIN
*   Address
*   Phone
*   Email
*   Driver's License of each equity owner (multiple uploads)
*   W9 of Trust (upload)
*   Trust Certificate (upload - required)
*   Trust Agreement (upload - optional)
*   Type of Trust dropdown (Revocable/Irrevocable)
*   Purpose of Formation (text)

#### FR4.1.4 Entity General Info

**Fields**:
*   Name
*   Date of Formation
*   TIN/EIN
*   Address
*   Phone
*   Email
*   Driver's License of each equity owner (multiple uploads)
*   W9 of Entity (upload)
*   Type of Entity dropdown (LLC/Corporation/Partnership)
*   Purpose of Formation (text)
**Conditional Documents**:
*   If Corporation:
    *   Certificate of Incorporation (upload)
    *   Corporate Resolution (upload)
*   If LLC:
    *   Articles of Incorporation (upload)
    *   LLC Operating Agreement (upload)
    *   Certificate Of Beneficial Owners (upload)
    *   LLC Certificate (upload - only if no Operating Agreement)

#### FR4.1.5 IRA General Info

**Fields**:
*   Name
*   DOB
*   SSN
*   Address
*   Phone
*   Email
*   Driver's License (upload)
*   W9 (upload)
*   Custodian (text)
*   Account Type dropdown (5 types):
    *   Traditional IRA
    *   Roth IRA
    *   SEP IRA
    *   Inherited IRA
    *   Inherited Roth IRA
*   IRA Account Number
*   Rolling over to CNB Custodian? (Yes/No)
**If Rolling Over = Yes**:
*   Upload current IRA Statement
*   Custodian Phone Number
*   Custodian Fax Number
*   Have you liquidated assets? (Yes/No)
*   If No: Display "Please liquidate your accounts to set your account up for transfer to new custodian"

### FR4.2 Section 2: Primary Investor Information

**Same for all investor types**
**Personal Information Fields**:
*   First Name
*   Last Name
*   Legal Street Address
*   City
*   State
*   Zip
*   Email
*   Cell Phone Number
*   Married (Yes/No)
*   Social Security Number
*   Date of Birth
*   Driver's License Number
*   Driver's License Expiration Date
*   Occupation
*   Employer Name
*   If Retired, former profession
*   Alternate Address (Yes/No)
**Broker Dealer Affiliation**:
*   Employee of broker-dealer? (Yes/No)
    *   If yes, broker-dealer name
*   Related to employee of broker-dealer? (Yes/No)
    *   If yes, broker-dealer name
    *   Employee name
    *   Relationship
*   Senior officer/director/10%+ shareholder? (Yes/No)
*   Manager/member/executive officer of private placements? (Yes/No)
**Investment Experience Matrix**: For each asset class, select: None, 1-5 Years, Over 5 Years
*   Individual Stocks
*   Fixed Income
*   Mutual Funds/ETFs
*   Direct Real Estate Holdings
*   Real Estate Investments (REITs, RE Funds, DSTs/1031s)
*   LPs and/or LLCs
*   Private Equity/Debt
*   Oil and Gas
*   Tax Mitigation Investments
*   Other Alternative Investments (specify)
**Income Information**:
*   Lowest income previous two years
*   Anticipated income this year
*   Relying on joint income for accreditation? (Yes/No)
**Source of Funds** (Multi-Select):
*   Income From earnings
*   Inheritance
*   Insurance Proceeds
*   1031 Exchange/Sale of Property
*   Investment Proceeds
*   Pension/IRA Savings
*   Sale of Business
**Tax Rate** (Multi-Select):
*   0-15%
*   16-25%
*   26-30%
*   31-35%
*   Over 36%

### FR4.3 Section 3: Accreditation

**Conditional Document Requirements**:
**If Type 1 Selected** ($1 Million Net Worth):
*   Real Estate: Mortgage Statements AND Schedule E
*   Bank Statements
*   IRA/401K Statement
*   Stock Statement
*   QI Statement Of Funds
**If Type 2 Selected** (Income):
*   Tax Returns (2 years + extension if needed)
*   W2s (2 years)
**If Type 3, 4, or 5 Selected** (Professional):
*   Professional License upload
*   License Number
*   State License Held

### FR4.4 Section 4: Beneficiaries

**Process**:
1.  Collect number of Primary Beneficiaries
2.  Collect number of Contingent Beneficiaries
3.  For each beneficiary, collect:
    *   First/Middle/Last Name
    *   Social Security Number
    *   DOB
    *   Phone
    *   Relationship to owner
    *   Address
    *   City
    *   State
    *   ZIP
    *   Percentage of Benefit
**Validation**: Total percentage must equal 100% for each type

### FR4.5 Section 5: Personal Financial Statement

**Functional Requirements**:
*   FR4.5.1: System shall display fillable PDF form
*   FR4.5.2: System shall allow export as PDF
*   FR4.5.3: File name: TWS PFS Fillable.pdf

### FR4.6 Section 6: Financial Goals

**Checkbox Fields**:
*   Liquidity needs: Low/Medium/High (single select)
*   Investment timeline: 0-5 Years/6-11 Years/12+ Years (single select)
*   Investment objective: Income/Growth/Income & Growth (single select)
*   Risk Tolerance: Low Risk/Medium Risk/High Risk (single select)
**Goals** (Multi-Select):
*   Defer taxes
*   Protect principal
*   Grow principal
*   Consistent cash flow
*   Diversification
*   Retirement
*   Estate/legacy planning
**Additional**: "Tell us more" text field

### FR4.7 Section 7: Documents

**Functional Requirements**:
*   FR4.7.1: System shall provide drag-drop upload area
*   FR4.7.2: System shall collect document name for each upload
*   FR4.7.3: System shall store in /uploads/investors/{investorId}/

### FR4.8 Section 8: Financial Team

**For Each Team Member Type** (CPA, Financial Advisor, Estate Attorney):
*   Name (text)
*   Email (text)
*   Phone Number (text)

### FR4.9 Section 9: My Investments

**Functional Requirements**:
*   FR4.9.1: System shall display gallery of investments
*   FR4.9.2: Gallery corresponds to investor profile (implementation TBD)

* * *

Module 5: Portal (CRM)
----------------------

### FR5.1 Access Control

**Functional Requirements**:
*   FR5.1.1: System shall restrict access to Advisors and Operations Team only
*   FR5.1.2: System shall deny access to Investor role

### FR5.2 Dashboard Display

**Columns**:
1.  Offerings
2.  Client Name
3.  Status (dropdown with 13 options)
4.  Advisor
5.  Investment Type (dropdown with 5 options)

### FR5.3 Status Options

System shall support 13 statuses:
1.  Need DST to come out
2.  Onboarding
3.  Investment Paperwork
4.  BD Approval
5.  Docs done, need pprty to close
6.  At custodian for signature
7.  In Backup Status at Sponsor
8.  Sponsor
9.  QI
10.  Wire Requested
11.  Funded
12.  Closed WON
13.  Full Cycle Investment

### FR5.4 Investment Types

System shall support 5 types:
1.  Private Placement 506(c)
2.  1031 Exchange Investment
3.  Universal Offering
4.  Tax Strategy
5.  Roth IRA Conversion

### FR5.5 Investment Tracker Popup

**When offering name clicked, display popup with fields**:
*   Offering
*   Status
*   Lead Owner - Licensed Rep
*   Relationship
*   Investment Type
*   Client Name
*   Investment Held in the Names of
*   Date Investment Closed
*   $ Original Equity Investment Amount
*   $ Total (TWS AUM)
*   $ Rep Commission Amount
*   $ TWS Revenue
*   $ DST Revenue
*   $ Alt Revenue
*   $ Tax Strategy Revenue
*   $ Oil and Gas Revenue
*   $ Initial v Recurring Revenue
*   Marketing Source
*   Referred By
*   Notes
*   $ ALT AUM
*   $ DST AUM
*   $ O&G AUM
*   $ Tax Strategy AUM

* * *

3. Process Flows
----------------

### 3.1 Account Creation Flow

    1. User clicks "Request Account" on homepage
    2. User submits request form
    3. Advisor reviews request
    4. Advisor creates investor account
    5. System sends credentials to investor
    6. Investor logs in
    7. Investor selects type
    8. Investor completes 9 profile sections
    

### 3.2 Investor Type Flow

    1. Select investor type (1 of 5)
    2. Complete initial information
    3. System creates investor record
    4. System directs to profile sections
    

### 3.3 Profile Completion Flow

    For each of 9 sections:
    1. User navigates to section
    2. System displays appropriate form based on investor type
    3. User enters information
    4. System validates required fields
    5. System saves data
    6. System updates completion status
    

* * *

4. Validation Rules
-------------------

### 4.1 Required Field Validation

*   All fields marked as required must be completed before saving

### 4.2 Format Validation

*   Email: Valid email format
*   SSN: XXX-XX-XXXX format
*   Phone: Valid phone format
*   Percentages: Numeric 0-100

### 4.3 Business Logic Validation

*   Beneficiary percentages must total 100%
*   Accreditation type must match investor type group

* * *

5. Error Handling
-----------------

### 5.1 Form Submission Errors

*   Display field-level validation messages
*   Prevent form submission until corrected

### 5.2 File Upload Errors

*   Display error for unsupported file types
*   Display error for files exceeding size limit

* * *

6. Data Persistence
-------------------

### 6.1 Save Functionality

*   Each section saves independently
*   Auto-save not required
*   Manual save button per section

### 6.2 Data Retrieval

*   Load existing data when returning to section
*   Display completion status for each section

* * *

**END OF FUNCTIONAL REQUIREMENTS SPECIFICATION**
_This document contains only the functional requirements as specified in the business requirements. No additional features have been included._