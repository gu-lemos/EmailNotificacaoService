using EmailNotificacaoService.Domain.Interfaces;
using System.Net;
using System.Net.Mail;

namespace EmailNotificacaoService.Application.Services
{
    public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task EnviarAsync(string para, string assunto, string mensagem, CancellationToken cancellationToken)
        {
            var smtpConfig = _configuration.GetSection("Smtp");

            using var smtp = new SmtpClient(smtpConfig["Host"])
            {
                Port = int.Parse(smtpConfig["Port"] ?? "587"),
                EnableSsl = bool.Parse(smtpConfig["EnableSsl"] ?? "true"),
                Credentials = new NetworkCredential(
                    smtpConfig["Username"],
                    smtpConfig["Password"]
                )
            };

            var fromAddress = smtpConfig["FromAddress"] ?? smtpConfig["Username"]
                ?? throw new InvalidOperationException("Configuração SMTP 'FromAddress' ou 'Username' não encontrada");

            using var mail = new MailMessage(
                fromAddress,
                para,
                assunto,
                mensagem
            );

            await smtp.SendMailAsync(mail, cancellationToken);

            _logger.LogInformation("Email enviado com sucesso para {Email} com assunto: {Assunto}", para, assunto);
        }
    }
}
