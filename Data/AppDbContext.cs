using JobSeeker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobSeeker.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<JobPosition> JobPositions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mengabaikan entitas ErrorViewModel agar tidak dimigrasikan
            modelBuilder.Ignore<ErrorViewModel>();
        }
    }
}