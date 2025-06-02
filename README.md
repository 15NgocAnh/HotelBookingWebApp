# Hotel Booking System

![CI Status](https://github.com/yourusername/HotelBooking/workflows/Continuous%20Integration/badge.svg)
![CD Status](https://github.com/yourusername/HotelBooking/workflows/Continuous%20Deployment/badge.svg)

## Overview

Hotel Booking System is a comprehensive solution for managing hotel reservations, rooms, and guests. Built using Clean Architecture, Domain-Driven Design (DDD), and Command Query Responsibility Segregation (CQRS) patterns.

## Architecture

### Clean Architecture

The application follows Clean Architecture with the following layers:

1. **Domain Layer** - Contains business entities, value objects, domain events, and business logic
2. **Application Layer** - Contains application use cases implemented through CQRS (commands/queries)
3. **Infrastructure Layer** - Contains implementations of repositories, external services, and data access
4. **API Layer** - Contains API controllers and presentation logic

### Domain-Driven Design (DDD)

We've applied DDD principles:

- **Aggregates** - Room, Booking, User
- **Value Objects** - Money, DateRange
- **Domain Events** - BookingCreated, RoomStatusChanged
- **Repositories** - IRoomRepository, IBookingRepository
- **Domain Services** - For complex business rules spanning multiple aggregates

### CQRS Pattern

- **Commands** - For operations that change state (Create, Update, Delete)
- **Queries** - For operations that read state
- **Mediator** - Using MediatR to dispatch commands and queries

## Technology Stack

- **.NET 8** - Core framework
- **Entity Framework Core** - ORM
- **MediatR** - For implementing CQRS
- **FluentValidation** - For request validation
- **xUnit** - For testing
- **SQL Server** - Database
  
## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server

### Installation

1. Clone the repository
```bash
git clone https://github.com/15NgocAnh/HotelBookingWebApp.git
```

2. Navigate to the project directory
```bash
cd HotelBookingWebApp
```

3. Restore NuGet packages
```bash
dotnet restore
```

4. Update the connection string in `appsettings.json`

5. Apply migrations
```bash
dotnet ef database update --project HotelBooking.Infrastructure
```

6. Run the application
```bash
dotnet run --project HotelBooking.API
```

## Project Structure

```
├── HotelBooking.Domain            # Core domain logic
├── HotelBooking.Application       # Application services, CQRS
├── HotelBooking.Infrastructure    # Data access, external services
├── HotelBooking.API               # API endpoints, controllers
├── HotelBooking.UnitTests         # Unit tests
├── HotelBooking.IntegrationTests  # Integration tests
└── .github                        # CI/CD workflows
```

## Testing

### Running Tests

```bash
dotnet test
```

## CI/CD Pipeline

Our CI/CD pipeline includes:

1. **Continuous Integration**
   - Build verification
   - Unit and integration tests
   - Code quality analysis with SonarCloud
   - Security scanning

2. **Continuous Deployment**
   - Automated deployment to Azure
   - Database migrations
   - Deployment notifications

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details. 
