using System.ComponentModel.DataAnnotations;

namespace BookItApi.Models;

/// <summary>
/// Modelo para armazenar as informações de uma reserva
/// </summary>
public class Reserva {

    /// <summary>
    /// Id para identificar único de uma reserva
    /// </summary>
    [Key][Required] public int Id { get; set; }

    /// <summary>
    /// Tipo da reserva (auditório, mini auditório, sala de reunião, veículos, etc.)
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "O tipo de reserva não pode ter mais de 50 caracteres.")]
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// Data da reserva
    /// </summary>
    [Required]
    public DateTime DataReserva { get; set; }

    /// <summary>
    /// Horários da reserva (mais de um horário pode ser selecionado)
    /// Armazenar no formato: "08:00,09:00,10:00")
    /// </summary>
    [Required]
    public string Horarios { get; set; } = string.Empty;

    /// <summary>
    /// Usuário responsável pela reserva
    /// </summary>
    [Required]
    public string UsuarioId { get; set; } = string.Empty;

    /// <summary>
    /// Navegação para o usuário que fez a reserva.
    /// </summary>
    public virtual User? Usuario { get; set; }
}