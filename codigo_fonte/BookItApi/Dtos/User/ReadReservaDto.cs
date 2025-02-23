namespace BookItApi.Dtos.User;

/// <summary>
/// Leitura das reservas
/// </summary>
public class ReadReservaDto {

    /// <summary>
    /// identificador das reservas
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// tipo das reservas
    /// </summary>
    public string Tipo { get; set; } = string.Empty;
    
    /// <summary>
    /// dia da reservas
    /// </summary>
    public DateTime DataReserva { get; set; }
    
    /// <summary>
   /// Horários das reservas
    /// </summary>
    public string Horarios { get; set; } = string.Empty;
}