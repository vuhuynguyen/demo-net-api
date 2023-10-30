# Overview

This is a .NET application that implements the Hexagon Architecture, also known as Ports and Adapters or Clean Architecture. It provides a modular and testable design approach for building robust and maintainable software.

## Architecture Overview

The Hexagon Architecture separates the core business logic from external concerns such as frameworks, databases, and user interfaces. It consists of three main layers:

1. **Domain Layer**: Contains the core business logic and entities. This layer defines the domain model and encapsulates the essential behaviors and rules of the application.

2. **Application Layer**: Orchestrates the flow of data and logic between the external interfaces and the domain layer. It provides the application's use cases and services, acting as the entry point for external interactions.

3. **Infrastructure Layer**: Handles communication with external systems, such as databases, web APIs, or messaging systems. It contains adapters that implement the interfaces defined in the application layer.

This architecture promotes flexibility, testability, and maintainability by enforcing a clear separation of concerns and reducing dependencies on external frameworks.

## Architecture Diagram
![download](https://github.com/vuhuynguyen/dotnet-api-application/assets/39457047/80964f71-7c0b-4224-bc27-1c72c95cb07d)

## Live demo
Check out the live demo of the project hosted on Azure.
Please note that if it's using a free service plan, it may take some time to wake the server up at the first-time access.

- Swagger API Documentation: [API Documentation URL](https://donet-test-api.azurewebsites.net/swagger/index.html)

# Development

### Prerequisites
- .NET 7

### Database installation
1.  Update the connection string in appsettings.
2.  Set the Migrations project as a startup project.
3.	Run EF update-database on the Migrations project to generate the database.

### Run application
1.  Update the connection string in appsettings.
1.  Set project WebApi as a startup project.

# Test
- UnitTests project for domain logic tests.
- IntegrationTests project for testing with real things such as databases,...

#### Database installation
1.	Run EF update-database on the Migrations project to generate a test database
2.  Update the connection string Tests project
