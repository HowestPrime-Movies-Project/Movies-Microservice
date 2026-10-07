<div align="center">

# 🎬 HowestPrime Movies Service

**A REST API for the HowestPrime platform that manages the movie catalog and publishes domain events.**

<p>
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C# badge">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET badge">
  <img src="https://img.shields.io/badge/ASP.NET_Core-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core badge">
  <img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="Entity Framework Core badge">
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger badge">
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL badge">
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white" alt="RabbitMQ badge">
</p>

</div>

> A lightweight, event-driven service that acts as the catalog source of truth for the platform.

## 📑 Table of Contents

- [📖 About](#about)
- [🏗️ Architecture](#architecture)
- [✨ Features](#features)
- [📱 API Surface](#api-surface)
- [🛠️ Tech Stack](#tech-stack)
- [🚀 Getting Started](#getting-started)
- [📄 License](#license)
- [👤 Author](#author)

## 📖 About

- This service manages movie data for the HowestPrime platform.
- It exposes a REST API for creating, reading, updating, and deleting movies.
- The service publishes domain events so other parts of the platform can react asynchronously.
- The startup project lives in `src/Howestprime.Movies.Main`.

## 🏗️ Architecture

```mermaid
flowchart TB
    API[ASP.NET Core Web API]
    APP[Application layer]
    INFRA[Infrastructure layer]
    DB[(PostgreSQL)]
    MQ[Message Broker]
    SWAGGER[Swagger / OpenAPI]

    API --> APP
    APP --> INFRA
    INFRA --> DB
    APP --> MQ
    API --> SWAGGER
```

## ✨ Features

**🎞️ Movie catalog**

- Create, update, and delete movie records.
- Retrieve movie details with filtering and pagination support.
- Keep the catalog available as the source of truth for the platform.

**📡 Integration**

- Publish movie-related events for downstream consumers.
- Keep the service container-friendly and environment-configurable.
- Expose health and API documentation endpoints for local development.

**📚 Documentation**

- Swagger UI for exploring the API.
- Contract-friendly project structure with clear application and infrastructure boundaries.

## 📱 API Surface

| Area | Details |
| --- | --- |
| REST API | Movie CRUD, filtering, and paging |
| Documentation | Swagger / OpenAPI |
| Events | Movie created, updated, and deleted messages |
| Runtime | ASP.NET Core web host |

## 🛠️ Tech Stack

| Area | Technologies |
| --- | --- |
| Language | C# |
| Framework | ASP.NET Core |
| Data access | Entity Framework Core |
| Database | PostgreSQL |
| Messaging | RabbitMQ or equivalent broker |
| Build | .NET SDK 10 |
| Docs | Swagger / OpenAPI |

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL
- A message broker for event publishing
- The required configuration values for the runtime environment

### Restore dependencies

```bash
dotnet restore
```

### Run the service

```bash
dotnet run --project src/Howestprime.Movies.Main
```

### Run tests

```bash
dotnet test
```

### Open the API docs

When the service is running, open the Swagger UI at `http://localhost:8000/swagger/`.

## 📄 License

This project is used for educational and demo purposes within the HowestPrime course context.

## 👤 Author

| Name | GitHub | LinkedIn |
| --- | --- | --- |
| Maurice De Kegel | [MriceDK](https://github.com/MriceDK) | [LinkedIn](https://www.linkedin.com/in/dekegelmaurice/) |
