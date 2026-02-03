using StarkChallenge.Config;
using StarkChallenge.Interfaces;
using StarkChallenge.Interfaces.IProcess;
using StarkChallenge.Interfaces.IRepositorys;
using StarkChallenge.Interfaces.IServices;
using StarkChallenge.Main;
using StarkChallenge.Models;
using StarkChallenge.Process;
using StarkChallenge.Repositorys;
using StarkChallenge.Services;
using StarkChallenge.Utils;
using System.Globalization;


namespace StarkChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            var builder = WebApplication.CreateBuilder(args);

            // Scheduler
            builder.Services.Configure<ConfigScheduler>(builder.Configuration.GetSection("ProgramConfig"));

            // Add services to the container.
            builder.Services.AddControllers();

            //Services 
            //Singleton
            builder.Services.AddSingleton<IClientsService, ClientsService>();
            builder.Services.AddSingleton<IInvoiceService, InvoiceService>();
            builder.Services.AddSingleton<IInvoiceProcess, InvoiceProcessor>();
            //Scoped
            builder.Services.AddScoped<ITransferService, TransferService>();
            builder.Services.AddScoped<IInvoiceJsonStore, InvoiceJsonStore>();
            builder.Services.AddScoped<IInvoiceIDRepository, InvoiceIDRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Enviroment
            var starkConfig = builder.Configuration.GetSection("StarkBank").Get<ConfigEnviroment>() ?? new ConfigEnviroment();
            var project = new StarkBank.Project(
                environment: starkConfig.Enviroment,
                id: starkConfig.ProjectId,
                privateKey: starkConfig.PrivateKey
            );

            StarkBank.Settings.User = project;

            builder.Services.AddHostedService<SchedulerProcess>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
