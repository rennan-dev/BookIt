using BookItApi.Dtos.Servidor;
using BookItApi.Profiles;
using BookItApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookItApi.Controllers;


/// <summary>
/// Controlador responsável por gerenciar as operações de servidores no sistema,
/// incluindo cadastro, login e logout de servidores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ServidorController : ControllerBase {
    
    private ServidorService _servidorService;

    /// <summary>
    /// Inicializa uma nova instância do controlador <see cref="ServidorController"/>.
    /// </summary>
    /// <param name="servidorService">Serviço responsável pela lógica de negócios relacionada aos administradores.</param>
    public ServidorController(ServidorService servidorService) {
        _servidorService = servidorService;
    }

    /// <summary>
    /// Adiciona um servidor ao banco de dados do BookIt
    /// </summary>
    /// <param name="servidorDto">Objetos com os campos necessários para a criação de um servidor</param>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("cadastro")]
    public async Task<IActionResult> CadastraUsuario(CreateServidorDto servidorDto) {
        await _servidorService.CadastraServidor(servidorDto);
        return Ok("Servidor cadastrado!");
    }

    /// <summary> 
    /// Faz login no sistema como Servidor
    /// </summary>
    /// <param name="servidorDto">Objeto com os campos necessários para a realização do login</param>
    /// <returns>Retorna um token JWT se a autenticação for bem-sucedida.</returns>
    /// <response code="200">Caso login seja feito com sucesso</response>
    /// <response code="401">Caso as credenciais estejam incorretas</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginServidorDto servidorDto) {
        try {
            var token = await _servidorService.Login(servidorDto);
            return Ok(token);
        }catch(ApplicationException ex) {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Faz logout do sistema como Servidor.
    /// </summary>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso o servidor se deslogue com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("logout")]
    public IActionResult Logout() {
        _servidorService.Logout();
        return Ok("Logout realizado com sucesso.");
    }
}