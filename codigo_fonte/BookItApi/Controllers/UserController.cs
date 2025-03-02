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
        try {
            await _userService.CadastraUsuario(userDto);
            return Ok("Usuário cadastrado!");
        } catch (ApplicationException ex) {
            return BadRequest(new { message = ex.Message });
        } catch (Exception ex) {
            return StatusCode(500, new { message = $"Erro interno no servidor. Erro: {ex}" });
        }
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
        try {
            var token = await _userService.LoginUser(userDto);
            return Ok(token);
        }catch(ApplicationException ex) {
            return BadRequest(new { message = ex.Message });
        }catch(Exception ex) {
            return StatusCode(500, new { message = $"Erro interno no servidor. Erro: {ex}" });
        }
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
    [HttpGet("pendentes")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ObterUsuariosNaoAprovados() {
        var usuarios = await _userService.ObterUsuariosNaoAprovadosAsync();

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
    [HttpGet("servidores")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ObterUsuariosAprovados() {
        var usuarios = await _userService.ObterUsuariosAprovadosAsync();

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
    /// <response code="200">Caso não haja reservas para os parâmetros fornecidos</response>
    [HttpGet("reservas/{dataReserva}/{ambiente}")]
    public async Task<IActionResult> GetReservasPorDataEAmbiente(string dataReserva, string ambiente) {
        try {
            if (!DateTime.TryParse(dataReserva, out DateTime data)) {
                return BadRequest("Data inválida.");
            }

            var reservas = await _userService.GetReservasPorDataEAmbienteAsync(data, ambiente);

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
        } catch (Exception ex) {
            return StatusCode(500, $"Erro ao buscar reservas: {ex.Message}");
        }
    }

    /// <summary>
    /// Retorna as reservas do usuário autenticado, utilizando o CPF associado ao usuário.
    /// </summary>
    /// <returns>Lista de reservas do usuário.</returns>
    /// <response code="200">Reservas encontradas (ou lista vazia se nenhuma reserva)</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("reservas")]
    [Authorize(Policy = "ServidorOnly")]
    public async Task<IActionResult> GetReservasDoUsuario() {
        try {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userId)) {
                return Unauthorized("Usuário não autenticado.");
            }
            var user = await _userService.GetUsuarioPorIdAsync(userId);
            if(user == null) {
                return NotFound("Usuário não encontrado.");
            }

            var reservas = await _userService.GetReservasPorUsuarioAsync(user.Cpf);

            if(reservas == null || reservas.Count == 0) {
                return NotFound("Nenhuma reserva encontrada.");
            }
            return Ok(reservas);
        }catch(Exception ex) {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    /// <summary>
    /// Método para excluir reservas de um servidor
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("reservas/{id}")]
    [Authorize(Policy = "ServidorOnly")]
    public async Task<IActionResult> ExcluirReserva(int id) {
        var resultado = await _userService.ExcluirReservaAsync(id);
        
        if(!resultado) {
            return NotFound($"Reserva com ID {id} não encontrada.");
        }

        return NoContent(); 
    }
}