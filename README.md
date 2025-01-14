# Revenue Recognition System

## Project Overview
This project involves developing a Revenue Recognition System as a REST API for a large corporation, ABC. The application addresses a critical financial issue known as "revenue recognition," ensuring compliance with legal and professional standards while maintaining transparency.

## Problem Statement
Revenue recognition involves determining when received payments can be recorded as company income. This becomes complex for services or products delivered over time, where revenue may need to be spread across the service duration. Ensuring proper revenue recognition prevents financial misrepresentation and builds trust in financial markets.

## Features

### 1. Customer Management
- Details for individual and corporate customers are stored:
  - **Individuals**: Name, address, email, phone number, and PESEL (immutable).
  - **Companies**: Name, address, email, phone number, and KRS (immutable).
- Functionalities:
  - Add new customers.
  - Soft-delete individual customers (retain records but anonymize data).
  - Update customer details.

### 2. Software Licensing
#### 2.1 Software Products
- Project manages details of software products, including name, description, version, and category (e.g., finance, education).
- Products can be sold via subscriptions or one-time purchases with update rights for a specified period.

#### 2.2 Discounts
- Discounts can apply to purchases or first subscription periods.
- Managed by:
  - Name, value (%), and active time range.
  - Automatically selecting the highest applicable discount.
- Loyal customers (those with prior purchases) get an additional 5% discount.

#### 2.3 Contract Management
- Software purchase contracts are created and managed with:
  - Defined start and end dates (3-30 days validity).
  - Pricing, including additional support years (1, 2, or 3 years, each adding 1000 PLN).
  - Discounts applied.
- Contract Lifecycle:
  - Await full payment within the validity period.
  - Mark as signed after full payment.
  - Treat contract value as revenue only after full payment.
  - Invalidate unpaid contracts after expiration and refund partial payments.
- The project ensures one active contract or subscription per product per customer.

### 3. Revenue Calculation
- Available calculations:
  - **Current Revenue**: Based on completed payments.
  - **Forecasted Revenue**: Assuming all pending contracts are signed.
- The following revenue calculations are supported:
  - Entire company.
  - Specific products.
  - Conversion to specified currencies using live exchange rates.

## Technical Details
- Developed using .NET 8 and Entity Framework.
- REST API endpoints secured with authentication and role-based access control:
  - Admins can edit and delete client data.
  - Standard users handle other use cases.
- Swagger integration for API testing.
- Includes unit tests for business logic validation.
