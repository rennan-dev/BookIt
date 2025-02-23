using System;
using System.ComponentModel.DataAnnotations;

namespace BookItApi.Dtos.User;

/// <summary>
/// Dto para criar uma nova reserva. Usado para transferir os dados necessários para a criação de uma reserva.
/// </summary>
public class CreateReservaDto {

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
    /// </summary>
    [Required]
    public string Horarios { get; set; } = string.Empty;
}