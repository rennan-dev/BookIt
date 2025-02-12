using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace docente_tecnico_api.Models;

public class User : IdentityUser {

    [Required(ErrorMessage = "O SIAPE é obrigatório.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "O SIAPE deve ter exatamente 6 dígitos numéricos.")]
    public string Siape { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 000.000.000-00.")]
    public string Cpf { get; set; } = string.Empty;
}