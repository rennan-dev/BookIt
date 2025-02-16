using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.User;

/// <summary>
/// Representa os dados de um usuário para fazer login no sistema.
/// </summary>
public class LoginUserDto {

    /// <summary>
    /// Obtém o CPF do usuário.
    /// O CPF deve estar no formato 000.000.000-00.
    /// </summary>
    [Required] public string Cpf { get; set; } = string.Empty;


    /// <summary>
    /// Obtém a senha do usuário.
    /// </summary>
    [Required] public string Password { get; set; } = string.Empty;
}