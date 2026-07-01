WorkServices API
WorkServices – Home Services Marketplace API

A production-oriented ASP.NET Core Web API that connects customers with skilled artisans for home services including plumbing, electrical work, carpentry, painting, cleaning, appliance repairs, and more.

The platform enables customers to request services, receive quotations, approve work, make payments, receive live notifications, and review completed jobs while artisans manage assignments throughout the job lifecycle.

Project Overview

WorkServices demonstrates modern backend engineering practices using Clean Architecture, CQRS, Domain-Driven Design principles, Domain Events, Repository Pattern, and Unit of Work.

The project is structured to be scalable, maintainable, and suitable for enterprise-level systems.

Architecture
                    Client Apps
          (Web / Mobile / Swagger UI)
                     │
                     ▼
               ASP.NET Core API
                     │
        ┌────────────┴────────────┐
        │                         │
        ▼                         ▼
 Authentication              SignalR Hub
        │                         │
        ▼                         ▼
           Application Layer
     Commands • Queries • DTOs
             MediatR (CQRS)
                     │
                     ▼
             Domain Layer
      Entities • Events • Enums
          Business Rules
                     │
                     ▼
          Infrastructure Layer
 EF Core • PostgreSQL • Redis
 Email • JWT • Repositories
                     │
                     ▼
               PostgreSQL
Folder Structure
src
│
├── WorkServices.API
│   ├── Controllers
│   ├── Middleware
│   ├── Hubs
│   ├── Services
│   └── Program.cs
│
├── WorkServices.Application
│   ├── Common
│   ├── DTOs
│   ├── Features
│   │     ├── Commands
│   │     └── Queries
│   ├── Interfaces
│   └── Behaviors
│
├── WorkServices.Domain
│   ├── Entities
│   ├── Events
│   ├── Enums
│   └── Abstractions
│
└── WorkServices.Infrastructure
    ├── Authentication
    ├── Persistence
    ├── Repositories
    ├── Services
    ├── UnitOfWork
    └── DomainEvents
Technology Stack
ASP.NET Core 9
C#
Entity Framework Core
PostgreSQL
Redis
MediatR
SignalR
Serilog
JWT Authentication
BCrypt Password Hashing
Clean Architecture
CQRS
Domain Events
Repository Pattern
Unit of Work
Swagger / OpenAPI
Core Features
Authentication
User Registration
Login
JWT Access Tokens
Refresh Tokens
Password Hashing
Customer Features
Create Service Requests
View Requests
Approve Quotes
Pay Materials
Pay Labour
Leave Reviews
Receive Notifications
Artisan Features
Accept Jobs
Reject Jobs
Submit Quotes
Start Jobs
Complete Jobs
Notifications
SignalR Real-time Updates
Stored Notifications
Notification History
Payments
Material Payments
Labour Payments
Payment Tracking
Reviews
Ratings
Comments
Artisan Reputation
CQRS

The API follows the Command Query Responsibility Segregation pattern.

Commands

Commands modify state.

Examples

CreateServiceRequestCommand
AssignJobCommand
SubmitQuoteCommand
ApproveQuoteCommand
PayMaterialsCommand
CompleteJobCommand
Queries

Queries return data without changing state.

Examples

GetServiceRequestById
GetNotifications
GetReviewsByArtisan
GetPaymentsByServiceRequest
GetQuoteByServiceRequest

This separation makes the application easier to maintain, test, and extend.

Domain Events

Business actions automatically trigger events.

Examples

JobAssignedDomainEvent

↓

Notification

↓

Email

↓

SignalR Update

Other events include

Quote Submitted
Quote Approved
Materials Paid
Labour Paid
Job Started
Job Completed
Review Submitted

The Unit of Work dispatches events automatically after successful database commits.

SignalR

Real-time notifications are pushed to connected clients.

Examples

New Job Assignment
Quote Submitted
Payment Confirmed
Job Completed
Review Received
JWT Authentication

Authentication is implemented using JWT Bearer Tokens.

Supported functionality:

Login
Refresh Tokens
Claims-based Identity
SignalR Authentication
Authorization

Role-based authorization policies include:

CustomerOnly

ArtisanOnly

AdminOnly

Controllers and endpoints are protected using these policies.

Repository Pattern

Repositories encapsulate all persistence logic.

Examples

IUserRepository

IServiceRequestRepository

IQuoteRepository

IPaymentRepository

IReviewRepository

INotificationRepository
Unit of Work

All repositories participate in a single transaction.

Benefits include:

Atomic database updates
Automatic Domain Event Dispatch
Reduced coupling
Centralized persistence
Redis

Redis is used for caching and distributed application support.

Potential future enhancements include:

Response caching
Session storage
Background queues
PostgreSQL

Primary relational database.

Entity Framework Core manages:

Migrations
Relationships
Queries
Transactions
Health Checks

Health endpoint

GET /health

Verifies

Database Connectivity
API Availability
API Endpoints
Authentication
POST /api/auth/register

POST /api/auth/login

POST /api/auth/refresh
Service Requests
POST /api/service-requests

GET /api/service-requests/{id}

GET /api/service-requests/customer/{id}
Job Assignments
POST /api/job-assignments

POST /accept

POST /reject

POST /start

POST /complete
Quotes
POST /api/quotes

POST /approve

GET /service-request/{id}
Payments
POST /materials

POST /labour

GET /service-request/{id}
Reviews
POST /api/reviews

GET /artisan/{id}
Notifications
GET /api/notifications
Swagger

Placeholder

Insert screenshots of:

Swagger Home
Authentication
Service Requests
Payments
SignalR Endpoints
Environment Variables

Example .env

DB_HOST=localhost
DB_PORT=5432
DB_DATABASE=WorkServices
DB_USERNAME=postgres
DB_PASSWORD=password

JWT_KEY=your_secret_key

JWT_ISSUER=WorkServices

JWT_AUDIENCE=WorkServicesUsers

SMTP_HOST=

SMTP_PORT=

SMTP_USERNAME=

SMTP_PASSWORD=

SMTP_FROM=

REDIS_CONNECTION=localhost:6379

SEQ_URL=http://localhost:5341
Running Locally
git clone https://github.com/<your-username>/WorkServices.git

cd WorkServices

dotnet restore

dotnet ef database update

dotnet run --project src/WorkServices.API

Open

http://localhost:5100
Testing

The project includes support for:

Unit Tests
Integration Tests (WebApplicationFactory)
API Testing via Swagger
Postman Collections
Docker

Docker support will include:

API
PostgreSQL
Redis
Seq

Run using

docker compose up --build

(To be added once Docker Compose is complete.)

Deployment Roadmap

Future deployment targets:

Azure App Service
Azure Database for PostgreSQL
Azure Redis Cache
Azure Container Apps
GitHub Actions CI/CD
Future Improvements
FluentValidation Pipeline
API Versioning
Rate Limiting
Background Jobs (Hangfire)
Email Templates
OpenTelemetry
Metrics Dashboard
Docker Compose
Kubernetes
Distributed Caching
Multi-tenancy
License

This project is licensed under the MIT License.

See the LICENSE file for details.

Contact

Developer: Godwin Igwegbe

Email: godwincliff10@gmail.com

LinkedIn: 

GitHub: https://github.com/HendrixCliff