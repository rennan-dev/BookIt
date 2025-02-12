using docente_tecnico_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace docente_tecnico_api.Data;

public class UserContext : IdentityDbContext<User> {

    public UserContext(DbContextOptions<UserContext> opts) : base(opts) {
        
    }
}