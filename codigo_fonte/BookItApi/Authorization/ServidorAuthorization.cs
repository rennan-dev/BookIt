using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BookItApi.Authorization;

/// <summary>
/// Handler de autorização para verificar se o usuário é um administrador.
/// </summary>
public class ServidorAuthorization : AuthorizationHandler<ServidorOnly> {

    /// <summary>
    /// Processa o requisito de autorização para verificar se o usuário tem a claim 'IsAprovado' e 'IsAdmin' como 'True'.
    /// </summary>
    /// <param name="context">O contexto da autorização, que contém o usuário e as informações necessárias para autorização.</param>
    /// <param name="requirement">O requisito de autorização que define o critério para a autorização.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ServidorOnly requirement) {
        var isAdminClaim = context.User.FindFirst(claim => claim.Type == "IsAdmin");
        var isAprovadoClaim = context.User.FindFirst(claim => claim.Type == "IsAprovado");

        if(isAdminClaim == null || isAdminClaim.Value != "False") {
            //se a claim não existir ou não for 'False', a autorização falha
            return Task.CompletedTask;
        }
        if(isAprovadoClaim == null || isAprovadoClaim.Value != "True") {
            return Task.CompletedTask;
        }

        //se a claim for válida e o usuário for servidor e aprovado, sinaliza que ele passou na autorização
        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
