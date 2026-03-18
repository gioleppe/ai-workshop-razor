
# SPECS

## Architecture & Technology Stack

- Framework: .NET 10 (ASP.NET Core)
- Project type: Single ASP.NET Core project hosting both a Web API (Controllers) for agent access and Razor Pages for the human UI. Razor Pages is recommended for a lightweight, server-rendered UI.
- Data store: In-memory EF Core provider (Microsoft.EntityFrameworkCore.InMemory) for demo purposes, easy to swap to SQL Server/Postgres later.
- API docs: Use the built-in OpenAPI/Swagger tooling to generate an OpenAPI 3.0 spec from XML comments.

## Domain Model

Core entity: `FamousPerson` with the following properties:

- `Id` (Guid): unique identifier.
- `FullName` (string): required; first and last name.
- `Background` (string): brief biography or context.
- `CompetenceField` (string): domain (e.g., "Physics", "Literature").

Document DTOs and controller actions with XML `<summary>` comments so OpenAPI descriptions are useful for the AI agent.

## API Specifications (Agent Interface)

Base route: `/api/famouspeople`

- GET `/api/famouspeople`
	- Description: Returns the list of famous people.
	- Agent use case: Check existing entries before adding.
	- Response: `200 OK` — array of `FamousPerson` objects.

- POST `/api/famouspeople`
	- Description: Add a new famous person.
	- Agent use case: Create a requested figure.
	- Payload example:

```json
{ "fullName": "...", "background": "...", "competenceField": "..." }
```

	- Response: `201 Created` — the created object (with `Id`).

- PUT `/api/famouspeople/{id}`
	- Description: Update an existing person.
	- Agent use case: Correct or enrich records.
	- Payload example:

```json
{ "id": "...", "fullName": "...", "background": "...", "competenceField": "..." }
```

	- Response: `204 No Content` on success, `404 Not Found` if the Id doesn't exist.

Crucial requirement: Add XML `<summary>` comments to every endpoint and DTO property so the generated OpenAPI descriptions serve as the agent's prompt/instructions.

## User Interface (Human Interface)

- Route: `/` (root URL)
- Technology: Razor Pages (server-rendered)
- Design: Simple HTML/CSS (optionally use Bootstrap or Tailwind via CDN).
- Data access: Razor Pages should read from the shared `IFamousPersonService` directly (no internal HTTP calls).
- Presentation: Grid of cards or a table showing `FullName`, `CompetenceField`, and `Background`.
- Optional demo: Client-side auto-refresh (e.g., `setInterval`) to observe agent-driven updates in real time.

## Extensibility & Best Practices

- Dependency injection: Define `IFamousPersonService` and register a single implementation used by both controllers and Razor Pages.
- Validation: Use data annotations (e.g., `[Required]`, `[MaxLength]`) on DTOs to leverage automatic `400 Bad Request` behavior for invalid input.
- Keep controllers and services small and well-documented so OpenAPI remains informative for agents.

---

Would you like me to implement the `Program.cs` setup, controller, DTOs, and `IFamousPersonService` implementation to get this running? If so, I can scaffold the code next.

