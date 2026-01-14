using EmailNotificacaoService.Domain.Interfaces;

namespace EmailNotificacaoService;

public class Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker iniciado em: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessarEnviosAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar envios de email");
            }

            _logger.LogInformation("Aguardando 24 horas até a próxima execução...");
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }

        _logger.LogInformation("Worker finalizado em: {Time}", DateTimeOffset.Now);
    }

    private async Task ProcessarEnviosAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var agendamentoService = scope.ServiceProvider
            .GetRequiredService<IAgendamentoEmailService>();

        await agendamentoService.ProcessarEnviosDiaAtualAsync(cancellationToken);
    }
}
