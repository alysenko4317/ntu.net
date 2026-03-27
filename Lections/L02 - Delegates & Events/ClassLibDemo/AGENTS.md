# AGENTS.md

## Scope and intent
- This solution is a delegates/events teaching demo with three projects: a reusable solver library, a console consumer, and a WinForms consumer.
- Keep changes small and explicit; code is intentionally simple for learning event flow.

## Architecture map
- `MathSolverClassLibrary/MathSolver.cs` is the core boundary.
- `MathSolverClassLibrary.MathSolver` owns configuration (`_xmin`, `_xmax`, `_step`) and exposes `Solve(Func<double,double> fn)`.
- `Solve` triggers events in strict order: `InitializeEvent` -> `ProcessingStartedEvent` -> numeric loop -> `ProcessingFinishedEvent`.
- `MathSolverConsole/Program.cs` demonstrates lightweight subscription with lambdas and writes event data to stdout.
- `EventsFormsApp/Program.cs` demonstrates UI-oriented consumption: event callbacks append HTML fragments, then an `IMessageSender` renders with `WebBrowser` in a modal form.

## Data and control flow details
- Computation is a simple accumulation loop (`for (double x = _xmin; x <= _xmax; x += _step)`), so floating-point endpoint behavior is intentional and visible.
- Event payload types are custom `EventArgs` classes in the same file: `MathSolverInitializeEventArgs`, `MathSolverProcessingFinishedEventArgs`.
- There is no async/threading layer; all callbacks run synchronously in the caller thread.

## Project references and integration points
- `MathSolverConsole/MathSolverConsole.csproj` -> references `MathSolverClassLibrary`.
- `EventsFormsApp/EventsFormsApp.csproj` -> references `MathSolverClassLibrary` and uses Windows Forms (`<UseWindowsForms>true</UseWindowsForms>`).
- No external NuGet packages are used; only .NET SDK and framework libraries.

## Local workflow (verified)
- Build full solution: `dotnet build DelegatesDemo.sln`.
- Run console demo: `dotnet run --project MathSolverConsole/MathSolverConsole.csproj`.
- Run WinForms demo (Windows only): `dotnet run --project EventsFormsApp/EventsFormsApp.csproj`.
- Current TFMs are `net7.0` and `net7.0-windows`; SDK 9 emits `NETSDK1138` end-of-support warnings during build/run.

## Conventions specific to this repo
- Keep event names and sequencing consistent with existing API (`InitializeEvent`, `ProcessingStartedEvent`, `ProcessingFinishedEvent`).
- Existing public event-arg properties are lowercase (`xmin`, `xmax`, `step`, `result`); preserve for compatibility unless intentionally refactoring all consumers.
- Use lambda subscriptions in app entry points as shown in both `Program.cs` files.
- Favor direct file-local demo types in `EventsFormsApp/Program.cs` (e.g., `IMessage`, `HtmlMessage`, `IMessageSender`) rather than introducing extra layers.

## AI-instruction sources discovered
- Searched for `AGENTS.md`, `AGENT.md`, `CLAUDE.md`, Copilot/Cursor/Windsurf/Cline rules, and `README.md`; none were present before this file.

