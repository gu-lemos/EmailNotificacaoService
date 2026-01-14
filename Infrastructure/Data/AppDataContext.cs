using EmailNotificacaoService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailNotificacaoService.Infrastructure.Data
{
    public class AppDataContext(DbContextOptions<AppDataContext> options) : DbContext(options)
    {
        public DbSet<AgendamentoEmail> Agendamentos => Set<AgendamentoEmail>();
    }
}
