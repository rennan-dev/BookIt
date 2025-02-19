using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.User;

/// <summary>
/// Representa os dados de um usuário no sistema com o cadastro não confirmado.
/// </summary>
public class CadastroPendenteDto {

    /// <summary>
    /// Obtém o nome completo do usuário.
    /// </summary>
    [Required] public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o CPF do usuário.
    /// </summary>
    [Required] public string Cpf { get; set; } = string.Empty;
}
