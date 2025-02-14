using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.Admin;

/// <summary>
/// Representa os dados de um admin que foram recuperados do sistema.
/// </summary>
public class ReadAdminDto {

    /// <summary>
    /// Obtém o identificador único do admin.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o nome completo do admin.
    /// Exemplo: "Rennan Alves".
    /// O nome completo não pode ter mais de 100 caracteres.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o SIAPE do admin.
    /// O SIAPE deve ter exatamente 6 dígitos numéricos.
    /// </summary>
    public string Siape { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o CPF do admin.
    /// O CPF deve estar no formato 000.000.000-00.
    /// </summary>
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o e-mail do admin.
    /// O e-mail deve ser válido e não pode ter mais de 120 caracteres.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtém o número de celular do admin.
    /// O celular deve estar no formato (00) 00000-0000.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
}