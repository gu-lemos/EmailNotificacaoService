using EmailNotificacaoService.Domain.Models;

namespace EmailNotificacaoService.Domain.Validators
{
    public static class AgendamentoEmailValidator
    {
        public static ValidationResult Validar(AgendamentoEmail agendamento)
        {
            var result = new ValidationResult();

            if (agendamento == null)
            {
                result.AddError("O agendamento não pode ser nulo");
                return result;
            }

            var (IsValid, ErrorMessage) = EmailValidator.Validar(agendamento.EmailDestino);

            if (!IsValid)
            {
                result.AddError($"Email de destino inválido: {ErrorMessage}");
            }

            if (string.IsNullOrWhiteSpace(agendamento.Assunto))
            {
                result.AddError("O assunto não pode ser vazio");
            }
            else if (agendamento.Assunto.Length > 200)
            {
                result.AddError("O assunto não pode ter mais de 200 caracteres");
            }

            if (string.IsNullOrWhiteSpace(agendamento.Mensagem))
            {
                result.AddError("A mensagem não pode ser vazia");
            }
            else if (agendamento.Mensagem.Length > 5000)
            {
                result.AddError("A mensagem não pode ter mais de 5000 caracteres");
            }

            if (agendamento.DiaDoMes < 1 || agendamento.DiaDoMes > 31)
            {
                result.AddError("O dia do mês deve estar entre 1 e 31");
            }

            return result;
        }

        public static bool EhValido(AgendamentoEmail agendamento)
        {
            return Validar(agendamento).IsValid;
        }
    }

    public class ValidationResult
    {
        private readonly List<string> _errors = [];

        public bool IsValid => _errors.Count == 0;
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public string GetErrorMessage()
        {
            return string.Join("; ", _errors);
        }
    }
}
