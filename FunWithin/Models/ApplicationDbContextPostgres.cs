using Microsoft.EntityFrameworkCore;

namespace FunWithin.Models
{
    public class ApplicationDbContextPostgres : DbContext
    {
        public ApplicationDbContextPostgres(DbContextOptions<ApplicationDbContextPostgres> options)
            : base(options) { }
        public DbSet<Review> Reviews { get; set; }
    }
}
