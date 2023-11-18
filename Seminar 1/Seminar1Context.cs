using Microsoft.EntityFrameworkCore;
using Seminar_1.Models.Entities;

namespace Seminar_1
{
    public class Seminar1Context : DbContext
    {
        public Seminar1Context(DbContextOptions<Seminar1Context> options)
            : base(options) 
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
