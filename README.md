# ECatalog v2

ECatalog v2 is a backend-focused learning project built with .NET and Clean Architecture (Onion).
The goal is to demonstrate real-world backend engineering practices, not just CRUD.

## Project Goals
- Clean Architecture with strict separation of concerns
- CQRS using MediatR for clear command/query flows
- Production-style observability (logs, metrics, alerts)

## Architecture Overview
- ECatalog.Domain. Core business entities and domain rules. No dependencies
- ECatalog.API. HTTP Layer, controllers, middlewares, DI and observability config/setup
- ECatalog.Application. Use cases, CQRS, handlers and business flow are handled here
- ECatalog.Infrastructure. Implementation and persistence concerns (EF Core, PostgreSQL, migrations, OTel wiring, implementation)
- ECatalog.Web. Front-end.

## Architectural Decisions & Tradeoffs
- CQRS was chosen to improve readability, debugging, and separation of write/read concerns. (Also learning process to understand more about CQRS)
- MediatR pipeline behaviors are used to centralize concerns such as logging and metrics.
- Business validation failures are treated differently from system failures to avoid operational metrics.
- Metrics are aggregated using OpenTelemetry and Prometheus counters with request labels instead of per-handler counters.

## Tech Stack
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MediatR (CQRS)
- Serilog + Seq (structured logging)
- OpenTelemetry + Prometheus (metrics)
- Grafana (dashboards & alerts)
- Docker (observability tooling)

## Architecture
- **ECatalog.Api** — API layer, controllers, DI, configuration
- **ECatalog.Application** — CQRS commands/queries, handlers, DTOs, use-cases.
- **ECatalog.Domain** — entities and domain logic
- **ECatalog.Infrastructure** — persistence, EF Core, migrations

## Current Status
- Clean Architecture structure in place
- CQRS implemented for CRUD use cases
- EF Core migrations working with PostgreSQL
- Structured logging with Serilog + Seq
- Metrics using OpenTelemetry & Prometheus
- Grafana dashboards and alert rules configured

## Why this project exists
This project is part of a structured learning journey to move from
"working code" to "well-architected systems" and job-ready backend skills. It priorites clarity over speed and understanding.

## Notes
This repository is intentionally built step-by-step with clarity over speed and learning process. So expect some bugs, refactors, smells, and etc.

## Status
**Completed (v2.0.0)** — This project is architecturally complete and maintained for reference.

