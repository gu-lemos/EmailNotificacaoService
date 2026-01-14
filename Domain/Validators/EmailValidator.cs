using System.ComponentModel.DataAnnotations;

namespace EmailNotificacaoService.Domain.Validators
{
    public static class EmailValidator
    {
        public static bool EhValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        public static (bool IsValid, string ErrorMessage) Validar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "O endereço de email não pode ser vazio");

            if (email.Length > 254)
                return (false, "O endereço de email não pode ter mais de 254 caracteres");

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
                return (false, "O formato do endereço de email é inválido");

            return (true, string.Empty);
        }
    }
}
