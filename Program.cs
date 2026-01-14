using EmailNotificacaoService;
using EmailNotificacaoService.Application.Services;
using EmailNotificacaoService.Domain.Interfaces;
using EmailNotificacaoService.Infrastructure.Data;
using EmailNotificacaoService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) =>
    {
        // Data Layer
        services.AddDbContext<AppDataContext>(options =>
            options.UseSqlite(
                context.Configuration.GetConnectionString("Default")));

        // Repository Layer
        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

        // Service Layer
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAgendamentoEmailService, AgendamentoEmailService>();

        // Hosted Service
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();
