using AutoMapper;
using BookItApi.Data;
using BookItApi.Dtos.User;
using BookItApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookItApi.Services;

/// <summary>
/// Serviço responsável pelas operações de autenticação e gerenciamento de administradores no sistema.
/// Inclui funcionalidades como cadastro, login, logout e etc.
/// </summary>
public class UserService {
    private IMapper _mapper;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private TokenService _tokenService;
    private UserDbContext _context;
    private readonly IEmailSender _emailSender;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserService"/>.
    /// </summary>
    /// <param name="mapper">Objeto de mapeamento entre DTOs e modelos de domínio.</param>
    /// <param name="userManager">Gerenciador de usuários do Identity para administração de contas de usuário.</param>
    /// <param name="signInManager">Gerenciador de login do Identity.</param>
    /// <param name="tokenService">Serviço responsável pela geração de tokens JWT.</param>
    /// <param name="context">Contexto do banco de dados específico para a administração de dados de usuários.</param>
    /// <param name="emailSender">Serviço de envio de email para admins e servidores.</param>
    public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService, UserDbContext context, IEmailSender emailSender) {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
        _emailSender = emailSender;
    }

    /// <summary>
    /// Cadastra um novo usuário no sistema, garantindo que SIAPE, e-mail e telefone sejam únicos
    /// </summary>
    /// <param name="userDto">Objeto com os dados do usuário a ser cadastrado.</param>
    /// <returns>Retorna um Task representando a operação de cadastro.</returns>
    /// <exception cref="ApplicationException">Lançada caso já exista um usuário com dados duplicados.</exception>
    public async Task CadastraUsuario(CreateUserDto userDto) {
        try {
            var userExistente = await _userManager.Users
                .Where(a => a.Siape == userDto.Siape || 
                            a.Email == userDto.Email || 
                            a.PhoneNumber == userDto.PhoneNumber)
                .FirstOrDefaultAsync();

            if(!userDto.Email.EndsWith("@ufam.edu.br")) {
                throw new ApplicationException("O Email precisa terminar com @ufam.edu.br");
            }

            if(userExistente != null) {
                if(userExistente.Siape == userDto.Siape) {
                    throw new ApplicationException("Já existe um usuário cadastrado com este SIAPE.");
                }
                if(userExistente.Cpf == userDto.Cpf) {
                    throw new ApplicationException("Já existe um usuário cadastrado com este CPF.");
                }
                if(userExistente.Email == userDto.Email) {
                    throw new ApplicationException("Já existe um usuário cadastrado com este e-mail.");
                }
                if(userExistente.PhoneNumber == userDto.PhoneNumber) {
                    throw new ApplicationException("Já existe um usuário cadastrado com este telefone.");
                }
            }

            User user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Cpf;

            IdentityResult resultado = await _userManager.CreateAsync(user, userDto.Password);

            if(!user.IsAdmin && user.Email!=null) {
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "BookIt - Cadastro Pendente",
                    $"Olá, senhor/senhora {user.Name}.\nSeu cadastro foi armazenado no sistema, aguarde a resposta de um administrador para acessar o sistema.\nNós retornaremos com a resposta.");
            
                var admins = await _userManager.Users.Where(u => u.IsAdmin && u.Email!=null).ToListAsync();
                foreach(var admin in admins) {
                    if(admin.Email!=null) {
                        await _emailSender.SendEmailAsync(
                            admin.Email,
                            "BookIt - Novo Cadastro Pendente",
                            $"Olá, {admin.Name}.\nUm novo usuário ({user.Name}, CPF: {userDto.Cpf}) solicitou cadastro no sistema. Acesse o painel administrativo para revisar a solicitação.");
                    }
                }
            }

            if(!resultado.Succeeded) {
                var errors = string.Join(", ", resultado.Errors.Select(e => e.Description));
                throw new ApplicationException($"Falha ao cadastrar Usuário: {errors}");
            }
        }catch(Exception ex) {
            Console.WriteLine($"Erro ao cadastrar o usuário: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw new ApplicationException($"{ex.Message}");
        }
    }

    /// <summary>
    /// Realiza o login de um usuário no sistema.
    /// </summary>
    /// <param name="userDto">Objeto com as credenciais do Usuário para o login.</param>
    /// <returns>Retorna o token JWT gerado após o login.</returns>
    public async Task<string> LoginUser(LoginUserDto userDto) {
        var resultado = await _signInManager.PasswordSignInAsync(userDto.Cpf, userDto.Password, false, false);

        if(!resultado.Succeeded) {
            throw new ApplicationException("Usuário não autenticado");
        }
        var user = _signInManager.UserManager.Users.FirstOrDefault(user => user.Cpf == userDto.Cpf);

        if(user == null) {
            throw new ApplicationException("Usuário não encontrado.");
        }
        var token = _tokenService.GenerateToken(user);

        return token;
    }

    /// <summary>
    /// Realiza o logout do usuário.
    /// </summary>
    /// <returns>Retorna um Task representando a operação de logout.</returns>
    public async Task Logout() {
        await _signInManager.SignOutAsync();
    }

    /// <summary>
    /// Obtém todos os usuários que ainda não foram aprovados.
    /// </summary>
    /// <returns>Uma lista de DTOs contendo apenas Nome e CPF de usuários não aprovados.</returns>
    public async Task<List<CadastroPendenteDto>> ObterUsuariosNaoAprovadosAsync() {
        return await _userManager.Users.Where(u => !u.IsAprovado)
                                .Select(u => new CadastroPendenteDto {
                                    Nome = u.Name,
                                    Cpf = u.Cpf
                                })
                                .ToListAsync();
    }

    /// <summary>
    /// Obtém um usuário específico que ainda não foi aprovado pelo CPF informado.
    /// </summary>
    /// <param name="cpf">O CPF do usuário a ser buscado.</param>
    /// <returns>Um DTO contendo os dados do usuário não aprovado ou nulo se não encontrado.</returns>
    public async Task<ReadUserDto?> ObterUsuarioNaoAprovadoPorCpfAsync(string cpf) {
        return await _userManager.Users
                                .Where(u => !u.IsAprovado && u.Cpf == cpf)
                                .Select(u => new ReadUserDto {
                                    Id = u.Id,
                                    IsAdmin = u.IsAdmin,
                                    Name = u.Name,
                                    Siape = u.Siape,
                                    Cpf = u.Cpf,
                                    Email = u.Email ?? string.Empty,
                                    PhoneNumber = u.PhoneNumber ?? string.Empty,
                                    IsAprovado = u.IsAprovado
                                })
                                .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtém todos os usuários que já foram aprovados.
    /// </summary>
    /// <returns>Uma lista de DTOs contendo apenas Nome e CPF de usuários aprovados.</returns>
    public async Task<List<CadastroPendenteDto>> ObterUsuariosAprovadosAsync() {
        return await _userManager.Users.Where(u => u.IsAprovado && !u.IsAdmin)
                                .Select(u => new CadastroPendenteDto {
                                    Nome = u.Name,
                                    Cpf = u.Cpf
                                })
                                .ToListAsync();
    }

    /// <summary>
    /// Obtém um usuário específico que já foi aprovado pelo CPF informado.
    /// </summary>
    /// <param name="cpf">O CPF do usuário a ser buscado.</param>
    /// <returns>Um DTO contendo os dados do usuário já aprovado ou nulo se não encontrado.</returns>
    public async Task<ReadUserDto?> ObterUsuarioAprovadoPorCpfAsync(string cpf) {
        return await _userManager.Users
                                .Where(u => u.IsAprovado && u.Cpf == cpf && !u.IsAdmin)
                                .Select(u => new ReadUserDto {
                                    Id = u.Id,
                                    IsAdmin = u.IsAdmin,
                                    Name = u.Name,
                                    Siape = u.Siape,
                                    Cpf = u.Cpf,
                                    Email = u.Email ?? string.Empty,
                                    PhoneNumber = u.PhoneNumber ?? string.Empty,
                                    IsAprovado = u.IsAprovado
                                })
                                .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Aprova um usuário, alterando o status de IsAprovado para true.
    /// </summary>
    /// <param name="cpf">CPF do usuário a ser aprovado.</param>
    /// <returns>Task</returns>
    public async Task<string> AprovarUsuarioAsync(string cpf) {
        var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

        if(usuario == null) {
            throw new Exception("Usuário não encontrado.");
        }
        if(usuario.IsAprovado) {
            return "Usuário já está aprovado.";
        }

        usuario.IsAprovado = true;
        
        if(usuario.Email!=null) {
            await _emailSender.SendEmailAsync(
                usuario.Email,
                "BookIt - Cadastro Aprovado",
                $"Parabéns, seu cadastro foi aprovado e você já pode acessar o sistema BookIt.");
        }

        await _userManager.UpdateAsync(usuario);
        return "Usuário aprovado com sucesso.";
    }

    /// <summary>
    /// Exclui um usuário pelo CPF, verificando se o usuário não é administrador.
    /// </summary>
    /// <param name="cpf">CPF do usuário a ser excluído.</param>
    /// <returns>Task</returns>
    public async Task<string> ExcluirUsuarioAsync(string cpf) {
        var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

        if(usuario == null) {
            throw new Exception("Usuário não encontrado.");
        }
        
        //verifica se o usuário é administrador
        var isAdminClaim = await _userManager.GetClaimsAsync(usuario);
        var isAdmin = isAdminClaim.Any(c => c.Type == "IsAdmin" && c.Value == "True");
        if(isAdmin) {
            return "Não é possível excluir um usuário administrador.";
        }

        //exclui o usuário
        var result = await _userManager.DeleteAsync(usuario);

        if(!result.Succeeded) {
            throw new Exception("Erro ao excluir o usuário.");
        }

        return "Usuário excluído com sucesso.";
    }

    /// <summary>
    /// Cria uma nova reserva.
    /// </summary>
    /// <param name="createReservaDto">Objeto DTO contendo os dados necessários para criar uma reserva.</param>
    /// <param name="userId">Id do usuário que está fazendo a reserva.</param>
    /// <returns>Retorna a reserva criada.</returns>
    public async Task<Reserva> CreateReservaAsync(CreateReservaDto createReservaDto, string userId) {
        var user = await _userManager.FindByIdAsync(userId);
        if(user == null) {
            throw new ApplicationException("Usuário não encontrado.");
        }

        var reserva = new Reserva {
            Tipo = createReservaDto.Tipo,
            DataReserva = createReservaDto.DataReserva,
            Horarios = createReservaDto.Horarios,
            Usuario = user //associando a reserva ao usuário logado
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return reserva;
    }

    /// <summary>
    /// Obtém todas as reservas para um ambiente em um dia específico.
    /// Inclui os dados do usuário que fez a reserva.
    /// </summary>
    /// <param name="dataReserva">Data da reserva</param>
    /// <param name="ambiente">Nome do ambiente</param>
    /// <returns>Lista de reservas encontradas com informações do usuário</returns>
    public async Task<List<Reserva>> GetReservasPorDataEAmbienteAsync(DateTime dataReserva, string ambiente) {
        return await _context.Reservas
            .Include(r => r.Usuario)  
            .Where(r => r.DataReserva.Date == dataReserva.Date && r.Tipo == ambiente)
            .ToListAsync();
    }

    /// <summary>
    /// Busca um usuário pelo ID.
    /// </summary>
    public async Task<User?> GetUsuarioPorIdAsync(string userId) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user == null) {
            Console.WriteLine($"Usuário com ID {userId} não encontrado.");
        }else {
            Console.WriteLine($"Usuário encontrado: {user.Name} (CPF: {user.Cpf})");
        }
        return user; 
    }

    /// <summary>
    /// Busca todas as reservas de um usuário específico pelo CPF.
    /// </summary>
    public async Task<List<Reserva>> GetReservasPorUsuarioAsync(string cpf) {
        try {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

            if(user == null) {
                throw new ApplicationException("Usuário não encontrado.");
            }

            var reservas = await _context.Reservas
                .Where(r => r.UsuarioId == user.Id)
                .ToListAsync();

            return reservas;
        }catch(Exception ex) {
            Console.WriteLine($"Erro ao buscar reservas para CPF {cpf}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Serviço para excluir uma reserva de um servidor
    /// </summary>
    /// <param name="reservaId"></param>
    /// <returns></returns>
    public async Task<bool> ExcluirReservaAsync(int reservaId) {
        var reserva = await _context.Reservas.FindAsync(reservaId);
        if (reserva == null) {
            return false; 
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return true; 
    }
}