using Microsoft.EntityFrameworkCore;

namespace practicecrudmvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }
    }
}
