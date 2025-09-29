# TWS Investment Platform - UI/UX Requirements

**Source**: Pages.md
**Purpose**: Detailed UI/UX specifications for web application
**Status**: Extracted and consolidated

---

## Table of Contents

1. [Landing & Authentication](#1-landing--authentication)
2. [User Roles & Access](#2-user-roles--access)
3. [Investor Landing Page](#3-investor-landing-page)
4. [Home/Gallery Pages](#4-homegallery-pages)
5. [My Profile Section](#5-my-profile-section)
6. [Offering Details Page](#6-offering-details-page)
7. [Password Requirements](#7-password-requirements)
8. [Contact & Communication](#8-contact--communication)

---

## 1. Landing & Authentication

### 1.1 Homepage (www.tws.com)

**Intro Page**:
- TWS logo (large, background)
- Two prominent buttons:
  - **Login**
  - **Request Account**

**Navigation**:
- **About** page - Information about TWS
- **Disclosures** page - All relevant disclosures

### 1.2 Login Flow

**Login Screen**:
- Email textbox
- Password textbox
- **Submit** button
- **Forgot Password** link

**Error Handling**:
- Display "Invalid Credentials" if authentication fails

**Forgot Password Flow**:
1. Click "Forgot Password"
2. Enter email in textbox
3. Click Submit
4. Send password reset email (template below)
5. Link expires in **3 hours**

**Password Reset Email Template**:
```
Subject: TWS - Please reset your password

Reset your TWS password

We heard that you lost your TWS password. Sorry about that!

But don't worry! You can use the following button to reset your password:

[Reset your password] (button with reset link)

If you don't use this link within 3 hours, it will expire.
Click here to get a new password reset link.

Thanks,
TWS Team
```

### 1.3 Request Account Flow

**Request Account Screen**:
- Email textbox
- Submit button
- Purpose: Send to advisor for due diligence and account creation

### 1.4 Disclosures

**Implementation**:
- Separate page for each disclosure
- Each disclosure has:
  - Full disclosure text
  - Checkbox: "I have read this disclosure"
- Track which disclosures user has acknowledged

---

## 2. User Roles & Access

### 2.1 User Role Identification

**Role Assignment Based on Email Domain**:

1. **TWS Admin**:
   - Provided to developers
   - Global access to all features

2. **Advisor/Rep** (yourtws.com suffix):
   - Email ending with `@yourtws.com`
   - Rep account privileges
   - Access to Portal/CRM

3. **Investor** (all other emails):
   - Standard investor account
   - Access to own profiles and investments

### 2.2 Permission Summary

| Feature | Investor | Advisor | Operations/Admin |
|---------|----------|---------|------------------|
| My Profiles | Full Access | Read Only | Full Access |
| My Investments | Full Access | View All | View All |
| My Offerings Gallery | View | Full Access | Full Access |
| Portal/CRM | No Access | Full Access | Full Access |
| Add Offerings | No | Yes | Yes |
| Edit Offerings | No | Yes | Yes |

---

## 3. Investor Landing Page

### 3.1 After Successful Login

**Page Layout**:
- Gallery view with two types of thumbnails:
  1. Investor Profiles
  2. Investments

### 3.2 Offering Thumbnail Design

**Display Elements**:
- Picture of offering
- Name of offering
- **Tag** (top-right corner): Investment type
  - Private Placement 506(c)
  - 1031 Exchange Investment
  - Universal Offering
  - Tax Strategy
  - Roth IRA Conversion
- Description of investment
- Total value of investment
- Status badge: **Raising** or **Closed**
- **Details** button

### 3.3 Investor Profile Thumbnail Design

**Display Elements**:
- Name of investor
- **Tag** (top-right corner): Investor type
  - Individual
  - Joint
  - Trust
  - Entity
  - IRA
- Net Worth
- Names of 2 most recent investments
- **Edit** button/option

### 3.4 Additional Features

- **My Advisor** section
- **My Tasks** section
- **See More** option → redirects to full gallery

---

## 4. Home/Gallery Pages

### 4.1 Investor Home Page

**Access**: Click "See More" from Landing Page

**Features**:
- Gallery of all offerings
- **Filter** option at top:
  - Private Placement 506(c)
  - 1031 Exchange Investment
  - Universal Offering
  - Tax Strategy
  - Roth IRA Conversion
- **Contact Advisor** button (left bottom corner)
  - Opens email collection screen
  - Sends to advisor for follow-up

**Offering Thumbnail** (same as landing page):
- Picture
- Name
- Type tag
- Description
- Total value
- Status (Raising/Closed)
- Details button

### 4.2 Advisor Home Page

**Toggle Options** (top-left corner):
- **My Offerings** (default view)
- **My Investors**

#### My Offerings View

**Features**:
- Gallery of all offerings
- **Filter** by type:
  - Private Placement 506(c)
  - 1031 Exchange Investment
  - Universal Offering
  - Tax Strategy
  - Roth IRA Conversion
- Same thumbnail design as investor view
- Click Details → Offering page

#### My Investors View

**Features**:
- Gallery of all investors
- **Filter** by investor type:
  - Individual
  - Joint
  - Trust
  - Entity
  - IRA
- **Read-only permissions** for investor profiles

**Investor Thumbnail**:
- Name of investor
- Type tag (top-right)
- Net Worth
- Names of recent investments
- Click thumbnail → Investor profile page

---

## 5. My Profile Section

### 5.1 Investor My Profile

**Location**: Right corner after login

**Dropdown Menu Options**:
- **My Investor Profiles**
- **My Account**
- **Logout**

#### My Account Page

**Fields**:
- Email (editable)
- Name (editable)
- Password field

**Reset Password Section**:
- Old Password textbox
- New Password textbox (with requirements shown)
- Confirm Password textbox
- Submit button

### 5.2 Advisor My Profile

**Location**: Right corner after login

**Dropdown Menu Options**:
- **My Portal**
- **My Offerings**
- **My Account**
- **Logout**

#### My Account Page
(Same as Investor - see 5.1)

#### My Offerings Page

**Features**:
- Gallery of advisor's offerings
- **Edit** option for each offering
- **Add New Offering** button (left corner)

**Add New Offering Form**:
- Picture upload
- Name of offering
- Type tag dropdown:
  - Private Placement 506(c)
  - 1031 Exchange Investment
  - Universal Offering
  - Tax Strategy
  - Roth IRA Conversion
- Description textarea
- Total value input
- Status dropdown: Raising/Closed
- PDF upload
- Additional documents upload

---

## 6. Offering Details Page

### 6.1 Main Content Area

**Center Section**:
- Full PDF of offering details
- Downloadable with download icon at top
- PDF viewer/display

### 6.2 Left Sidebar

**Documents Button**:
- Click to show additional documents at bottom of page
- All documents downloadable

**Contact Advisor Button** (bottom-left):
- Opens email collection screen
- Sends to advisor for follow-up

### 6.3 Bottom Section (Advisors Only)

**Investor List**:
- Shows all investor profiles with **closed investments** in this offering
- List format with key details
- Click to view investor profile

---

## 7. Password Requirements

**Display on Password Creation/Reset**:

Password must be **8-24 characters** and include:
- ✅ At least 1 uppercase letter
- ✅ At least 1 number
- ✅ At least 1 special character

**Implementation**:
- Show requirements on screen
- Real-time validation feedback
- Prevent submission if requirements not met

---

## 8. Contact & Communication

### 8.1 Contact Advisor Feature

**Locations**:
- Home/Gallery page (bottom-left)
- Offering Details page (bottom-left)

**Flow**:
1. User clicks "Contact Advisor"
2. Email collection screen opens
3. User enters email (or pre-filled if logged in)
4. Submit
5. Email sent to assigned advisor or operations team

### 8.2 Email Notifications

**Triggers**:
- New account request
- Contact advisor request
- Password reset request
- Profile completion milestones (optional)

---

## Additional UI/UX Guidelines

### Gallery/Thumbnail Best Practices

1. **Consistent Card Sizes**: All thumbnails same dimensions
2. **Hover Effects**: Show more info on hover
3. **Loading States**: Show skeleton loaders while fetching
4. **Empty States**: Message when no items in gallery
5. **Pagination**: If many items (or infinite scroll)

### Responsive Design

- Mobile-first approach
- Gallery adjusts columns based on screen size
- Desktop: 3-4 columns
- Tablet: 2 columns
- Mobile: 1 column, stacked

### Color Coding (Suggestions)

**Status Badges**:
- Raising: Blue/Green
- Closed: Gray
- Coming Soon: Orange/Yellow

**Investor Type Tags**:
- Individual: Blue
- Joint: Purple
- Trust: Green
- Entity: Orange
- IRA: Red/Maroon

**Investment Type Tags**:
- Private Placement 506(c): Dark Blue
- 1031 Exchange: Green
- Universal Offering: Purple
- Tax Strategy: Orange
- Roth IRA Conversion: Red

---

## Implementation Notes

### Priority 1 (MVP)
- Login/Authentication
- Request Account
- Landing page with galleries
- Basic offering thumbnails
- My Profile → My Account

### Priority 2
- Filters for galleries
- My Offerings (advisor)
- My Investors (advisor)
- Offering details page

### Priority 3
- Contact Advisor functionality
- Disclosure pages with checkboxes
- My Tasks section
- My Advisor section

---

## Wireframe References

### Login Page
```
+----------------------------------+
|           TWS LOGO               |
|                                  |
|    [LOGIN]  [REQUEST ACCOUNT]   |
|                                  |
|    About         Disclosures     |
+----------------------------------+
```

### Landing Page (Investor)
```
+----------------------------------+
|  My Profile ▼              [👤] |
+----------------------------------+
|                                  |
|  +------+  +------+  +------+   |
|  |Offer1| |Offer2| |Invest1|   |
|  |      | |      | |      |    |
|  +------+  +------+  +------+   |
|                                  |
|  +------+  +------+             |
|  |Invest2| |Invest3|            |
|  +------+  +------+             |
|                                  |
|           [See More]             |
+----------------------------------+
```

### Gallery Page with Filter
```
+----------------------------------+
|  Filters: [All ▼]          [👤] |
+----------------------------------+
|                                  |
|  +------+  +------+  +------+   |
|  |Offer1| |Offer2| |Offer3|    |
|  |      | |      | |      |    |
|  +------+  +------+  +------+   |
|                                  |
|  +------+  +------+  +------+   |
|  |Offer4| |Offer5| |Offer6|    |
|  +------+  +------+  +------+   |
|                                  |
| [Contact Advisor]                |
+----------------------------------+
```

---

**END OF UI/UX REQUIREMENTS**

*These requirements complement the business and functional requirements. Implement UI exactly as specified.*