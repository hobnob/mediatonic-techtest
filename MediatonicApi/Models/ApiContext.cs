using Microsoft.EntityFrameworkCore;

namespace MediatonicApi.Models
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<UserAnimal> UserAnimals { get; set; }

        public ApiContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAnimal>().HasKey(ua => new { ua.UserId, ua.AnimalId });
            modelBuilder.Entity<UserAnimal>().Property("lastHungerUpdate");
            modelBuilder.Entity<UserAnimal>().Property("lastHappinessUpdate");
            modelBuilder.Entity<UserAnimal>().Property("hungerAtUpdate");
            modelBuilder.Entity<UserAnimal>().Property("happinessAtUpdate");

            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Animal>().Property(a => a.Id).ValueGeneratedOnAdd();
        }
    }
}
