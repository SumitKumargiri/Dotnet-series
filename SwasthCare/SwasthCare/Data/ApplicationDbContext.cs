using Microsoft.EntityFrameworkCore;
using SwasthCare.Models;
using System.Collections.Generic;

namespace SwasthCare.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
