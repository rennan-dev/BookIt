
# BookIt - (em desenvolvimento)

O BookIt é um sistema de gerenciamento de reservas desenvolvido para a UFAM, permitindo a reserva de salas de reunião, auditórios e veículos de forma prática e eficiente. Com três tipos de usuários (Administrador, Técnico e Docente), o sistema oferece controle total sobre as reservas, cadastros e status dos recursos disponíveis.

## Autores

- [@rennan-dev](https://github.com/rennan-dev)
- [@arsouza81](https://github.com/arsouza81)
- [@brucediandre](https://github.com/brucediandre)


## Funcionalidades

1. Administrador (Admin)
    - Aprovação de solicitações de reserva: O admin pode aceitar ou recusar solicitações de reserva feitas por técnicos e docentes.
    - Gerenciamento de cadastros: Aprova ou recusa pedidos de cadastro de novos usuários no sistema.
    - Gestão de recursos: Altera o status de veículos e salas (ex: quebrado, em manutenção, disponível, etc.).
2. Técnico e Docente
    - Solicitação de reserva: Podem solicitar a reserva de salas de reunião, auditórios ou veículos, informando data e horário de início e fim.
    - Cadastro no sistema: Podem se cadastrar no sistema, mas só terão acesso após aprovação do admin.
    - Cancelamento de reserva: Podem cancelar reservas já feitas em seu nome.
3. Cadastro e Login
    - Admin: Já vem cadastrado diretamente no sistema.
    - Usuários comuns (Técnicos e Docentes): Podem se cadastrar fornecendo as seguintes informações:
        - SIAPE
        - CPF 
        - Nome
        - Email
        - Celular
        - Senha

    - Aprovação de cadastro: O acesso ao sistema só é liberado após aprovação do admin.


## Como Usar
1. Admin:
    - Faça login com suas credenciais.
    - Aprove ou recuse solicitações de reserva e cadastros de usuários.
    - Gerencie o status dos recursos (salas, auditórios e veículos).

2. Técnico e Docente:
    - Faça o cadastro no sistema e aguarde a aprovação do admin.
    - Após aprovado, faça login e solicite reservas de recursos.
    - Gerencie suas reservas, cancelando-as se necessário.

## Como Rodar a Aplicação  

### Pré-requisitos  

Antes de começar, certifique-se de ter instalado:  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [MySQL](https://dev.mysql.com/downloads/) 

no codigo_fonte/BookItApi/appsettings.json coloque o seu user e senha do MySQL, depois entre no terminal dentro da pasta do projeto BookIt, utilize esse comando para atualizar o database:

``dotnet ef database update``

Posteriormente utilize para rodar o programa:

``dotnet run``

Depois acesse essa URL para acessar a API da aplicação:

``http://localhost:5092/swagger/index.html``



## Tecnologias Utilizadas
- **Frontend:** React
- **Backend:** C#
- **Banco de Dados:** MySQL
- **Mensageria:** RabbitMQ
- **Autenticação:** JWT (JSON Web Tokens)
- **Containerização:** Docker
