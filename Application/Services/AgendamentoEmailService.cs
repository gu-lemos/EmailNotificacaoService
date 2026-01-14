using EmailNotificacaoService.Domain.Interfaces;
using EmailNotificacaoService.Domain.Validators;

namespace EmailNotificacaoService.Application.Services
{
    public class AgendamentoEmailService(
        IAgendamentoRepository agendamentoRepository,
        IEmailService emailService,
        ILogger<AgendamentoEmailService> logger
        ) : IAgendamentoEmailService
    {
        private readonly IAgendamentoRepository _agendamentoRepository = agendamentoRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<AgendamentoEmailService> _logger = logger;

        public async Task ProcessarEnviosDiaAtualAsync(CancellationToken cancellationToken)
        {
            var diaAtual = DateTime.Today.Day;

            _logger.LogInformation("Iniciando processamento de envios para o dia {Dia}", diaAtual);

            var agendamentos = await _agendamentoRepository.ObterAgendamentosParaHoje(cancellationToken);

            if (agendamentos is null || agendamentos.Count == 0)
            {
                _logger.LogInformation("Nenhum agendamento encontrado para hoje");
                return;
            }

            _logger.LogInformation("Encontrados {Quantidade} agendamento(s) para processar", agendamentos.Count);

            foreach (var agendamento in agendamentos)
            {
                try
                {
                    var validationResult = AgendamentoEmailValidator.Validar(agendamento);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogWarning(
                            "Agendamento ID {Id} inválido. Erros: {Erros}",
                            agendamento.Id,
                            validationResult.GetErrorMessage());
                        continue;
                    }

                    _logger.LogInformation("Enviando email para {Email}", agendamento.EmailDestino);

                    await _emailService.EnviarAsync(
                        agendamento.EmailDestino,
                        agendamento.Assunto,
                        agendamento.Mensagem,
                        cancellationToken);

                    await _agendamentoRepository.AtualizarUltimoEnvio(
                        agendamento.Id,
                        DateTime.Now,
                        cancellationToken);

                    _logger.LogInformation("Email enviado com sucesso para {Email}", agendamento.EmailDestino);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao enviar email para {Email}", agendamento.EmailDestino);
                }
            }

            _logger.LogInformation("Processamento de envios concluído");
        }
    }
}
