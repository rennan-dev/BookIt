using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.Servidor;

/// <summary>
/// Representa os dados de um servidor que foram recuperados do sistema.
/// </summary>
public class ReadServidorDto {

    /// <summary>
    /// Obtém ou define o identificador único do servidor.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome completo do servidor.
    /// Exemplo: "Rennan Alves".
    /// O nome completo não pode ter mais de 100 caracteres.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o SIAPE do servidor.
    /// O SIAPE deve ter exatamente 6 dígitos numéricos.
    /// </summary>
    public string Siape { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o CPF do servidor.
    /// O CPF deve estar no formato 000.000.000-00.
    /// </summary>
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o e-mail do servidor.
    /// O e-mail deve ser válido e não pode ter mais de 120 caracteres.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o número de celular do servidor.
    /// O celular deve estar no formato (00) 00000-0000.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o servidor foi aprovado pelo administrador para utilizar o sistema.
    /// Só será verdadeiro quando o administrador aprovar o servidor.
    /// </summary>
    public bool IsAprovado { get; set; }
}