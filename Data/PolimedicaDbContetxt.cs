using Polimedica.Models;
using Microsoft.EntityFrameworkCore;

namespace Polimedica.Data
{
    public class PolimedicaDbContetxt : DbContext
    {
        public PolimedicaDbContetxt(DbContextOptions<PolimedicaDbContetxt> options) : base(options) { }

        public DbSet<Roteiro> Roteiros { get; set;}
    }
}
