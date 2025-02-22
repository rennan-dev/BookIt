using System.Security.Claims;
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
    /// Obtém um usuário específico que ainda não foi aprovado, utilizando o CPF fornecido.
    /// </summary>
    /// <param name="cpf">O CPF do usuário a ser buscado.</param>
    /// <returns>Os dados do usuário não aprovado.</returns>
    /// <response code="200">Retorna os dados do usuário não aprovado</response>
    /// <response code="404">Caso o usuário não seja encontrado</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("pendentes/{cpf}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ObterUsuarioNaoAprovadoPorCpf(string cpf) {
        var usuario = await _userService.ObterUsuarioNaoAprovadoPorCpfAsync(cpf);

        if(usuario == null) {
            return NotFound($"Nenhum usuário pendente encontrado com o CPF: {cpf}");
        }

        return Ok(usuario);
    }

    /// <summary>
    /// Obtém todos os usuários que já foram aprovados.
    /// </summary>
    /// <returns>Lista de usuários aprovados.</returns>
    /// <response code="200">Retorna a lista de usuários aprovados</response>
    /// <response code="404">Caso não haja usuários aprovados</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("servidores")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ObterUsuariosAprovados() {
        var usuarios = await _userService.ObterUsuariosAprovadosAsync();

        if(usuarios == null || usuarios.Count == 0) {
            return NotFound("Nenhum usuário aprovado encontrado.");
        }

        return Ok(usuarios);
    }

    /// <summary>
    /// Obtém um usuário específico que já foi aprovado, utilizando o CPF fornecido.
    /// </summary>
    /// <param name="cpf">O CPF do usuário a ser buscado.</param>
    /// <returns>Os dados do usuário aprovado.</returns>
    /// <response code="200">Retorna os dados do usuário aprovado</response>
    /// <response code="404">Caso o usuário não seja encontrado</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("servidores/{cpf}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ObterUsuarioAprovadoPorCpf(string cpf) {
        var usuario = await _userService.ObterUsuarioAprovadoPorCpfAsync(cpf);

        if(usuario == null) {
            return NotFound($"Nenhum usuário aprovado encontrado com o CPF: {cpf}");
        }

        return Ok(usuario);
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

    /// <summary>
    /// Exclui um usuário com base no CPF, verificando se ele é administrador.
    /// </summary>
    /// <param name="cpf">CPF do usuário a ser excluído.</param>
    /// <returns>Task</returns>
    /// <response code="200">Usuário excluído com sucesso ou não pode ser excluído.</response>
    /// <response code="404">Caso o usuário não seja encontrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("excluir/{cpf}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ExcluirUsuario(string cpf) {
        try {
            var resultado = await _userService.ExcluirUsuarioAsync(cpf);
            return Ok(resultado); 
        }catch(Exception ex) {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Cria uma nova reserva.
    /// </summary>
    /// <param name="createReservaDto">Objeto DTO contendo os dados necessários para criar uma reserva.</param>
    /// <returns>Retorna a reserva criada.</returns>
    /// <response code="200">Caso a reserva seja criada com sucesso</response>
    /// <response code="400">Caso o usuário não seja um servidor ou haja erro ao criar a reserva</response>
    [HttpPost("reservar")]
    [Authorize(Policy = "ServidorOnly")]  
    public async Task<IActionResult> CreateReserva(CreateReservaDto createReservaDto) {
        try {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(string.IsNullOrEmpty(userId)) {
                return BadRequest("Usuário não autenticado ou claim NameIdentifier ausente.");
            }

            var reserva = await _userService.CreateReservaAsync(createReservaDto, userId);
            return Ok(reserva);
        }catch(ApplicationException ex) {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Busca todas as reservas de um ambiente em um dia específico.
    /// </summary>
    /// <param name="dataReserva">Data da reserva no formato YYYY-MM-DD</param>
    /// <param name="ambiente">Nome do ambiente</param>
    /// <returns>Retorna uma lista de reservas para o dia e ambiente especificados.</returns>
    /// <response code="200">Caso as reservas sejam encontradas</response>
    /// <response code="400">Caso os parâmetros sejam inválidos</response>
    /// <response code="404">Caso não haja reservas para os parâmetros fornecidos</response>
    [HttpGet("reservas/{dataReserva}/{ambiente}")]
    public async Task<IActionResult> GetReservasPorDataEAmbiente(string dataReserva, string ambiente) {
        try {
            if(!DateTime.TryParse(dataReserva, out DateTime data)) {
                return BadRequest("Data inválida.");
            }
            var reservas = await _userService.GetReservasPorDataEAmbienteAsync(data, ambiente);

            if(reservas == null || !reservas.Any()) {
                return NotFound("Nenhuma reserva encontrada.");
            }
            var resultado = reservas.Select(r => new {
                r.Id,
                r.Tipo,
                r.DataReserva,
                r.Horarios,
                Usuario = r.Usuario != null ? new {
                    r.Usuario.Id,
                    r.Usuario.Name,
                    r.Usuario.Cpf
                } : null
            });

            return Ok(resultado);
        }catch(Exception ex) {
            return StatusCode(500, $"Erro ao buscar reservas: {ex.Message}");
        }
    }
}