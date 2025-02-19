using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BookItApi.Models;

namespace BookItApi.Services;

/// <summary>
/// Serviço responsável pela geração de tokens JWT para autenticação de Admin ou Servidor.
/// </summary>
public class TokenService  {

    /// <summary>
    /// Gera um token JWT para o usuário do sistema.
    /// </summary>
    /// <param name="user">O usuário que está sendo autenticado.</param>
    /// <returns>Retorna um token JWT assinado e válido.</returns>
    public string GenerateToken(User user) {
        Claim[] claims = new Claim[]  {
            new Claim(ClaimTypes.Name, user.Name), 
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("IsAdmin", user.IsAdmin ? "True" : "False"),
            new Claim("IsAprovado", user.IsAprovado.ToString()), 
        };

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("2323R02N902FI03908N038J31093N10ND2049NASIDPOM0J923") 
        );

        // Define as credenciais de assinatura com o algoritmo HMACSHA256
        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        //cria o token JWT
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(1), //o token expira em 1 hora
            claims: claims, 
            signingCredentials: signingCredentials 
        );

        //retorna o token JWT como uma string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}