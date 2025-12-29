# ECatalog v2

ECatalog v2 is a backend-focused learning project built with .NET and Clean Architecture (Onion).
The goal is to demonstrate real-world backend engineering practices, not just CRUD.

This project prioritizes:
- Correct architecture
- Clear separation of concerns
- Production-style patterns (CQRS, migrations, observability)

## Tech Stack
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MediatR
- Clean Architecture
- OpenTelemetry, Prometheus, Grafana

## Architecture
- **ECatalog.Api** — API layer, controllers, DI, configuration
- **ECatalog.Application** — CQRS commands/queries, handlers, DTOs, use-cases.
- **ECatalog.Domain** — entities and domain logic
- **ECatalog.Infrastructure** — persistence, EF Core, migrations

## Current Status
- Project scaffolded
- Clean Architecture (Onion) structures
- EF Core migrations working
- CQRS done
- Log using Serilog and Seq, Metrics using OpenTelemetry and Prometheus, Visualization dashboard using Grafana

## Why this project exists
This project is part of a structured learning journey to move from
"working code" to "well-architected systems" and job-ready backend skills.

## Notes
This repository is intentionally built step-by-step with clarity over speed. So expect some bugs, refactors, smells, and etc.
