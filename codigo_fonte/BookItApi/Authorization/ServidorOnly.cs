using Microsoft.AspNetCore.Authorization;

namespace BookItApi.Authorization;

/// <summary>
/// Requisito de autorização que verifica se o usuário é um servidor.
/// </summary>
public class ServidorOnly : IAuthorizationRequirement {

    /// <summary>
    /// Construtor para definir o requisito de ser admin.
    /// </summary>
    /// <param name="isAdmin">Indica se o usuário deve ser um administrador.</param>
    /// <param name="isAprovado">Indica se o usuário está aprovado pelo admin ou não</param>
    public ServidorOnly(bool isAdmin, bool isAprovado) {
        IsAdmin = isAdmin;
        IsAprovado = isAprovado;
    }

    /// <summary>
    /// Indica se o requisito é que o usuário seja um administrador.
    /// </summary>
    public bool IsAdmin { get; set; }
    
    /// <summary>
    /// Verifica se ele está aprovado
    /// </summary>
    public bool IsAprovado { get; set; }
}