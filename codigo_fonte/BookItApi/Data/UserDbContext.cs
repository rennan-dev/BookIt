using BookItApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookItApi.Data;

/// <summary>
/// Representa o contexto de dados para a aplicação, que gerencia as entidades de admin e servidor, 
/// além das tabelas de autenticação do Identity.
/// </summary>
public class UserDbContext : IdentityDbContext<User>{

    /// <summary>
    /// Inicializa uma nova instância do contexto, configurando as opções passadas.
    /// </summary>
    /// <param name="opts">As opções de configuração do contexto do banco de dados.</param>    
    public UserDbContext(DbContextOptions<UserDbContext> opts) : base(opts) { }
}