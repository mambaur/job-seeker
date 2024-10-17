using JobSeeker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobSeeker.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<JobPosition> JobPositions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobCategory> JobCategories { get; set; }

        public DbSet<SeekerProfile> SeekerProfiles { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<SeekerProfileExperience> SeekerProfileExperiences { get; set; }

        public DbSet<Applicant> Applicants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mengabaikan entitas ErrorViewModel agar tidak dimigrasikan
            modelBuilder.Ignore<ErrorViewModel>();
        }
    }
}