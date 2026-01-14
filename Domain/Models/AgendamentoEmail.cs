using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailNotificacaoService.Domain.Models
{
    [Table("agendamento_email")]
    public class AgendamentoEmail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email_destino")]
        public string EmailDestino { get; set; } = "";

        [Column("assunto")]
        public string Assunto { get; set; } = "";

        [Column("mensagem")]
        public string Mensagem { get; set; } = "";

        [Column("dia_do_mes")]
        public int DiaDoMes { get; set; }

        [Column("ultimo_envio")]
        public DateTime? UltimoEnvio { get; set; }
    }
}
