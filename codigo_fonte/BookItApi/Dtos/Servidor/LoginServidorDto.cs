using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.Servidor;

/// <summary>
/// Representa os dados de um servidor para fazer login no sistema.
/// </summary>
public class LoginServidorDto {

    /// <summary>
    /// Obtém o CPF do admin.
    /// O CPF deve estar no formato 000.000.000-00.
    /// </summary>
    [Required] public string Cpf { get; set; } = string.Empty;


    /// <summary>
    /// Obtém a senha do admin.
    /// </summary>
    [Required] public string Password { get; set; } = string.Empty;
}