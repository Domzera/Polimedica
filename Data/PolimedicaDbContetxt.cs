using Polimedica.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Polimedica.Data
{
    public class PolimedicaDbContetxt : IdentityDbContext<Pessoa>
    {
        public PolimedicaDbContetxt(DbContextOptions<PolimedicaDbContetxt> options) : base(options) { }

        public DbSet<Roteiro> Roteiros { get; set;}
    }
}
