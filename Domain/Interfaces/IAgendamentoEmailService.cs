namespace EmailNotificacaoService.Domain.Interfaces
{
    public interface IAgendamentoEmailService
    {
        Task ProcessarEnviosDiaAtualAsync(CancellationToken cancellationToken);
    }
}
