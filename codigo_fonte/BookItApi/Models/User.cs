using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookItApi.Models;

/// <summary>
/// Representa um usuário no sistema, que herda de IdentityUser e inclui campos adicionais como IsAdmin, Name, SIAPE, CPF e IsAprovado.
/// </summary>
public class User : IdentityUser {

    /// <summary>
    /// Indica se o usuário é admin ou não.
    /// </summary>
    public bool IsAdmin { get; set; } = false;

    /// <summary>
    /// Obtém ou define o nome do usuário.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o SIAPE do usuário.
    /// O SIAPE deve ter exatamente 6 dígitos numéricos.
    /// </summary>
    [Required(ErrorMessage = "O SIAPE é obrigatório.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "O SIAPE deve ter exatamente 6 dígitos numéricos.")]
    public string Siape { get; set; } = string.Empty;


    /// <summary>
    /// Obtém ou define o CPF do usuário.
    /// O CPF deve estar no formato 000.000.000-00.
    /// </summary>
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 000.000.000-00.")]
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o servidor foi aprovado pelo administrador para utilizar o sistema.
    /// Só será verdadeiro quando o administrador aprovar o servidor.
    /// </summary>
    [Required(ErrorMessage = "O usuário precisa ser aprovado para acessar o sistema")]
    public bool IsAprovado { get; set; } = false;
}