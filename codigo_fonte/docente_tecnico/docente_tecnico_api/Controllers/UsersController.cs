using System.Threading.Tasks;
using AutoMapper;
using docente_tecnico_api.Dtos;
using docente_tecnico_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace docente_tecnico_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersControler : ControllerBase {

    private IMapper _mapper;
    private UserManager<User> _userManager;

    public UsersControler(IMapper mapper, UserManager<User> userManager) {
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarUsuario(CreateUserDto createUserDto) {
        
        User user = _mapper.Map<User>(createUserDto);

        IdentityResult result = await _userManager.CreateAsync(user, createUserDto.Password);

        if(result.Succeeded) return Ok("Usuário cadastrado.");

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest($"Falha ao cadastrar usuário: {errors}");
    }
}