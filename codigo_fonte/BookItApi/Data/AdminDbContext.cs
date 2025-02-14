using BookItApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookItApi.Data;

/// <summary>
/// Representa o contexto de dados para a aplicação, que gerencia as entidades de admin e servidor, 
/// além das tabelas de autenticação do Identity.
/// </summary>
public class AdminDbContext : IdentityDbContext<Admin>{

    /// <summary>
    /// Inicializa uma nova instância do contexto, configurando as opções passadas.
    /// </summary>
    /// <param name="opts">As opções de configuração do contexto do banco de dados.</param>    
    public AdminDbContext(DbContextOptions<AdminDbContext> opts) : base(opts) { }

    /// <summary>
    /// Representa a tabela de servidores no banco de dados.
    /// </summary>
    public DbSet<Servidor> Servidores { get; set; }
}