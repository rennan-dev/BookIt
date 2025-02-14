using BookItApi.Dtos.Admin;
using BookItApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookItApi.Controllers;


/// <summary>
/// Controlador responsável por gerenciar as operações de administradores no sistema,
/// incluindo cadastro, login e logout de administradores.
/// </summary>
[ApiController]
[Route("api/controller")]
public class AdminController : ControllerBase {
    
    private AdminService _adminService;

    /// <summary>
    /// Inicializa uma nova instância do controlador <see cref="AdminController"/>.
    /// </summary>
    /// <param name="adminService">Serviço responsável pela lógica de negócios relacionada aos administradores.</param>
    public AdminController(AdminService adminService) {
        _adminService = adminService;
    }

    /// <summary>
    /// Adiciona um administrador ao banco de dados do WebAPI
    /// </summary>
    /// <param name="adminDto">Objetos com os campos necessários para a criação de um admin</param>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("cadastro")]
    public async Task<IActionResult> CadastraUsuario(CreateAdminDto adminDto) {
        await _adminService.CadastraAdmin(adminDto);
        return Ok("Admin cadastrado!");
    }

    /// <summary>
    /// Faz login no sistema como admin
    /// </summary>
    /// <param name="adminDto">Objetos com os campos necessários para a realização do login</param>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso login seja feito com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAdminDto adminDto) {
        var token = await _adminService.Login(adminDto);
        return Ok(token);
    }

    /// <summary>
    /// Faz login com um administrador no WebAPI
    /// </summary>
    /// <returns>
    /// Task
    /// </returns>
    /// <response code="200">Caso o admin se deslogue com sucesso</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await _adminService.Logout();
        return Ok("Logout realizado com sucesso.");
    }
}