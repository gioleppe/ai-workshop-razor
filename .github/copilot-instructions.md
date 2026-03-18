# Copilot Instructions for AI-Workshop-App

## Build and run commands

- Build the app with `dotnet build`.
- Run the app with `dotnet run`.
- Use the launch profiles in `Properties/launchSettings.json` when you need predictable local URLs:
  - `dotnet run --launch-profile http`
  - `dotnet run --launch-profile https`
- Run tests with `dotnet test`. At the moment there is no separate test project in the repository, so this currently behaves as a validation/build step rather than executing named test cases.

## High-level architecture

- This repository currently contains a single ASP.NET Core web app targeting `.NET 10` (`AI-Workshop-App.csproj`).
- The live implementation is still the minimal starter app: all composition is in `Program.cs`, the app builds a `WebApplication`, and the only mapped endpoint is `GET /`, which returns `"Hello World!"`.
- Runtime configuration is split between `appsettings.json`, `appsettings.Development.json`, and the local launch profiles in `Properties/launchSettings.json`.
- `SPECS.md` is important context for future work. It describes the intended shape of the app even though that design is not implemented yet:
  - keep this as a single ASP.NET Core project
  - expose agent-facing REST endpoints under `/api/famouspeople`
  - provide a human-facing root page at `/`
  - share the same data through an `IFamousPersonService` used by both the API and the UI
  - use an in-memory data store for the demo

## Key conventions in this repository

- Treat `SPECS.md` as the product/design source of truth for planned features, but do not assume those controllers, pages, services, or entities already exist. Check `Program.cs` and the project tree before editing.
- Keep the architecture as a single web project unless the user explicitly asks for a larger restructure. The spec favors shared in-process services over internal HTTP calls between app layers.
- If you implement the planned API from `SPECS.md`, keep agent-facing routes under `/api` and preserve descriptive XML `<summary>` comments on endpoints and DTO properties so generated OpenAPI descriptions stay useful.
- If you implement the planned UI from `SPECS.md`, prefer reading data from the shared service directly instead of calling the app's own HTTP endpoints from the server-rendered UI.
- Local development URLs are already standardized in `Properties/launchSettings.json`: HTTP uses `http://localhost:5000`, and the HTTPS profile also exposes `https://localhost:7197`.
