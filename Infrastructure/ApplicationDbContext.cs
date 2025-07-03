using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            Database.Migrate();
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any custom configuration here if needed
        }
    }
} 