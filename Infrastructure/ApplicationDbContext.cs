using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Design;

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

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<Infrastructure.ApplicationDbContext>
    {
        public Infrastructure.ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Infrastructure.ApplicationDbContext>();
            // Use your actual connection string or a placeholder
            optionsBuilder.UseNpgsql("Host=localhost;Database=PredictorsScores;Username=postgres;Password=yourpassword");
            return new Infrastructure.ApplicationDbContext(optionsBuilder.Options);
        }
    }
} 