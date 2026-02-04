# StarkChallenge

Descrição
-------
Aplicação ASP.NET Core para demonstração de processamento de faturas e execução de transferências em lote com integração ao StarkBank (SDK). O projeto contém serviços, repositório de persistência de IDs de fatura, um pipeline de processamento e um agendador configurável.

Principais funcionalidades
-------------------------
- Pipeline de processamento de faturas (`Process/InvoiceProcess.cs`).
- Serviço de clientes e geração de dados (`Services/ClientsService.cs`).
- Serviço de transferência com integração ao StarkBank (`Services/TransferService.cs`).
- Repositório para controle de IDs processados (`Repositorys/InvoiceIDRepository.cs`).
- Agendador configurável via `ProgramConfig` (intervalos e tamanho do lote).

Stack tecnológica
-----------------
- Plataforma: .NET 8 (ASP.NET Core)
- Banco (opcional): MongoDB (`MongoDB.Driver`)
- Bibliotecas: `StarkBank` SDK, `Swashbuckle.AspNetCore` (Swagger), `Bogus` (dados fictícios)
- Testes: xUnit (projeto `StarkChallenge.Tests`)

Pré-requisitos
--------------
- .NET 8 SDK
- (Opcional) Instância MongoDB se for utilizar persistência real
- Credenciais StarkBank (ProjectId e PrivateKey) para executar transferências reais

Configuração
-------------
- Arquivos principais: `StarkChallenge/appsettings.json` e `StarkChallenge/appsettings.Development.json`.
- Exemplo de entrada de faturas: `StarkChallenge/invoiceJson.json`.
- Se estiver usando `appsettings.Development.json`, não comite credenciais sensíveis no repositório — use variáveis de ambiente ou um secret store.

Seções relevantes em `appsettings.json`:
- `StarkBank`: `Enviroment`, `ProjectId`, `PrivateKey`, `TransferTarget` (dados da conta destino).
- `ProgramConfig`: `MinClients`, `MaxCLients`, `IntervalHours` (configuração do agendador).
- `ConnectionStrings:MongoDb`: string de conexão do MongoDB.

Como executar (localmente)
-------------------------
1. Restaurar pacotes:

```bash
dotnet restore
```

2. Build:

```bash
dotnet build
```

3. Executar:

```bash
dotnet run --project StarkChallenge
```

4. Testes:

```bash
dotnet test
```

API e documentação
-------------------
- O projeto registra o Swagger (Swashbuckle). Após iniciar a aplicação, abra `/swagger` para explorar os endpoints.
- Exemplo de requisição está em `StarkChallenge/StarkChallenge.http`.

Segurança e boas práticas
------------------------
- Nunca comite chaves privadas ou segredos em `appsettings.*.json`. Utilize variáveis de ambiente ou secret stores.
- Valide ambientes e credenciais StarkBank antes de executar transferências reais; recomenda-se executar primeiro em `sandbox`.
- Garanta idempotência nas operações de transferência para evitar duplicidade.

Estrutura resumida do projeto
----------------------------
- `Controllers/` - endpoints HTTP (ex.: `WebhookController`).
- `Services/` - lógica de negócio.
- `Process/` - processamento de faturas.
- `Repositorys/` - persistência de dados.
- `Utils/` - utilitários e stores.

Contribuição
------------
- Abra uma issue antes de implementar features maiores.
- Crie PRs por branch com descrição clara e testes quando aplicável.
 
Scheduler (detalhado)
--------------------
- Configuração: gerenciada por `ConfigScheduler` (`Config/ConfigScheduler.cs`) com as propriedades `MinClients`, `MaxClients` e `IntervalHours`.
- Executado como um `BackgroundService` em `Schedulers/SchedulerProcess.cs`. O serviço roda em loop enquanto a aplicação estiver ativa.
- Fluxo por ciclo:
	- Cria um escopo de DI (`IServiceScopeFactory`) para resolver serviços com o lifetime correto.
	- Usa `IClientsService.GenerateRandomClients(MinClients, MaxClients)` para gerar um lote de clientes.
	- Chama `IInvoiceProcess.ProcessInvoices(clients)`, que delega a `IInvoiceService.CreateInvoices` para criar invoices via SDK do StarkBank.
	- Aguarda `IntervalHours` horas antes do próximo ciclo (`Task.Delay(TimeSpan.FromHours(...))`).
- Impacto prático: automatiza a criação periódica de invoices em lote; útil para simular carga, gerar faturamentos periódicos ou acionar processamento contínuo.

Webhook (rota e comportamento)
-----------------------------
- Rota exposta: `POST /Webhook/starkbank/invoiceReceptor` (controller `WebhookController`, atributo `[Route("[controller]/starkbank")]`).
- Entrada: um `ResponseInvoiceDTO` no corpo da requisição (payload do webhook do StarkBank).
- Lógica:
	- O controller chama `await _transferService.ValidateTransferProcess(payload)`.
	- Em `TransferService.ValidateTransferProcess`:
		- Verifica se `Event.Log.Type == "credited"` (invoice creditada).
		- Consulta `IInvoiceIDRepository.AlreadyExists(invoiceId)` para garantir idempotência e evitar processar o mesmo evento duas vezes.
		- Se for um crédito novo, cria um `Transfer` usando os dados de `TransferTarget` em `appsettings` e chama `Transfer.Create(...)` (SDK StarkBank).
		- Registra o ID da invoice com `IInvoiceIDRepository.RecordID(...)` após criar a transferência.
- Efeito prático: quando o StarkBank notifica que uma invoice foi creditada, o webhook valida o evento e inicia a transferência automaticamente, com proteção contra duplicidade.



