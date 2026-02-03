using Microsoft.Extensions.Options;
using StarkChallenge.Config;
using StarkChallenge.Interfaces.IProcess;
using StarkChallenge.Interfaces.IServices;
using StarkChallenge.Services;

namespace StarkChallenge.Main
{
    public class SchedulerProcess : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConfigScheduler _configScheduler;

        public SchedulerProcess(
            IServiceScopeFactory scopeFactory,
            IOptions<ConfigScheduler> options
        )
        {
            _scopeFactory = scopeFactory;
            _configScheduler = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Scheduler iniciado");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var clientsService = scope.ServiceProvider
                        .GetRequiredService<IClientsService>();

                    var invoiceService = scope.ServiceProvider
                        .GetRequiredService<IInvoiceProcess>();

                    var clients = clientsService.GenerateRandomClients(
                        _configScheduler.MinClients,
                        _configScheduler.MaxClients
                    );

                    invoiceService.ProcessInvoices(clients);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro no scheduler:");
                    Console.WriteLine(ex);
                }

                await Task.Delay(
                    TimeSpan.FromHours(_configScheduler.IntervalHours),
                    stoppingToken
                );
            }
        }

    }
}