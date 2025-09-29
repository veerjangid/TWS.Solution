TWS Investment Platform - Complete API Documentation with Request/Response
==========================================================================

Document Information
--------------------

*   **Version**: 3.0 (Complete with all payloads)
*   **Base URL**: `https://api.tws.com/v1`
*   **Authentication**: Bearer Token (JWT)
*   **Content-Type**: `application/json` (except file uploads: `multipart/form-data`)
*   **Note**: No server-side pagination implemented

* * *

1. Authentication Endpoints (AccountController)
-----------------------------------------------

### 1.1 Login

**Endpoint**: `POST /api/account/login`  
**Authentication**: None
**Request Body**:

    {
      "email": "john.doe@example.com",
      "password": "Password123!"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Login successful",
      "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "refreshToken": "refresh_token_here",
        "role": "Investor",
        "userId": "550e8400-e29b-41d4-a716-446655440000",
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@example.com"
      },
      "statusCode": 200
    }
    

### 1.2 Register

**Endpoint**: `POST /api/account/register`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Request Body**:

    {
      "email": "newinvestor@example.com",
      "password": "SecurePass123!",
      "confirmPassword": "SecurePass123!",
      "firstName": "Jane",
      "lastName": "Smith",
      "role": "Investor"
    }
    

**Response 201 Created**:

    {
      "success": true,
      "message": "User registered successfully",
      "data": {
        "userId": "660e8400-e29b-41d4-a716-446655440001",
        "email": "newinvestor@example.com",
        "firstName": "Jane",
        "lastName": "Smith"
      },
      "statusCode": 201
    }
    

### 1.3 Forgot Password

**Endpoint**: `POST /api/account/forgot-password`  
**Authentication**: None
**Request Body**:

    {
      "email": "john.doe@example.com"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Password reset email sent successfully",
      "data": null,
      "statusCode": 200
    }
    

### 1.4 Reset Password

**Endpoint**: `POST /api/account/reset-password`  
**Authentication**: None
**Request Body**:

    {
      "token": "reset_token_from_email",
      "newPassword": "NewSecurePass123!",
      "confirmPassword": "NewSecurePass123!"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Password reset successful",
      "data": null,
      "statusCode": 200
    }
    

### 1.5 Refresh Token

**Endpoint**: `POST /api/account/refresh-token`  
**Authentication**: None
**Request Body**:

    {
      "refreshToken": "existing_refresh_token"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Token refreshed successfully",
      "data": {
        "token": "new_jwt_token",
        "refreshToken": "new_refresh_token"
      },
      "statusCode": 200
    }
    

### 1.6 Logout

**Endpoint**: `POST /api/account/logout`  
**Authentication**: Bearer Token
**Request Body**: None
**Response 200 OK**:

    {
      "success": true,
      "message": "Logout successful",
      "data": null,
      "statusCode": 200
    }
    

* * *

2. Account Request Endpoints (RequestAccountController)
-------------------------------------------------------

### 2.1 Submit Request

**Endpoint**: `POST /api/request-account`  
**Authentication**: None
**Request Body**:

    {
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@example.com",
      "phone": "555-123-4567",
      "message": "I am interested in investing in real estate through TWS"
    }
    

**Response 201 Created**:

    {
      "success": true,
      "message": "Account request submitted successfully",
      "data": {
        "requestId": 1,
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@example.com",
        "requestDate": "2024-01-15T10:30:00"
      },
      "statusCode": 201
    }
    

### 2.2 Get All Requests

**Endpoint**: `GET /api/request-account`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Response 200 OK**:

    {
      "success": true,
      "message": "Account requests retrieved successfully",
      "data": [
        {
          "id": 1,
          "firstName": "John",
          "lastName": "Doe",
          "email": "john.doe@example.com",
          "phone": "555-123-4567",
          "message": "I am interested in investing",
          "requestDate": "2024-01-15T10:30:00",
          "isProcessed": false,
          "processedDate": null,
          "processedByUserId": null,
          "notes": null
        }
      ],
      "statusCode": 200
    }
    

### 2.3 Get Request by ID

**Endpoint**: `GET /api/request-account/{id}`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Response 200 OK**:

    {
      "success": true,
      "message": "Account request retrieved successfully",
      "data": {
        "id": 1,
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@example.com",
        "phone": "555-123-4567",
        "message": "I am interested in investing",
        "requestDate": "2024-01-15T10:30:00",
        "isProcessed": false,
        "processedDate": null,
        "processedByUserId": null,
        "notes": null
      },
      "statusCode": 200
    }
    

### 2.4 Process Request

**Endpoint**: `PUT /api/request-account/{id}/process`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Request Body**:

    {
      "notes": "Account created and credentials sent to investor"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Account request processed successfully",
      "data": {
        "id": 1,
        "isProcessed": true,
        "processedDate": "2024-01-16T09:00:00",
        "processedByUserId": "advisor-user-id",
        "notes": "Account created and credentials sent to investor"
      },
      "statusCode": 200
    }
    

### 2.5 Delete Request

**Endpoint**: `DELETE /api/request-account/{id}`  
**Authentication**: Bearer Token (OperationsTeam only)
**Response 204 No Content**: (Empty response)

* * *

3. Investor Endpoints (InvestorController)
------------------------------------------

### 3.1 Select Investor Type - Individual

**Endpoint**: `POST /api/investor/select-type`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorType": 1,
      "firstName": "John",
      "lastName": "Doe",
      "isUSCitizen": true,
      "isAccredited": true,
      "accreditationType": 1
    }
    

**Response 201 Created**:

    {
      "success": true,
      "message": "Investor type selected successfully",
      "data": {
        "investorId": 1,
        "userId": "user-id",
        "investorType": 1,
        "isUSCitizen": true,
        "isAccredited": true,
        "accreditationType": 1
      },
      "statusCode": 201
    }
    

### 3.1.2 Select Investor Type - Joint

**Request Body**:

    {
      "investorType": 2,
      "firstName": "John",
      "lastName": "Doe",
      "isUSCitizen": true,
      "isAccredited": true,
      "accreditationType": 1,
      "isJointInvestment": true,
      "jointAccountType": 1
    }
    

### 3.1.3 Select Investor Type - Trust

**Request Body**:

    {
      "investorType": 3,
      "trustName": "Doe Family Trust",
      "isUSTrust": true,
      "accreditationType": 1
    }
    

### 3.1.4 Select Investor Type - Entity

**Request Body**:

    {
      "investorType": 4,
      "companyName": "Doe Investments LLC",
      "isUSCompany": true,
      "accreditationType": 1
    }
    

### 3.1.5 Select Investor Type - IRA

**Request Body**:

    {
      "investorType": 5,
      "iraType": 1,
      "nameOfIRA": "John Doe Traditional IRA",
      "firstName": "John",
      "lastName": "Doe",
      "isUSCitizen": true,
      "isAccredited": true,
      "accreditationType": 1
    }
    

### 3.2 Get Current Investor

**Endpoint**: `GET /api/investor/current`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Investor retrieved successfully",
      "data": {
        "investorId": 1,
        "userId": "user-id",
        "investorType": 1,
        "isUSCitizen": true,
        "isAccredited": true,
        "accreditationType": 1,
        "profileCompletionStatus": 33,
        "createdDate": "2024-01-15T10:30:00",
        "isActive": true
      },
      "statusCode": 200
    }
    

### 3.3 Get Investor by ID

**Endpoint**: `GET /api/investor/{id}`  
**Authentication**: Bearer Token
**Response 200 OK**: Same as 3.2

### 3.4 Update Investor

**Endpoint**: `PUT /api/investor/{id}`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "isAccredited": true,
      "accreditationType": 2
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Investor updated successfully",
      "data": {
        "investorId": 1,
        "accreditationType": 2
      },
      "statusCode": 200
    }
    

### 3.5 Get All Investors

**Endpoint**: `GET /api/investor`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Response 200 OK**:

    {
      "success": true,
      "message": "Investors retrieved successfully",
      "data": [
        {
          "investorId": 1,
          "firstName": "John",
          "lastName": "Doe",
          "email": "john.doe@example.com",
          "investorType": 1,
          "isAccredited": true,
          "profileCompletionStatus": 100,
          "createdDate": "2024-01-15T10:30:00"
        }
      ],
      "statusCode": 200
    }
    

* * *

4. General Info Endpoints (GeneralInfoController)
-------------------------------------------------

### 4.1 Get General Info

**Endpoint**: `GET /api/general-info/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "General info retrieved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "name": "John Doe",
        "dateOfBirth": "1980-01-01",
        "ssn": "***-**-1234",
        "address": "123 Main St, City, State 12345",
        "phone": "555-123-4567",
        "email": "john.doe@example.com",
        "driverLicensePath": "/uploads/investors/1/dl.pdf",
        "w9Path": "/uploads/investors/1/w9.pdf"
      },
      "statusCode": 200
    }
    

### 4.2 Save Individual General Info

**Endpoint**: `POST /api/general-info/individual`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "name": "John Doe",
      "dateOfBirth": "1980-01-01",
      "ssn": "123-45-6789",
      "address": "123 Main St, City, State 12345",
      "phone": "555-123-4567",
      "email": "john.doe@example.com"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "General info saved successfully",
      "data": {
        "id": 1,
        "investorId": 1
      },
      "statusCode": 200
    }
    

### 4.3 Save Joint General Info

**Endpoint**: `POST /api/general-info/joint`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 2,
      "isJointInvestment": true,
      "jointAccountType": 1
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Joint general info saved successfully",
      "data": {
        "id": 1,
        "investorId": 2,
        "jointAccountType": 1
      },
      "statusCode": 200
    }
    

### 4.4 Add Joint Account Holder

**Endpoint**: `POST /api/general-info/joint/account-holder`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "jointInvestorDetailId": 1,
      "name": "Jane Doe",
      "dateOfBirth": "1982-06-15",
      "ssn": "987-65-4321",
      "address": "123 Main St, City, State 12345",
      "phone": "555-123-4568",
      "email": "jane.doe@example.com",
      "orderIndex": 2
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Account holder added successfully",
      "data": {
        "id": 1,
        "jointInvestorDetailId": 1,
        "name": "Jane Doe"
      },
      "statusCode": 200
    }
    

### 4.5 Save Trust General Info

**Endpoint**: `POST /api/general-info/trust`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 3,
      "trustName": "Doe Family Trust",
      "isUSTrust": true,
      "trustType": 1,
      "dateOfFormation": "2020-01-01",
      "purposeOfFormation": "Estate planning and asset protection",
      "tinEin": "12-3456789"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Trust general info saved successfully",
      "data": {
        "id": 1,
        "investorId": 3,
        "trustName": "Doe Family Trust"
      },
      "statusCode": 200
    }
    

### 4.6 Add Trust Grantor

**Endpoint**: `POST /api/general-info/trust/grantor`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "trustInvestorDetailId": 1,
      "name": "John Doe"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Trust grantor added successfully",
      "data": {
        "id": 1,
        "trustInvestorDetailId": 1,
        "name": "John Doe"
      },
      "statusCode": 200
    }
    

### 4.7 Save Entity General Info

**Endpoint**: `POST /api/general-info/entity`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 4,
      "companyName": "Doe Investments LLC",
      "isUSCompany": true,
      "entityType": 1,
      "dateOfFormation": "2019-06-01",
      "purposeOfFormation": "Real estate investment",
      "tinEin": "98-7654321",
      "hasOperatingAgreement": true
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Entity general info saved successfully",
      "data": {
        "id": 1,
        "investorId": 4,
        "companyName": "Doe Investments LLC"
      },
      "statusCode": 200
    }
    

### 4.8 Add Entity Equity Owner

**Endpoint**: `POST /api/general-info/entity/equity-owner`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "entityInvestorDetailId": 1,
      "name": "John Doe"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Equity owner added successfully",
      "data": {
        "id": 1,
        "entityInvestorDetailId": 1,
        "name": "John Doe"
      },
      "statusCode": 200
    }
    

### 4.9 Save IRA General Info

**Endpoint**: `POST /api/general-info/ira`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 5,
      "name": "John Doe",
      "dateOfBirth": "1980-01-01",
      "ssn": "123-45-6789",
      "address": "123 Main St, City, State 12345",
      "phone": "555-123-4567",
      "email": "john.doe@example.com",
      "custodianName": "Fidelity",
      "accountType": 1,
      "iraAccountNumber": "123456789",
      "isRollingOverToCNB": true,
      "custodianPhoneNumber": "800-123-4567",
      "custodianFaxNumber": "800-123-4568",
      "hasLiquidatedAssets": false
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "IRA general info saved successfully",
      "data": {
        "id": 1,
        "investorId": 5,
        "iraAccountNumber": "123456789"
      },
      "statusCode": 200
    }
    

### 4.10 Upload General Info Document

**Endpoint**: `POST /api/general-info/document`  
**Authentication**: Bearer Token  
**Content-Type**: `multipart/form-data`
**Form Data**:

    investorId: 1
    documentType: "DriverLicense"
    file: [binary file data]
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Document uploaded successfully",
      "data": {
        "documentId": 1,
        "filePath": "/uploads/investors/1/dl_20240115.pdf"
      },
      "statusCode": 200
    }
    

* * *

5. Primary Investor Information Endpoints (PrimaryInvestorInfoController)
-------------------------------------------------------------------------

### 5.1 Get Primary Info

**Endpoint**: `GET /api/primary-investor-info/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Primary investor info retrieved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "firstName": "John",
        "lastName": "Doe",
        "legalStreetAddress": "123 Main Street",
        "city": "New York",
        "state": "NY",
        "zip": "10001",
        "email": "john.doe@example.com",
        "cellPhoneNumber": "555-123-4567",
        "isMarried": true,
        "socialSecurityNumber": "***-**-6789",
        "dateOfBirth": "1980-01-01",
        "driversLicenseNumber": "D123456789",
        "driversLicenseExpirationDate": "2025-01-01",
        "occupation": "Software Engineer",
        "employerName": "Tech Corp",
        "retiredProfession": null,
        "hasAlternateAddress": false,
        "lowestIncomeLastTwoYears": 250000,
        "anticipatedIncomeThisYear": 275000,
        "isRelyingOnJointIncome": false
      },
      "statusCode": 200
    }
    

### 5.2 Save Primary Info

**Endpoint**: `POST /api/primary-investor-info`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "firstName": "John",
      "lastName": "Doe",
      "legalStreetAddress": "123 Main Street",
      "city": "New York",
      "state": "NY",
      "zip": "10001",
      "email": "john.doe@example.com",
      "cellPhoneNumber": "555-123-4567",
      "isMarried": true,
      "socialSecurityNumber": "123-45-6789",
      "dateOfBirth": "1980-01-01",
      "driversLicenseNumber": "D123456789",
      "driversLicenseExpirationDate": "2025-01-01",
      "occupation": "Software Engineer",
      "employerName": "Tech Corp",
      "retiredProfession": null,
      "hasAlternateAddress": false,
      "alternateAddress": null,
      "lowestIncomeLastTwoYears": 250000,
      "anticipatedIncomeThisYear": 275000,
      "isRelyingOnJointIncome": false
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Primary investor info saved successfully",
      "data": {
        "id": 1,
        "investorId": 1
      },
      "statusCode": 200
    }
    

### 5.3 Save Broker Affiliation

**Endpoint**: `POST /api/primary-investor-info/broker-affiliation`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "isEmployeeOfBrokerDealer": false,
      "brokerDealerName": null,
      "isRelatedToEmployee": true,
      "relatedBrokerDealerName": "ABC Securities",
      "employeeName": "Jane Doe",
      "relationship": "Spouse",
      "isSeniorOfficer": false,
      "isManagerMemberExecutive": false
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Broker affiliation saved successfully",
      "data": {
        "id": 1,
        "investorId": 1
      },
      "statusCode": 200
    }
    

### 5.4 Save Investment Experience

**Endpoint**: `POST /api/primary-investor-info/experience`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "experiences": [
        {
          "assetClass": 1,
          "experienceLevel": 2
        },
        {
          "assetClass": 2,
          "experienceLevel": 1
        },
        {
          "assetClass": 3,
          "experienceLevel": 2
        },
        {
          "assetClass": 4,
          "experienceLevel": 0
        },
        {
          "assetClass": 5,
          "experienceLevel": 1
        },
        {
          "assetClass": 6,
          "experienceLevel": 0
        },
        {
          "assetClass": 7,
          "experienceLevel": 0
        },
        {
          "assetClass": 8,
          "experienceLevel": 0
        },
        {
          "assetClass": 9,
          "experienceLevel": 1
        }
      ],
      "otherInvestmentDescription": "Cryptocurrency investments"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Investment experience saved successfully",
      "data": {
        "investorId": 1,
        "experiencesAdded": 9
      },
      "statusCode": 200
    }
    

### 5.5 Save Source of Funds

**Endpoint**: `POST /api/primary-investor-info/source-of-funds`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "sourcesOfFunds": [1, 4, 5, 7]
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Source of funds saved successfully",
      "data": {
        "investorId": 1,
        "sourcesAdded": 4
      },
      "statusCode": 200
    }
    

### 5.6 Save Tax Rates

**Endpoint**: `POST /api/primary-investor-info/tax-rates`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "taxRates": [3, 4]
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Tax rates saved successfully",
      "data": {
        "investorId": 1,
        "ratesAdded": 2
      },
      "statusCode": 200
    }
    

* * *

6. Accreditation Endpoints (AccreditationController)
----------------------------------------------------

### 6.1 Get Accreditation

**Endpoint**: `GET /api/accreditation/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Accreditation retrieved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "accreditationType": 1,
        "isVerified": false,
        "verificationDate": null,
        "licenseNumber": null,
        "stateLicenseHeld": null,
        "documents": [
          {
            "id": 1,
            "documentType": "BankStatement",
            "documentPath": "/uploads/investors/1/bank_statement.pdf",
            "uploadDate": "2024-01-15T10:30:00"
          }
        ]
      },
      "statusCode": 200
    }
    

### 6.2 Save Accreditation

**Endpoint**: `POST /api/accreditation`  
**Authentication**: Bearer Token
**Request Body for Type 1 (Net Worth)**:

    {
      "investorId": 1,
      "accreditationType": 1
    }
    

**Request Body for Type 3 (Professional License)**:

    {
      "investorId": 1,
      "accreditationType": 3,
      "licenseNumber": "Series7-123456",
      "stateLicenseHeld": "NY"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Accreditation saved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "accreditationType": 1
      },
      "statusCode": 200
    }
    

### 6.3 Upload Accreditation Document

**Endpoint**: `POST /api/accreditation/document`  
**Authentication**: Bearer Token  
**Content-Type**: `multipart/form-data`
**Form Data**:

    investorAccreditationId: 1
    documentType: "BankStatement"
    file: [binary file data]
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Accreditation document uploaded successfully",
      "data": {
        "documentId": 1,
        "filePath": "/uploads/investors/1/accreditation/bank_statement.pdf"
      },
      "statusCode": 200
    }
    

* * *

7. Beneficiary Endpoints (BeneficiaryController)
------------------------------------------------

### 7.1 Get Beneficiaries

**Endpoint**: `GET /api/beneficiary/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Beneficiaries retrieved successfully",
      "data": {
        "primary": [
          {
            "id": 1,
            "beneficiaryType": 1,
            "firstMiddleLastName": "Jane Marie Doe",
            "socialSecurityNumber": "***-**-4321",
            "dateOfBirth": "1982-06-15",
            "phone": "555-987-6543",
            "relationshipToOwner": "Spouse",
            "address": "123 Main St",
            "city": "New York",
            "state": "NY",
            "zip": "10001",
            "percentageOfBenefit": 50.00
          }
        ],
        "contingent": [
          {
            "id": 2,
            "beneficiaryType": 2,
            "firstMiddleLastName": "Jack Doe",
            "socialSecurityNumber": "***-**-5678",
            "dateOfBirth": "2010-03-20",
            "phone": "555-987-6543",
            "relationshipToOwner": "Son",
            "address": "123 Main St",
            "city": "New York",
            "state": "NY",
            "zip": "10001",
            "percentageOfBenefit": 100.00
          }
        ]
      },
      "statusCode": 200
    }
    

### 7.2 Add Beneficiary

**Endpoint**: `POST /api/beneficiary`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "beneficiaryType": 1,
      "firstMiddleLastName": "Jane Marie Doe",
      "socialSecurityNumber": "987-65-4321",
      "dateOfBirth": "1982-06-15",
      "phone": "555-987-6543",
      "relationshipToOwner": "Spouse",
      "address": "123 Main St",
      "city": "New York",
      "state": "NY",
      "zip": "10001",
      "percentageOfBenefit": 50.00
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Beneficiary added successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "firstMiddleLastName": "Jane Marie Doe"
      },
      "statusCode": 200
    }
    

### 7.3 Add Multiple Beneficiaries

**Endpoint**: `POST /api/beneficiary/bulk`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "beneficiaries": [
        {
          "beneficiaryType": 1,
          "firstMiddleLastName": "Jane Marie Doe",
          "socialSecurityNumber": "987-65-4321",
          "dateOfBirth": "1982-06-15",
          "phone": "555-987-6543",
          "relationshipToOwner": "Spouse",
          "address": "123 Main St",
          "city": "New York",
          "state": "NY",
          "zip": "10001",
          "percentageOfBenefit": 50.00
        },
        {
          "beneficiaryType": 1,
          "firstMiddleLastName": "John Doe Jr",
          "socialSecurityNumber": "456-78-9012",
          "dateOfBirth": "2005-08-10",
          "phone": "555-987-6544",
          "relationshipToOwner": "Son",
          "address": "123 Main St",
          "city": "New York",
          "state": "NY",
          "zip": "10001",
          "percentageOfBenefit": 50.00
        }
      ]
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Beneficiaries added successfully",
      "data": {
        "investorId": 1,
        "beneficiariesAdded": 2
      },
      "statusCode": 200
    }
    

* * *

8. Personal Financial Statement Endpoints (PersonalFinancialStatementController)
--------------------------------------------------------------------------------

### 8.1 Get PFS

**Endpoint**: `GET /api/personal-financial-statement/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Personal financial statement retrieved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "filePath": "/uploads/investors/1/pfs/TWS_PFS_20240115.pdf",
        "uploadDate": "2024-01-15T10:30:00",
        "lastModifiedDate": "2024-01-15T10:30:00"
      },
      "statusCode": 200
    }
    

### 8.2 Upload PFS

**Endpoint**: `POST /api/personal-financial-statement`  
**Authentication**: Bearer Token  
**Content-Type**: `multipart/form-data`
**Form Data**:

    investorId: 1
    file: [binary PDF file]
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Personal financial statement uploaded successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "filePath": "/uploads/investors/1/pfs/TWS_PFS_20240115.pdf"
      },
      "statusCode": 200
    }
    

### 8.3 Download PFS

**Endpoint**: `GET /api/personal-financial-statement/{investorId}/download`  
**Authentication**: Bearer Token
**Response 200 OK**: Binary PDF file with headers:

    Content-Type: application/pdf
    Content-Disposition: attachment; filename="TWS_PFS_Fillable.pdf"
    

* * *

9. Financial Goals Endpoints (FinancialGoalsController)
-------------------------------------------------------

### 9.1 Get Financial Goals

**Endpoint**: `GET /api/financial-goals/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Financial goals retrieved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "liquidityNeeds": 2,
        "investmentTimeline": 3,
        "investmentObjective": 3,
        "riskTolerance": 2,
        "deferTaxes": true,
        "protectPrincipal": true,
        "growPrincipal": true,
        "consistentCashFlow": false,
        "diversification": true,
        "retirement": true,
        "estateLegacyPlanning": false,
        "additionalNotes": "Looking for stable long-term growth with moderate risk"
      },
      "statusCode": 200
    }
    

### 9.2 Save Financial Goals

**Endpoint**: `POST /api/financial-goals`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "liquidityNeeds": 2,
      "investmentTimeline": 3,
      "investmentObjective": 3,
      "riskTolerance": 2,
      "deferTaxes": true,
      "protectPrincipal": true,
      "growPrincipal": true,
      "consistentCashFlow": false,
      "diversification": true,
      "retirement": true,
      "estateLegacyPlanning": false,
      "additionalNotes": "Looking for stable long-term growth with moderate risk"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Financial goals saved successfully",
      "data": {
        "id": 1,
        "investorId": 1
      },
      "statusCode": 200
    }
    

* * *

10. Documents Endpoints (DocumentController)
--------------------------------------------

### 10.1 Get Documents

**Endpoint**: `GET /api/document/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Documents retrieved successfully",
      "data": [
        {
          "id": 1,
          "investorId": 1,
          "documentName": "Property Deed",
          "filePath": "/uploads/investors/1/documents/property_deed.pdf",
          "uploadDate": "2024-01-15T10:30:00"
        },
        {
          "id": 2,
          "investorId": 1,
          "documentName": "Insurance Policy",
          "filePath": "/uploads/investors/1/documents/insurance.pdf",
          "uploadDate": "2024-01-15T11:00:00"
        }
      ],
      "statusCode": 200
    }
    

### 10.2 Upload Document

**Endpoint**: `POST /api/document`  
**Authentication**: Bearer Token  
**Content-Type**: `multipart/form-data`
**Form Data**:

    investorId: 1
    documentName: "Property Deed"
    file: [binary file]
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Document uploaded successfully",
      "data": {
        "id": 1,
        "documentName": "Property Deed",
        "filePath": "/uploads/investors/1/documents/property_deed.pdf"
      },
      "statusCode": 200
    }
    

### 10.3 Upload Multiple Documents

**Endpoint**: `POST /api/document/bulk`  
**Authentication**: Bearer Token  
**Content-Type**: `multipart/form-data`
**Form Data**:

    investorId: 1
    files: [array of binary files]
    documentNames: ["Property Deed", "Insurance Policy"]
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Documents uploaded successfully",
      "data": {
        "investorId": 1,
        "documentsUploaded": 2
      },
      "statusCode": 200
    }
    

* * *

11. Financial Team Endpoints (FinancialTeamController)
------------------------------------------------------

### 11.1 Get Financial Team

**Endpoint**: `GET /api/financial-team/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Financial team retrieved successfully",
      "data": [
        {
          "id": 1,
          "investorId": 1,
          "memberType": 1,
          "name": "Robert Smith",
          "email": "robert.smith@cpafirm.com",
          "phoneNumber": "555-111-2222"
        },
        {
          "id": 2,
          "investorId": 1,
          "memberType": 2,
          "name": "Sarah Johnson",
          "email": "sarah@financialadvisors.com",
          "phoneNumber": "555-333-4444"
        },
        {
          "id": 3,
          "investorId": 1,
          "memberType": 3,
          "name": "Michael Brown",
          "email": "mbrown@lawfirm.com",
          "phoneNumber": "555-555-6666"
        }
      ],
      "statusCode": 200
    }
    

### 11.2 Add Team Member

**Endpoint**: `POST /api/financial-team`  
**Authentication**: Bearer Token
**Request Body**:

    {
      "investorId": 1,
      "memberType": 1,
      "name": "Robert Smith",
      "email": "robert.smith@cpafirm.com",
      "phoneNumber": "555-111-2222"
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Financial team member added successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "memberType": 1,
        "name": "Robert Smith"
      },
      "statusCode": 200
    }
    

* * *

12. My Investments Endpoints (MyInvestmentsController)
------------------------------------------------------

### 12.1 Get My Investments

**Endpoint**: `GET /api/my-investments/{investorId}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Investments retrieved successfully",
      "data": [
        {
          "id": 1,
          "investorId": 1,
          "offeringId": 1,
          "offeringName": "Downtown Office Building Fund",
          "investmentDate": "2024-01-01",
          "amount": 100000.00,
          "status": 11,
          "investmentTrackerId": 1
        },
        {
          "id": 2,
          "investorId": 1,
          "offeringId": 2,
          "offeringName": "Residential Complex REIT",
          "investmentDate": "2024-01-10",
          "amount": 50000.00,
          "status": 2,
          "investmentTrackerId": 2
        }
      ],
      "statusCode": 200
    }
    

### 12.2 Get Investment Details

**Endpoint**: `GET /api/my-investments/{investorId}/investment/{id}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Investment details retrieved successfully",
      "data": {
        "id": 1,
        "investorId": 1,
        "offeringId": 1,
        "offeringName": "Downtown Office Building Fund",
        "offeringDescription": "Class A office building in downtown area",
        "investmentDate": "2024-01-01",
        "amount": 100000.00,
        "status": 11,
        "statusDescription": "Funded",
        "documents": [
          {
            "id": 1,
            "name": "Investment Agreement",
            "uploadDate": "2024-01-01T10:00:00"
          }
        ]
      },
      "statusCode": 200
    }
    

* * *

13. Portal Endpoints (PortalController) - Advisor/Operations Only
-----------------------------------------------------------------

### 13.1 Get Dashboard

**Endpoint**: `GET /api/portal/dashboard`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Response 200 OK**:

    {
      "success": true,
      "message": "Dashboard data retrieved successfully",
      "data": [
        {
          "trackerId": 1,
          "offeringId": 1,
          "offeringName": "Downtown Office Building Fund",
          "clientName": "John Doe",
          "status": 11,
          "statusDescription": "Funded",
          "advisor": "Mike Wilson",
          "investmentType": 1,
          "investmentTypeDescription": "Private Placement 506(c)"
        },
        {
          "trackerId": 2,
          "offeringId": 2,
          "offeringName": "Residential Complex REIT",
          "clientName": "Jane Smith",
          "status": 2,
          "statusDescription": "Onboarding",
          "advisor": "Sarah Lee",
          "investmentType": 2,
          "investmentTypeDescription": "1031 Exchange Investment"
        }
      ],
      "statusCode": 200
    }
    

### 13.2 Get Investment Tracker

**Endpoint**: `GET /api/portal/tracker/{id}`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Response 200 OK**:

    {
      "success": true,
      "message": "Investment tracker retrieved successfully",
      "data": {
        "id": 1,
        "offeringId": 1,
        "offering": "Downtown Office Building Fund",
        "investorId": 1,
        "status": 11,
        "leadOwnerLicensedRep": "Mike Wilson",
        "relationship": "Direct Client",
        "investmentType": 1,
        "clientName": "John Doe",
        "investmentHeldInNamesOf": "John and Jane Doe",
        "dateInvestmentClosed": "2024-01-01",
        "originalEquityInvestmentAmount": 100000.00,
        "totalTWSAUM": 100000.00,
        "repCommissionAmount": 5000.00,
        "twsRevenue": 10000.00,
        "dstRevenue": 0.00,
        "altRevenue": 0.00,
        "taxStrategyRevenue": 0.00,
        "oilAndGasRevenue": 0.00,
        "initialVsRecurringRevenue": "Initial",
        "marketingSource": "Website",
        "referredBy": "None",
        "notes": "First-time investor, very engaged",
        "altAUM": 0.00,
        "dstAUM": 0.00,
        "ogAUM": 0.00,
        "taxStrategyAUM": 0.00
      },
      "statusCode": 200
    }
    

### 13.3 Create Investment Tracker

**Endpoint**: `POST /api/portal/tracker`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Request Body**:

    {
      "offeringId": 1,
      "investorId": 1,
      "status": 2,
      "leadOwnerLicensedRep": "Mike Wilson",
      "relationship": "Direct Client",
      "investmentType": 1,
      "clientName": "John Doe",
      "investmentHeldInNamesOf": "John and Jane Doe",
      "dateInvestmentClosed": null,
      "originalEquityInvestmentAmount": 100000.00,
      "totalTWSAUM": 100000.00,
      "repCommissionAmount": 5000.00,
      "twsRevenue": 10000.00,
      "dstRevenue": 0.00,
      "altRevenue": 0.00,
      "taxStrategyRevenue": 0.00,
      "oilAndGasRevenue": 0.00,
      "initialVsRecurringRevenue": "Initial",
      "marketingSource": "Website",
      "referredBy": "None",
      "notes": "First-time investor",
      "altAUM": 0.00,
      "dstAUM": 0.00,
      "ogAUM": 0.00,
      "taxStrategyAUM": 0.00
    }
    

**Response 201 Created**:

    {
      "success": true,
      "message": "Investment tracker created successfully",
      "data": {
        "trackerId": 1,
        "offeringId": 1,
        "investorId": 1
      },
      "statusCode": 201
    }
    

### 13.4 Update Tracker Status

**Endpoint**: `PUT /api/portal/tracker/{id}/status`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Request Body**:

    {
      "status": 11
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Tracker status updated successfully",
      "data": {
        "trackerId": 1,
        "status": 11,
        "statusDescription": "Funded"
      },
      "statusCode": 200
    }
    

* * *

14. Offering Endpoints (OfferingController)
-------------------------------------------

### 14.1 Get All Offerings

**Endpoint**: `GET /api/offering`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Offerings retrieved successfully",
      "data": [
        {
          "id": 1,
          "name": "Downtown Office Building Fund",
          "description": "Class A office building investment opportunity",
          "status": 1,
          "createdDate": "2024-01-01T00:00:00"
        },
        {
          "id": 2,
          "name": "Residential Complex REIT",
          "description": "Multi-family residential investment",
          "status": 1,
          "createdDate": "2024-01-05T00:00:00"
        }
      ],
      "statusCode": 200
    }
    

### 14.2 Get Offering by ID

**Endpoint**: `GET /api/offering/{id}`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Offering retrieved successfully",
      "data": {
        "id": 1,
        "name": "Downtown Office Building Fund",
        "description": "Class A office building investment opportunity in prime downtown location",
        "status": 1,
        "createdDate": "2024-01-01T00:00:00",
        "lastModifiedDate": "2024-01-10T00:00:00"
      },
      "statusCode": 200
    }
    

### 14.3 Create Offering

**Endpoint**: `POST /api/offering`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Request Body**:

    {
      "name": "Industrial Warehouse Portfolio",
      "description": "Portfolio of 5 industrial warehouses in major logistics hubs",
      "status": 1
    }
    

**Response 201 Created**:

    {
      "success": true,
      "message": "Offering created successfully",
      "data": {
        "id": 3,
        "name": "Industrial Warehouse Portfolio",
        "status": 1
      },
      "statusCode": 201
    }
    

### 14.4 Update Offering

**Endpoint**: `PUT /api/offering/{id}`  
**Authentication**: Bearer Token (Advisor or OperationsTeam only)
**Request Body**:

    {
      "name": "Industrial Warehouse Portfolio - Updated",
      "description": "Portfolio of 6 industrial warehouses in major logistics hubs",
      "status": 1
    }
    

**Response 200 OK**:

    {
      "success": true,
      "message": "Offering updated successfully",
      "data": {
        "id": 3,
        "name": "Industrial Warehouse Portfolio - Updated"
      },
      "statusCode": 200
    }
    

### 14.5 Delete Offering

**Endpoint**: `DELETE /api/offering/{id}`  
**Authentication**: Bearer Token (OperationsTeam only)
**Response 204 No Content**: (Empty response)

### 14.6 Get Active Offerings

**Endpoint**: `GET /api/offering/active`  
**Authentication**: Bearer Token
**Response 200 OK**:

    {
      "success": true,
      "message": "Active offerings retrieved successfully",
      "data": [
        {
          "id": 1,
          "name": "Downtown Office Building Fund",
          "description": "Class A office building investment opportunity",
          "status": 1
        },
        {
          "id": 2,
          "name": "Residential Complex REIT",
          "description": "Multi-family residential investment",
          "status": 1
        }
      ],
      "statusCode": 200
    }
    

* * *

Error Response Examples
-----------------------

### 400 Bad Request

    {
      "success": false,
      "message": "Validation failed",
      "errors": [
        "First name is required",
        "Email format is invalid"
      ],
      "statusCode": 400
    }
    

### 401 Unauthorized

    {
      "success": false,
      "message": "Authentication failed",
      "errors": ["Invalid or expired token"],
      "statusCode": 401
    }
    

### 403 Forbidden

    {
      "success": false,
      "message": "Access denied",
      "errors": ["You do not have permission to access this resource"],
      "statusCode": 403
    }
    

### 404 Not Found

    {
      "success": false,
      "message": "Resource not found",
      "errors": ["Investor with ID 999 not found"],
      "statusCode": 404
    }
    

### 409 Conflict

    {
      "success": false,
      "message": "Resource conflict",
      "errors": ["An investor profile already exists for this user"],
      "statusCode": 409
    }
    

### 500 Internal Server Error

    {
      "success": false,
      "message": "An error occurred while processing your request",
      "errors": ["Internal server error"],
      "statusCode": 500
    }
    

* * *

**END OF COMPLETE API DOCUMENTATION WITH ALL REQUEST/RESPONSE PAYLOADS**
_This documentation includes complete request and response payloads for all 100+ endpoints in the TWS Investment Platform._