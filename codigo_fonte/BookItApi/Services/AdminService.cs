using AutoMapper;
using BookItApi.Data;
using BookItApi.Dtos.Admin;
using BookItApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookItApi.Services;

/// <summary>
/// Serviço responsável pelas operações de autenticação e gerenciamento de administradores no sistema.
/// Inclui funcionalidades como cadastro, login, logout e etc.
/// </summary>
public class AdminService {
    private IMapper _mapper;
    private UserManager<Admin> _userManager;
    private SignInManager<Admin> _signInManager;
    private TokenService _tokenService;
    private AdminDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AdminService"/>.
    /// </summary>
    /// <param name="mapper">Objeto de mapeamento entre DTOs e modelos de domínio.</param>
    /// <param name="userManager">Gerenciador de usuários do Identity para administração de contas de usuário.</param>
    /// <param name="signInManager">Gerenciador de login do Identity.</param>
    /// <param name="tokenService">Serviço responsável pela geração de tokens JWT.</param>
    /// <param name="context">Contexto do banco de dados específico para a administração de dados de admins.</param>
    public AdminService(IMapper mapper, UserManager<Admin> userManager, SignInManager<Admin> signInManager, TokenService tokenService, AdminDbContext context) {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
    }

    /// <summary>
    /// Cadastra um novo administrador no sistema, garantindo que SIAPE, e-mail e telefone sejam únicos
    /// e que não haja um Servidor já cadastrado com os mesmos dados.
    /// </summary>
    /// <param name="adminDto">Objeto com os dados do administrador a ser cadastrado.</param>
    /// <returns>Retorna um Task representando a operação de cadastro.</returns>
    /// <exception cref="ApplicationException">Lançada caso já exista um administrador ou servidor com dados duplicados.</exception>
    public async Task CadastraAdmin(CreateAdminDto adminDto) {
        try {
            var adminExistente = await _userManager.Users
                .Where(a => a.Siape == adminDto.Siape || 
                            a.Email == adminDto.Email || 
                            a.PhoneNumber == adminDto.PhoneNumber)
                .FirstOrDefaultAsync();

            if(adminExistente != null) {
                if(adminExistente.Siape == adminDto.Siape) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este SIAPE.");
                }
                if(adminExistente.Email == adminDto.Email) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este e-mail.");
                }
                if(adminExistente.PhoneNumber == adminDto.PhoneNumber) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este telefone.");
                }
            }

            var servidorExistente = await _context.Servidores
                .Where(s => s.Siape == adminDto.Siape || 
                            s.Cpf == adminDto.Cpf || 
                            s.Email == adminDto.Email || 
                            s.PhoneNumber == adminDto.PhoneNumber)
                .FirstOrDefaultAsync();

            if(servidorExistente != null) {
                if(servidorExistente.Siape == adminDto.Siape) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este SIAPE.");
                }
                if(servidorExistente.Cpf == adminDto.Cpf) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este CPF.");
                }
                if(servidorExistente.Email == adminDto.Email) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este e-mail.");
                }
                if(servidorExistente.PhoneNumber == adminDto.PhoneNumber) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este telefone.");
                }
            }

            Admin admin = _mapper.Map<Admin>(adminDto);
            admin.UserName = adminDto.Cpf;

            IdentityResult resultado = await _userManager.CreateAsync(admin, adminDto.Password);

            if(!resultado.Succeeded) {
                var errors = string.Join(", ", resultado.Errors.Select(e => e.Description));
                throw new ApplicationException($"Falha ao cadastrar Administrador: {errors}");
            }
        }catch(Exception ex) {
            Console.WriteLine($"Erro ao cadastrar o administrador: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw new ApplicationException($"Falha ao cadastrar o administrador. Detalhes: {ex.Message}");
        }
    }

    /// <summary>
    /// Realiza o login de um administrador no sistema.
    /// </summary>
    /// <param name="adminDto">Objeto com as credenciais do administrador para o login.</param>
    /// <returns>Retorna o token JWT gerado após o login.</returns>
    public async Task<string> Login(LoginAdminDto adminDto) {
        var resultado = await _signInManager.PasswordSignInAsync(adminDto.Cpf, adminDto.Password, false, false);

        if(!resultado.Succeeded) {
            throw new ApplicationException("Administrador não autenticado");
        }
        var admin = _signInManager.UserManager.Users.FirstOrDefault(admin => admin.Cpf == adminDto.Cpf);

        if(admin == null) {
            throw new ApplicationException("Administrador não encontrado.");
        }
        var token = _tokenService.GenerateToken(admin);

        return token;
    }

    /// <summary>
    /// Realiza o logout do administrador.
    /// </summary>
    /// <returns>Retorna um Task representando a operação de logout.</returns>
    public async Task Logout() {
        await _signInManager.SignOutAsync();
    }
}