# CleanArchitectureBlazorTodoList

A modern, scalable, and maintainable Todo List application built with Blazor Server, following Clean Architecture principles.

## About

This project demonstrates how to implement a simple Todo List application using Blazor Server while adhering to Clean Architecture guidelines. It serves as an educational resource and a starting point for developers looking to build robust, testable, and scalable web applications with Blazor.

## Key Features

- Clean Architecture implementation
- Blazor Server frontend
- ASP.NET Core backend API
- Entity Framework Core for data persistence
- CQRS pattern with MediatR
- Unit and integration tests
- Dependency injection
- Responsive UI with Bootstrap

## Architecture

The solution is divided into four main projects:

1. Domain: Contains entities, interfaces, and business logic
2. Infrastructure: Implements data access and external services
3. Application: Defines use cases and application-specific logic
4. WebUIServer: Blazor Server UI (frontend)
5. API: API Controllers (backend)

This separation of concerns allows for better maintainability, testability, and scalability of the application.

## Getting Started

Clone the repository and follow the setup instructions in the README to run the project locally. Explore the codebase to learn how Clean Architecture principles are applied in a Blazor Server context.

### Running the Application with Docker Compose

You can run the entire application stack (frontend, backend, and MongoDB) using Docker Compose. Follow the steps below for your operating system.

#### Prerequisites

- Docker
- Docker Compose

#### Steps for Linux

1. **Clone the repository**:
    ```sh
    git clone https://github.com/vaayroon/CleanArchitectureBlazorTodoList.git
    cd CleanArchitectureBlazorTodoList
    ```

2. **Build and run the containers**:
    ```sh
    docker compose up --build
    ```

3. **Access the application**:
    - Frontend: [http://localhost:5030](http://localhost:5030)
    - Backend: [http://localhost:5206/swagger](http://localhost:5206/swagger)
    - MongoDB: `mongodb://admin:password1@localhost:27017/`

#### Steps for Windows

1. **Clone the repository**:
    ```sh
    git clone https://github.com/vaayroon/CleanArchitectureBlazorTodoList.git
    cd CleanArchitectureBlazorTodoList
    ```

2. **Build and run the containers**:
    ```sh
    docker-compose up --build
    ```

3. **Access the application**:
    - Frontend: [http://localhost:5030](http://localhost:5030)
    - Backend: [http://localhost:5206/swagger](http://localhost:5206/swagger)
    - MongoDB: `mongodb://admin:password1@localhost:27017/`


## Contributing

Contributions are welcome! Please read the contributing guidelines before submitting pull requests.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
