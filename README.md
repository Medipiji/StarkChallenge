# StarkChallenge

Description
-----------
ASP.NET Core application demonstrating invoice processing and batch transfer execution integrated with the StarkBank SDK. The project includes services, an invoice ID persistence repository, a processing pipeline, and a configurable scheduler.

Key features
------------
- Invoice processing pipeline (`Process/InvoiceProcess.cs`).
- Client service and test-data generation (`Services/ClientsService.cs`).
- Transfer service integrated with StarkBank (`Services/TransferService.cs`).
- Repository for tracking processed invoice IDs (`Repositorys/InvoiceIDRepository.cs`).
- Configurable scheduler using `ProgramConfig` (controls batch size and run interval).

Tech stack
----------
- Platform: .NET 8 (ASP.NET Core)
- Database (optional): MongoDB (`MongoDB.Driver`)
- Libraries: `StarkBank` SDK, `Swashbuckle.AspNetCore` (Swagger), `Bogus` (fake data)
- Testing: xUnit (project `StarkChallenge.Tests`)

Prerequisites
-------------
- .NET 8 SDK
- (Optional) MongoDB instance if using persistent storage
- StarkBank credentials (`ProjectId` and `PrivateKey`) to perform real transfers

Configuration
-------------
- Main config files: `StarkChallenge/appsettings.json` and `StarkChallenge/appsettings.Development.json`.
- Example invoice input: `StarkChallenge/invoiceJson.json`.
- If using `appsettings.Development.json`, do not commit sensitive credentials to the repository — use environment variables or a secret store.

Relevant `appsettings.json` sections:
- `StarkBank`: `Enviroment`, `ProjectId`, `PrivateKey`, `TransferTarget` (destination account details).
- `ProgramConfig`: `MinClients`, `MaxCLients`, `IntervalHours` (scheduler settings).
- `ConnectionStrings:MongoDb`: MongoDB connection string.

How to run (locally)
---------------------
1. Restore packages:

```bash
dotnet restore
```

2. Build:

```bash
dotnet build
```

3. Run:

```bash
dotnet run --project StarkChallenge
```

4. Tests:

```bash
dotnet test
```

API and documentation
---------------------
- The project registers Swagger (Swashbuckle). After starting the application, open `/swagger` to explore the API endpoints.
- A sample request file is located at `StarkChallenge/StarkChallenge.http`.

Security and best practices
--------------------------
- Never commit private keys or secrets in `appsettings.*.json`. Use environment variables or a secret manager.
- Validate StarkBank environment and credentials before performing real transfers; prefer `sandbox` for testing.
- Ensure idempotency in transfer operations to avoid duplicates.

Project structure (summary)
---------------------------
- `Controllers/` - HTTP endpoints (e.g. `WebhookController`).
- `Services/` - business logic.
- `Process/` - invoice processing logic.
- `Repositorys/` - data persistence.
- `Utils/` - utilities and stores.

Contribution
------------
- Open an issue before implementing major features.
- Create pull requests per feature branch with clear descriptions and tests where applicable.

Scheduler (detailed)
--------------------
- Configuration: managed by `ConfigScheduler` (`Config/ConfigScheduler.cs`) with `MinClients`, `MaxClients`, and `IntervalHours` properties.
- Runs as a `BackgroundService` in `Schedulers/SchedulerProcess.cs`. The service loops while the application is running.
- Per-cycle flow:
	- Creates a DI scope (`IServiceScopeFactory`) to resolve services with correct lifetimes.
	- Uses `IClientsService.GenerateRandomClients(MinClients, MaxClients)` to generate a batch of clients.
	- Calls `IInvoiceProcess.ProcessInvoices(clients)`, which delegates to `IInvoiceService.CreateInvoices` to create invoices via the StarkBank SDK.
	- Waits `IntervalHours` hours before the next cycle (`Task.Delay(TimeSpan.FromHours(...))`).
- Practical impact: automates periodic batch invoice creation; useful for load simulation, recurring billing, or continuous processing.

Webhook (route and behavior)
---------------------------
- Exposed route: `POST /Webhook/starkbank/invoiceReceptor` (controller `WebhookController`, attribute `[Route("[controller]/starkbank")]`).
- Input: a `ResponseInvoiceDTO` in the request body (the webhook payload from StarkBank).
- Logic:
	- The controller calls `await _transferService.ValidateTransferProcess(payload)`.
	- In `TransferService.ValidateTransferProcess`:
		- Checks whether `Event.Log.Type == "credited"` (invoice credited).
		- Checks `IInvoiceIDRepository.AlreadyExists(invoiceId)` for idempotency to avoid processing the same event twice.
		- If a new credit, creates a `Transfer` using `TransferTarget` from `appsettings` and calls `Transfer.Create(...)` (StarkBank SDK).
		- Records the invoice ID with `IInvoiceIDRepository.RecordID(...)` after creating the transfer.
- Practical effect: when StarkBank notifies that an invoice was credited, the webhook validates the event and triggers a transfer automatically, with duplicate protection.

Recommendations for improvements
--------------------------------
- Validate webhook signatures: confirm event origin by verifying headers/signature (there is an `EllipticCurve` reference in the controller, but no verification logic). Implementing signature verification prevents processing forged payloads.
- Logging and metrics: add structured logs and success/failure counters to monitor the scheduler and transfers.
- Retries and resilience: wrap `Invoice.Create` and `Transfer.Create` calls with retry/circuit-breaker policies to handle transient external API failures.
- Persistence and concurrency: ensure `IInvoiceIDRepository` is durable and race-condition safe (e.g. unique indexes in the DB to prevent duplicates).



