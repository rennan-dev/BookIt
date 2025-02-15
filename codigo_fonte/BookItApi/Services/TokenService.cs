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
    /// Gera um token JWT para o administrador do sistema.
    /// </summary>
    /// <param name="admin">O administrador que está sendo autenticado.</param>
    /// <returns>Retorna um token JWT assinado e válido.</returns>
    public string GenerateToken(Admin admin) {
        Claim[] claims = new Claim[]  {
            new Claim(ClaimTypes.Name, admin.Name), 
            new Claim(ClaimTypes.NameIdentifier, admin.Id) 
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

    /// <summary>
    /// Gera um token JWT para o servidor.
    /// </summary>
    /// <param name="servidor">O servidor que está sendo autenticado.</param>
    /// <returns>Retorna um token JWT assinado e válido.</returns>
    public string GenerateToken(Servidor servidor) {
        Claim[] claims = new Claim[]  {
            new Claim(ClaimTypes.Name, servidor.Name), 
            new Claim(ClaimTypes.NameIdentifier, servidor.Id), 
            new Claim("IsAprovado", servidor.IsAprovado.ToString()) 
        };

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("2323R02N902FI03908N038J31093N10ND2049NASIDPOM0J923") 
        );

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