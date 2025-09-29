TWS Investment Platform - Security Design Document
==================================================

Document Information
--------------------

*   **Version**: 1.0
*   **Purpose**: Define security architecture and data protection strategies
*   **Status**: Draft

* * *

1. Overview
-----------

### 1.1 Scope

This document outlines security measures for protecting sensitive investor data including:
*   Personal Identifiable Information (PII)
*   Financial documents
*   Investment records
*   Authentication credentials

### 1.2 Sensitive Data Classification

| Data Type | Classification | Examples |
| --- | --- | --- |
| **Critical** | Highly Sensitive | SSN, EIN, TIN, Bank Account Numbers |
| **Confidential** | Sensitive | Driver's License, W9, Tax Returns, Financial Statements |
| **Private** | Moderate | Name, Address, Phone, Email, Investment amounts |
| **Internal** | Low | Investment preferences, profile completion status |

* * *

2. Data Encryption Strategy
---------------------------

### 2.1 Data at Rest

#### Database Encryption

    - SSN: AES-256 encryption at field level
    - EIN/TIN: AES-256 encryption at field level  
    - Bank Details: AES-256 encryption at field level
    - Entire Database: Transparent Data Encryption (TDE) in MySQL
    

#### Encryption Implementation

    // Example structure (not actual implementation)
    public class EncryptedField
    {
        public string EncryptedValue { get; set; }
        public string EncryptionKeyId { get; set; }
        public DateTime EncryptedAt { get; set; }
    }
    

### 2.2 Data in Transit

*   **All API communications**: TLS 1.3 minimum
*   **Internal service communication**: HTTPS only
*   **Certificate pinning** for mobile/desktop clients (if applicable)

### 2.3 Data in Processing

*   Decrypt only when necessary
*   Clear sensitive data from memory after use
*   No logging of decrypted sensitive data

* * *

3. Azure Key Vault Integration
------------------------------

### 3.1 Key Vault Structure

    Production Key Vault: tws-prod-keyvault
    ├── Secrets/
    │   ├── DatabaseConnectionString
    │   ├── JwtSigningKey
    │   ├── DataEncryptionKey
    │   └── BlobStorageConnectionString
    ├── Keys/
    │   ├── SSNEncryptionKey
    │   ├── DocumentEncryptionKey
    │   └── BackupEncryptionKey
    └── Certificates/
        ├── SSLCertificate
        └── SigningCertificate
    

### 3.2 Key Rotation Policy

*   **Data Encryption Keys**: Rotate every 90 days
*   **JWT Signing Keys**: Rotate every 30 days
*   **SSL Certificates**: Before expiration
*   **Maintain key history** for decryption of historical data

### 3.3 Access Control

    Key Vault Access Policies:
      - API Application:
          Secrets: Get, List
          Keys: Decrypt, Encrypt, UnwrapKey, WrapKey
      - DevOps Pipeline:
          Secrets: Get, List, Set
      - Developers:
          Secrets: List (DEV environment only)
    

* * *

4. Azure Blob Storage for Documents
-----------------------------------

### 4.1 Storage Structure

    tws-documents-prod/
    ├── investors/
    │   └── {encrypted-investor-id}/
    │       ├── general-info/
    │       │   ├── drivers-license/
    │       │   ├── w9/
    │       │   └── entity-docs/
    │       ├── accreditation/
    │       │   ├── bank-statements/
    │       │   ├── tax-returns/
    │       │   └── licenses/
    │       ├── financial-statements/
    │       └── miscellaneous/
    

### 4.2 Blob Storage Security Configuration

    Storage Account Settings:
      - Name: twsdocumentsprod
      - Replication: GRS (Geo-Redundant Storage)
      - Access Tier: Hot
      - Secure Transfer: Required
      - Minimum TLS Version: 1.2
      - Public Access: Disabled
      - Firewall: Restrict to application IPs
      - Soft Delete: Enabled (30 days)
      - Versioning: Enabled
    

### 4.3 Document Access Pattern

    1. User requests document
    2. API validates user permission
    3. API generates SAS token (5 minute expiry)
    4. Return SAS URL to client
    5. Client downloads directly from Blob Storage
    

### 4.4 Document Encryption

*   **Server-side encryption**: Azure Storage Service Encryption (256-bit AES)
*   **Customer-managed keys** via Azure Key Vault
*   **Additional application-level encryption** for Critical documents (SSN, Tax Returns)

* * *

5. Authentication & Authorization Security
------------------------------------------

### 5.1 Password Policy

    - Minimum 12 characters
    - Uppercase, lowercase, number, special character required
    - Password history: Last 5 passwords cannot be reused
    - Password expiry: 90 days
    - Account lockout: 5 failed attempts = 30 minute lockout
    

### 5.2 JWT Token Security

    Access Token:
      - Expiration: 60 minutes
      - Signing: RS256 with certificate from Key Vault
      - Claims: UserId, Role, InvestorId
      
    Refresh Token:
      - Expiration: 7 days
      - Storage: Encrypted in database
      - Single use: Rotated on each refresh
      - Revocation: On logout or suspicious activity
    

### 5.3 Multi-Factor Authentication (MFA)

*   **Required for**: Advisors and Operations Team
*   **Optional for**: Investors (strongly recommended)
*   **Methods**: SMS, Email, Authenticator App

* * *

6. API Security Measures
------------------------

### 6.1 Rate Limiting

    Per User:
      - Authentication endpoints: 5 requests per minute
      - Data retrieval: 100 requests per minute
      - File upload: 10 requests per minute
    
    Per IP:
      - Overall: 1000 requests per hour
    

### 6.2 Input Validation

*   **All inputs sanitized** before processing
*   **SQL injection prevention** via parameterized queries
*   **XSS prevention** via output encoding
*   **File upload validation**: Type, size, content scanning

### 6.3 Security Headers

    X-Content-Type-Options: nosniff
    X-Frame-Options: DENY
    X-XSS-Protection: 1; mode=block
    Content-Security-Policy: default-src 'self'
    Strict-Transport-Security: max-age=31536000; includeSubDomains
    

* * *

7. Data Protection Implementation
---------------------------------

### 7.1 PII Handling Service

    public interface IDataProtectionService
    {
        Task<string> EncryptSSN(string plainSSN);
        Task<string> DecryptSSN(string encryptedSSN);
        Task<string> MaskSSN(string ssn); // Returns ***-**-1234
        Task<string> EncryptDocument(byte[] document);
        Task<byte[]> DecryptDocument(string encryptedDocument);
    }
    

### 7.2 Audit Logging

    Log all:
      - Authentication attempts (success/failure)
      - Access to sensitive data (who, what, when)
      - Document uploads/downloads
      - Profile modifications
      - Permission changes
      
    Store in: Azure Monitor/Log Analytics
    Retention: 7 years
    

### 7.3 Data Retention & Deletion

    Active Investor Data: Retained indefinitely while active
    Inactive Investor Data: Archived after 3 years
    Document Retention: 7 years per regulatory requirements
    Soft Delete: 30 days recovery period
    Hard Delete: Secure overwrite, audit log entry
    

* * *

8. Compliance & Regulations
---------------------------

### 8.1 Regulatory Requirements

*   **SEC Rule 17a-4**: Electronic records retention
*   **SOC 2 Type II**: Security controls
*   **PCI DSS**: If processing payments (future)
*   **State Privacy Laws**: CCPA, others as applicable

### 8.2 Data Residency

*   **Primary**: US East Azure Region
*   **Backup**: US West Azure Region
*   **No data leaves**: United States

* * *

9. Security Monitoring & Incident Response
------------------------------------------

### 9.1 Monitoring

    Azure Security Center:
      - Threat detection
      - Vulnerability assessments
      - Compliance monitoring
      
    Azure Sentinel:
      - SIEM capabilities
      - Automated threat response
      - Security orchestration
      
    Application Insights:
      - Performance monitoring
      - Error tracking
      - Custom security events
    

### 9.2 Incident Response Plan

    1. Detection: Automated alerts via Azure Monitor
    2. Containment: Automatic account lockout/service isolation
    3. Investigation: Log analysis, forensics
    4. Remediation: Patch, update, restore
    5. Recovery: Service restoration
    6. Lessons Learned: Document and update procedures
    

* * *

10. Development Security Practices
----------------------------------

### 10.1 Secure Development

*   **Code reviews** for all changes
*   **Static code analysis** via SonarQube
*   **Dependency scanning** for vulnerabilities
*   **Security testing** in CI/CD pipeline

### 10.2 Environment Separation

    Development:
      - Separate Azure subscription
      - Test data only (no production data)
      - Separate Key Vault
      
    Staging:
      - Production-like configuration
      - Anonymized data
      - Security testing environment
      
    Production:
      - Restricted access
      - Change control process
      - Audit all changes
    

### 10.3 Secrets Management

    Never in code:
      - Connection strings
      - API keys
      - Encryption keys
      - Certificates
    
    Always use:
      - Azure Key Vault references
      - Managed Identities
      - Environment variables (for local dev only)
    

* * *

11. Security Checklist
----------------------

### Pre-Deployment

*   [ ] All sensitive fields encrypted
*   [ ] Key Vault configured and tested
*   [ ] Blob Storage security configured
*   [ ] SSL certificates installed
*   [ ] Security headers configured
*   [ ] Rate limiting enabled
*   [ ] Audit logging operational
*   [ ] Backup and recovery tested

### Post-Deployment

*   [ ] Security monitoring active
*   [ ] Incident response team notified
*   [ ] Penetration testing scheduled
*   [ ] Security training completed
*   [ ] Compliance audit scheduled

* * *

12. Security Architecture Diagram
---------------------------------

    ┌─────────────────┐
    │   Web Users     │
    └────────┬────────┘
             │ HTTPS/TLS 1.3
             ▼
    ┌─────────────────┐
    │   Azure WAF     │ ◄── DDoS Protection
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐       ┌──────────────┐
    │   Web App       │◄──────│ Azure        │
    │   (MVC)         │       │ Key Vault    │
    └────────┬────────┘       └──────────────┘
             │ JWT                    ▲
             ▼                        │
    ┌─────────────────┐              │
    │   API Gateway   │──────────────┘
    │   (API)         │
    └────────┬────────┘
             │ Encrypted
             ▼
    ┌─────────────────┐       ┌──────────────┐
    │   MySQL DB      │◄──────│ Azure Blob   │
    │   (TDE Enabled) │       │ Storage      │
    └─────────────────┘       └──────────────┘
             │
             ▼
    ┌─────────────────┐
    │ Backup Storage  │
    │   (Encrypted)   │
    └─────────────────┘
    

* * *

13. Emergency Procedures
------------------------

### Data Breach Response

1.  **Immediate**: Isolate affected systems
2.  **Within 1 hour**: Notify security team and legal
3.  **Within 24 hours**: Initial assessment complete
4.  **Within 72 hours**: Regulatory notification if required
5.  **Ongoing**: User notification as required by law

### Key Compromise Response

1.  **Immediate**: Revoke compromised key
2.  **Immediate**: Rotate to backup key
3.  **Within 1 hour**: Re-encrypt affected data
4.  **Within 24 hours**: Full security audit

* * *

**END OF SECURITY DESIGN DOCUMENT**
_This document addresses security requirements for protecting sensitive investor data using Azure Key Vault for secrets management and Azure Blob Storage for document storage._