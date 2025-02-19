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

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserService"/>.
    /// </summary>
    /// <param name="mapper">Objeto de mapeamento entre DTOs e modelos de domínio.</param>
    /// <param name="userManager">Gerenciador de usuários do Identity para administração de contas de usuário.</param>
    /// <param name="signInManager">Gerenciador de login do Identity.</param>
    /// <param name="tokenService">Serviço responsável pela geração de tokens JWT.</param>
    /// <param name="context">Contexto do banco de dados específico para a administração de dados de usuários.</param>
    public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService, UserDbContext context) {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
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

            if(userExistente != null) {
                if(userExistente.Siape == userDto.Siape) {
                    throw new ApplicationException("Já existe um usuário cadastrado com este SIAPE.");
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

            if(!resultado.Succeeded) {
                var errors = string.Join(", ", resultado.Errors.Select(e => e.Description));
                throw new ApplicationException($"Falha ao cadastrar Usuário: {errors}");
            }
        }catch(Exception ex) {
            Console.WriteLine($"Erro ao cadastrar o usuário: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw new ApplicationException($"Falha ao cadastrar o usuário. Detalhes: {ex.Message}");
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
        return await _userManager.Users
                                .Where(u => !u.IsAprovado)
                                .Select(u => new CadastroPendenteDto {
                                    Nome = u.Name,
                                    Cpf = u.Cpf
                                })
                                .ToListAsync();
    }
}