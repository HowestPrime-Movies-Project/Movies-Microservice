# HowestPrime — Movies Microservice

A lightweight, production-oriented microservice that manages movie data for the HowestPrime platform. Designed to be RESTful, observable, and easy to integrate into a larger microservice ecosystem.

Key features

- REST API for managing movies (create, read, update, delete).
- Flexible querying: basic search and filtering plus pagination for large result sets.
- Event-driven integration: publishes domain events (movie created/updated/deleted) to RabbitMQ for asynchronous workflows.
- OpenAPI / Swagger documentation available at /swagger/ for interactive API exploration.
- Docker-friendly: designed to run in containers and compose setups for local development.
- Environment-configurable: connection details (RabbitMQ, DB), ports and logging are provided via environment variables.
- Health checks and readiness probes suitable for orchestration platforms.

Quick start (local)

1. Ensure RabbitMQ is running (management UI usually at http://localhost:15672/).
2. Start the service (using your preferred method: run from IDE, use your build tool, or a container).
3. Open the Swagger UI at http://localhost:8000/swagger/ to explore endpoints and try requests.

Configuration

Configure behavior through environment variables. Common ones include:

- PORT — HTTP port the service listens on (default: 8000)
- RABBITMQ_URL — RabbitMQ connection URL (e.g. amqp://guest:guest@localhost:5672)
- DATABASE_URL — Database connection string (if applicable)
- LOG_LEVEL — Logging verbosity (INFO, DEBUG, etc.)

Notes for developers

- Focused on keeping the API small and encapsulated so it can be composed with other services.
- Event messages are minimal and intend to support downstream services without tight coupling.

Useful links

- Swagger UI: http://localhost:8000/swagger/
- RabbitMQ management UI: http://localhost:15672/

Contributing

Contributions welcome — open an issue or PR with a clear change description. Keep changes small and add tests where appropriate.

License

Apache 2.0 License. See LICENSE file for details.