using DataAccessLeyer;
using Microsoft.EntityFrameworkCore;

namespace technology_kida.data
{
    public class applicationDbContext : DbContext
    {
        public applicationDbContext(DbContextOptions<applicationDbContext> options) : base(options) { }

        public DbSet<employee> employees { get; set; }
    }
}
