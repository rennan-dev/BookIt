using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.User;

/// <summary>
/// Representa os dados necessários para criar um novo usuário no sistema.
/// </summary>
public class CreateUserDto {

    /// <summary>
    /// Indica se o usuário é admin ou servidor.
    /// </summary>
    [Required(ErrorMessage = "O usuário precisa ser definido como admin ou não")]
    public bool IsAdmin { get; set; } = false;

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
    /// Obtém ou define o nome completo do usuário.
    /// O nome completo não pode ter mais de 100 caracteres.
    /// </summary>
    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome completo não pode ter mais de 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o e-mail do usuário.
    /// O e-mail deve ser válido e não pode ter mais de 120 caracteres.
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    [StringLength(120, ErrorMessage = "O e-mail deve ter no máximo 120 caracteres.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o número de celular do usuário.
    /// O celular deve estar no formato (00) 00000-0000.
    /// </summary>
    [Required(ErrorMessage = "O celular é obrigatório.")]
    [RegularExpression(@"^\(\d{2}\) \d{5}-\d{4}$", ErrorMessage = "O celular deve estar no formato (00) 00000-0000.")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o servidor foi aprovado pelo administrador para utilizar o sistema.
    /// Só será verdadeiro quando o administrador aprovar o servidor.
    /// </summary>
    [Required(ErrorMessage = "O servidor precisa ser aprovado para acessar o sistema")]
    public bool IsAprovado { get; set; } = false;

    /// <summary>
    /// Obtém ou define a senha do usuário.
    /// A senha deve ter no mínimo 8 caracteres e no máximo 120 caracteres.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(120, ErrorMessage = "A senha deve ter no máximo 120 caracteres.")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a confirmação de senha do usuário.
    /// A confirmação de senha deve ser igual à senha fornecida em "Password".
    /// </summary>
    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public string RePassword { get; set; } = string.Empty;
}