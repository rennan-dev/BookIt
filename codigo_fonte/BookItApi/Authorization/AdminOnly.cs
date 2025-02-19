using Microsoft.AspNetCore.Authorization;

namespace BookItApi.Authorization;

/// <summary>
/// Requisito de autorização que verifica se o usuário é um administrador.
/// </summary>
public class AdminOnly : IAuthorizationRequirement {

    /// <summary>
    /// Construtor para definir o requisito de ser admin.
    /// </summary>
    /// <param name="isAdmin">Indica se o usuário deve ser um administrador.</param>
    public AdminOnly(bool isAdmin) {
        IsAdmin = isAdmin;
    }

    /// <summary>
    /// Indica se o requisito é que o usuário seja um administrador.
    /// </summary>
    public bool IsAdmin { get; set; }
}