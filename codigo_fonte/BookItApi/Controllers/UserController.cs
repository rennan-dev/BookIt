using BookItApi.Dtos.User;
using BookItApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItApi.Controllers;


/// <summary>
/// Controlador responsável por gerenciar as operações dos usuários no sistema,
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    
    private UserService _userService;

    /// <summary>
    /// Inicializa uma nova instância do controlador <see cref="UserController"/>.
    /// </summary>
    /// <param name="userService">Serviço responsável pela lógica de negócios relacionada aos usuários.</param>
    public UserController(UserService userService) {
        _userService = userService;
    }

    /// <summary>
    /// Adiciona um usuário ao banco de dados do BookIt
    /// </summary>
    /// <param name="userDto">Objetos com os campos necessários para a criação de um usuário</param>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("cadastro")]
    public async Task<IActionResult> CadastraUser(CreateUserDto userDto) {
        await _userService.CadastraUsuario(userDto);
        return Ok("Usuário cadastrado!");
    }

    /// <summary>
    /// Faz login no sistema
    /// </summary>
    /// <param name="userDto">Objetos com os campos necessários para a realização do login</param>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso login seja feito com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserDto userDto) {
        var token = await _userService.LoginUser(userDto);
        return Ok(token);
    }

    /// <summary>
    /// Faz logout com um usuário
    /// </summary>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso o usuário se deslogue com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await _userService.Logout();
        return Ok("Logout realizado com sucesso.");
    }

    /// <summary>
    /// Obtém todos os usuários que ainda não foram aprovados.
    /// </summary>
    /// <returns>Lista de usuários não aprovados.</returns>
    /// <response code="200">Retorna a lista de usuários não aprovados</response>
    /// <response code="404">Caso não haja usuários não aprovados</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("pendentes")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ObterUsuariosNaoAprovados() {
        var usuarios = await _userService.ObterUsuariosNaoAprovadosAsync();

        if(usuarios == null || usuarios.Count == 0) {
            return NotFound("Nenhum usuário pendente encontrado.");
        }

        return Ok(usuarios);
    }

    /// <summary>
    /// Aprova um usuário com base no CPF.
    /// </summary>
    /// <param name="cpf">CPF do usuário a ser aprovado.</param>
    /// <returns>Task</returns>
    /// <response code="200">Usuário aprovado com sucesso ou já está aprovado.</response>
    /// <response code="404">Caso o usuário não seja encontrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("aprovar/{cpf}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AprovarUsuario(string cpf) {
        try {
            var resultado = await _userService.AprovarUsuarioAsync(cpf);
            return Ok(resultado); 
        }catch(Exception ex) {
            return NotFound(ex.Message);
        }
    }
}