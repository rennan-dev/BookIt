using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.User;

/// <summary>
/// Representa os dados de um usuário que foram recuperados do sistema.
/// </summary>
public class ReadUserDto {

    /// <summary>
    /// Obtém o identificador único do usuário.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o usuário é admin ou não.
    /// </summary>
    public bool IsAdmin { get; set; } = true;

    /// <summary>
    /// Obtém o nome completo do usuário.
    /// O nome completo não pode ter mais de 100 caracteres.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o SIAPE do usuário.
    /// O SIAPE deve ter exatamente 6 dígitos numéricos.
    /// </summary>
    public string Siape { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o CPF do usuário.
    /// O CPF deve estar no formato 000.000.000-00.
    /// </summary>
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o e-mail do usuário.
    /// O e-mail deve ser válido e não pode ter mais de 120 caracteres.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o número de celular do usuário.
    /// O celular deve estar no formato (00) 00000-0000.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o servidor foi aprovado pelo administrador para utilizar o sistema.
    /// Só será verdadeiro quando o administrador aprovar o servidor.
    /// </summary>
    public bool IsAprovado { get; set; } = false;
}