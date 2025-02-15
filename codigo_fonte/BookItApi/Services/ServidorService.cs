using AutoMapper;
using BookItApi.Data;
using BookItApi.Dtos.Admin;
using BookItApi.Dtos.Servidor;
using BookItApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookItApi.Services;

/// <summary>
/// Serviço responsável pelas operações de autenticação e gerenciamento de servidores no sistema.
/// Inclui funcionalidades como cadastro, login, logout e etc.
/// </summary>
public class ServidorService {
    private IMapper _mapper;
    private TokenService _tokenService;
    private AdminDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ServidorService"/>.
    /// </summary>
    /// <param name="mapper">Objeto de mapeamento entre DTOs e modelos de domínio.</param>
    /// <param name="tokenService">Serviço responsável pela geração de tokens JWT.</param>
    /// <param name="context">Contexto do banco de dados específico para a administração de dados de servidores.</param>
    public ServidorService(IMapper mapper, TokenService tokenService, AdminDbContext context) {
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
    }

    /// <summary>
    /// Cadastra um novo servidor no sistema, garantindo que SIAPE, CPF, e-mail e telefone sejam únicos
    /// e que não haja um Administrador já cadastrado com os mesmos dados.
    /// </summary>
    /// <param name="servidorDto">Objeto com os dados do servidor a ser cadastrado.</param>
    /// <returns>Retorna um Task representando a operação de cadastro.</returns>
    /// <exception cref="ApplicationException">Lançada caso já exista um servidor ou administrador com dados duplicados.</exception>
    public async Task CadastraServidor(CreateServidorDto servidorDto) {
        try {
            var servidorExistente = await _context.Servidores
                .Where(s => s.Siape == servidorDto.Siape || 
                            s.Cpf == servidorDto.Cpf || 
                            s.Email == servidorDto.Email || 
                            s.PhoneNumber == servidorDto.PhoneNumber)
                .FirstOrDefaultAsync();

            if(servidorExistente != null) {
                if(servidorExistente.Siape == servidorDto.Siape) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este SIAPE.");
                }
                if(servidorExistente.Cpf == servidorDto.Cpf) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este CPF.");
                }
                if(servidorExistente.Email == servidorDto.Email) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este e-mail.");
                }
                if(servidorExistente.PhoneNumber == servidorDto.PhoneNumber) {
                    throw new ApplicationException("Já existe um servidor cadastrado com este telefone.");
                }
            }

            var adminExistente = await _context.Users
                .Where(a => a.Siape == servidorDto.Siape || 
                            a.Cpf == servidorDto.Cpf || 
                            a.Email == servidorDto.Email || 
                            a.PhoneNumber == servidorDto.PhoneNumber)
                .FirstOrDefaultAsync();

            if(adminExistente != null) {
                if(adminExistente.Siape == servidorDto.Siape) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este SIAPE.");
                }
                if(adminExistente.Cpf == servidorDto.Cpf) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este CPF.");
                }
                if(adminExistente.Email == servidorDto.Email) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este e-mail.");
                }
                if(adminExistente.PhoneNumber == servidorDto.PhoneNumber) {
                    throw new ApplicationException("Já existe um administrador cadastrado com este telefone.");
                }
            }

            Servidor servidor = _mapper.Map<Servidor>(servidorDto);
            servidor.UserName = servidorDto.Cpf;
            var passwordHasher = new PasswordHasher<Servidor>();
            servidor.PasswordHash = passwordHasher.HashPassword(servidor, servidorDto.Password);

            _context.Servidores.Add(servidor);
            await _context.SaveChangesAsync();
            
        }catch(Exception ex) {
            Console.WriteLine($"Erro ao cadastrar o servidor: {ex.Message}");
            Console.WriteLine(ex.StackTrace);  
            throw new ApplicationException($"Falha ao cadastrar o servidor. Detalhes: {ex.Message}");
        }
    }

    /// <summary>
    /// Realiza o login de um servidor no sistema.
    /// </summary>
    /// <param name="servidorDto">Objeto com as credenciais do servidor para o login.</param>
    /// <returns>Retorna o token JWT gerado após o login.</returns>
    public async Task<string> Login(LoginServidorDto servidorDto) {
        var servidor = await _context.Servidores.FirstOrDefaultAsync(s => s.Cpf == servidorDto.Cpf);
        
        if(servidor == null) {
            throw new ApplicationException("Servidor não encontrado.");
        }

        if(string.IsNullOrEmpty(servidor.PasswordHash)) {
            throw new ApplicationException("Servidor não possui uma senha cadastrada.");
        }
        var passwordHasher = new PasswordHasher<Servidor>();
        var result = passwordHasher.VerifyHashedPassword(servidor, servidor.PasswordHash, servidorDto.Password);

        if(result != PasswordVerificationResult.Success) {
            throw new ApplicationException("Credenciais inválidas.");
        }

        // Gera o token JWT
        var token = _tokenService.GenerateToken(servidor);
        return token;
    }

    /// <summary>
    /// Realiza o logout do servidor.
    /// </summary>
    public void Logout() {
        // O logout para JWT é tratado no front-end, então não há nada para invalidar no backend.
    }
}