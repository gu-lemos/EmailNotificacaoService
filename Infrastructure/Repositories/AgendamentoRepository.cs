using EmailNotificacaoService.Domain.Interfaces;
using EmailNotificacaoService.Domain.Models;
using EmailNotificacaoService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailNotificacaoService.Infrastructure.Repositories
{
    public class AgendamentoRepository(AppDataContext context) : IAgendamentoRepository
    {
        private readonly AppDataContext _context = context;

        public async Task AtualizarUltimoEnvio(int agendamentoId, DateTime dataEnvio, CancellationToken cancellationToken)
        {
            var agendamento = await _context.Agendamentos
                .FirstOrDefaultAsync(a => a.Id == agendamentoId, cancellationToken);

            if (agendamento is not null)
            {
                agendamento.UltimoEnvio = dataEnvio;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<AgendamentoEmail>> ObterAgendamentosParaHoje(CancellationToken cancellationToken)
        {
            var hoje = DateTime.Today;
            var mesAtual = hoje.Month;
            var anoAtual = hoje.Year;

            var agendamentos = await _context.Agendamentos
                .Where(agendamento =>
                    agendamento.DiaDoMes == hoje.Day &&
                    (!agendamento.UltimoEnvio.HasValue))
                .ToListAsync(cancellationToken);

            return agendamentos;
        }
    }
}
