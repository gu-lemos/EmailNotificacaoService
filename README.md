# Email Notificacao Service

Serviço Windows para envio automático de emails agendados mensalmente. O serviço verifica diariamente os agendamentos cadastrados no banco de dados e envia os emails configurados no dia especificado de cada mês.

## Arquitetura

O projeto segue os princípios de **Clean Architecture** e **SOLID**:

```
EmailNotificacaoService/
├── Application/                    # Camada de Aplicação
│   └── Services/
│       ├── AgendamentoEmailService.cs
│       └── EmailService.cs
├── Domain/                         # Camada de Domínio
│   ├── Interfaces/
│   │   ├── IAgendamentoEmailService.cs
│   │   ├── IAgendamentoRepository.cs
│   │   └── IEmailService.cs
│   ├── Models/
│   │   └── AgendamentoEmail.cs
│   └── Validators/
│       ├── AgendamentoEmailValidator.cs
│       └── EmailValidator.cs
├── Infrastructure/                 # Camada de Infraestrutura
│   ├── Data/
│   │   └── AppDataContext.cs
│   └── Repositories/
│       └── AgendamentoRepository.cs
├── Program.cs                      # Configuração e DI
└── Worker.cs                       # Background Service
```

## Tecnologias

- **.NET 10** - Framework
- **Entity Framework Core** - ORM
- **SQLite** - Banco de dados
- **Windows Service** - Execução em background

## Configuração

### 1. Configurar credenciais SMTP

Copie o arquivo `appsettings.json` para `appsettings.Development.json` e configure suas credenciais:

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=C:\\ProgramData\\EmailNotificacaoService\\notificacao.db"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "EnableSsl": "true",
    "Username": "seu-email@gmail.com",
    "Password": "sua-senha-de-aplicativo",
    "FromAddress": "seu-email@gmail.com"
  }
}
```

> **Importante:** Para Gmail, você precisa gerar uma [senha de aplicativo](https://myaccount.google.com/apppasswords).

### 2. Criar o banco de dados

O banco SQLite será criado automaticamente em `C:\ProgramData\EmailNotificacaoService\notificacao.db`.

Estrutura da tabela `agendamento_email`:

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| id | INTEGER | Chave primária |
| email_destino | TEXT | Email do destinatário |
| assunto | TEXT | Assunto do email |
| mensagem | TEXT | Corpo do email |
| dia_do_mes | INTEGER | Dia do mês para envio (1-31) |
| ultimo_envio | DATETIME | Data do último envio |

## Execução

### Modo Desenvolvimento

```bash
dotnet run
```

### Instalação como Serviço Windows

1. **Publicar a aplicação:**
```bash
dotnet publish -c Release -o C:\ProgramData\EmailNotificacaoService\App
```

2. **Instalar o serviço (PowerShell como Administrador):**
```powershell
sc.exe create "EmailNotificacaoService" binPath="C:\ProgramData\EmailNotificacaoService\App\EmailNotificacaoService.exe" start=auto DisplayName="Email Notificacao Service"

sc.exe description "EmailNotificacaoService" "Servico de envio automatico de emails agendados mensalmente"

sc.exe start "EmailNotificacaoService"
```

3. **Gerenciar o serviço:**
```powershell
# Verificar status
sc.exe query EmailNotificacaoService

# Parar serviço
sc.exe stop EmailNotificacaoService

# Iniciar serviço
sc.exe start EmailNotificacaoService

# Remover serviço
sc.exe delete EmailNotificacaoService
```

## Funcionamento

1. O Worker executa a cada 24 horas
2. Busca agendamentos onde `dia_do_mes` = dia atual
3. Filtra agendamentos que ainda não foram enviados no mês atual
4. Valida os dados do agendamento
5. Envia o email via SMTP
6. Atualiza a coluna `ultimo_envio` com a data/hora atual

## Validações

O serviço valida os dados antes de enviar:

- **Email:** Formato válido, máximo 254 caracteres
- **Assunto:** Não vazio, máximo 200 caracteres
- **Mensagem:** Não vazia, máximo 5000 caracteres
- **Dia do mês:** Entre 1 e 31

## Logs

Os logs são exibidos no console durante desenvolvimento e no Event Viewer do Windows quando executado como serviço.

Níveis de log configuráveis no `appsettings.json`:
- `Debug` - Detalhado (desenvolvimento)
- `Information` - Informações gerais
- `Warning` - Alertas
- `Error` - Erros

## Segurança

- Nunca commite o arquivo `appsettings.Development.json`
- Use senhas de aplicativo ao invés de senhas reais
- O `.gitignore` já está configurado para ignorar arquivos sensíveis
