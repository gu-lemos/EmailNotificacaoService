using EmailNotificacaoService.Domain.Models;

namespace EmailNotificacaoService.Domain.Interfaces
{
    public interface IAgendamentoRepository
    {
        Task<List<AgendamentoEmail>> ObterAgendamentosParaHoje(CancellationToken cancellationToken);
        Task AtualizarUltimoEnvio(int agendamentoId, DateTime dataEnvio, CancellationToken cancellationToken);
    }
}
