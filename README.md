
# BookIt - (Teste)

Para utilizar o painel do administrador utilize essas credenciais:

```CPF: 123.456.789=00```

```Senha: Aaaa123!```

Utilize essa branch para testar a funcionalidade do BookIt sem a necessidade de cadastrar um email para o envio de mensagens para testar o sistema com todas as funcionalidades utilize a main.

O BookIt é um sistema de gerenciamento de reservas desenvolvido para a UFAM, permitindo a reserva de salas de reunião, auditórios e veículos de forma prática e eficiente. Com três tipos de usuários (Administrador, Técnico e Docente), o sistema oferece controle total sobre as reservas, cadastros e status dos recursos disponíveis.

## Autores

- [@rennan-dev](https://github.com/rennan-dev)
- [@arsouza81](https://github.com/arsouza81)
- [@brucediandre](https://github.com/brucediandre)


## Funcionalidades

1. Administrador (Admin)
    - Aprovação de solicitações de reserva: O admin pode aceitar ou recusar solicitações de reserva feitas por técnicos e docentes.
    - Gerenciamento de cadastros: Aprova ou recusa pedidos de cadastro de novos usuários no sistema.
    - Gestão de recursos(em desenvolvimento): Altera o status de veículos e salas (ex: quebrado, em manutenção, disponível, etc.).
2. Técnico e Docente
    - Realizar reservas: Podem realizar reservas de salas de reunião, auditórios ou veículos, informando data e horário.
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

# Como Rodar a Aplicação  

### Pré-requisitos  

Antes de começar, certifique-se de ter instalado e que esteja em execução:  
- [Docker](https://www.docker.com/get-started)  
- [Docker Compose](https://docs.docker.com/compose/install/)  

### Passos para Executar a Aplicação com Docker  

1. **Configuração de Envio de E-mail**  
   No arquivo `codigo_fonte/BookItApi/appsettings.json`, preencha as informações do seu e-mail para envio de mensagens. O arquivo deve se parecer com o seguinte:

   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "ConnectionStrings": {
       "BookItApi": "server=bookit-mysql; database=bookit; user=root; password=root"
     },
     "Smtp": {
       "Host": "smtp.gmail.com",
       "Port": 587,
       "Username": "<seu_email>@gmail.com",
       "Password": "<seu_codigo_de_app>",
       "From": "<seu_email>@gmail.com"
     }
   }


# Instruções para Execução do Projeto

## Subir os Containers com Docker Compose

Entre no diretório `codigo_fonte` no terminal e execute o seguinte comando para construir e iniciar os containers:

```
docker compose up --build
```

# Parar os Containers com Docker Compose
## Quando desejar parar os containers em execução, utilize o comando:
```
docker compose down
```

## Acessando a Aplicação
### Backend
```
http://localhost:5092/swagger/index.html
```

### Frontend
```
http://localhost:3000/login
```

## Tecnologias Utilizadas
- **Frontend:** React
- **Backend:** C#
- **Banco de Dados:** MySQL
- **Autenticação:** JWT (JSON Web Tokens)
- **Envio de E-mail:** SMTP (Gmail)
- **Containerização:** Docker
