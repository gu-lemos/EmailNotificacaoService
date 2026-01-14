namespace EmailNotificacaoService.Domain.Interfaces
{
    public interface IEmailService
    {
        Task EnviarAsync(string para, string assunto, string mensagem, CancellationToken cancellationToken);
    }
}
