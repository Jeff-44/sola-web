using ApplicationCore.Models;
using ApplicationCore.Settings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SolaContext: IdentityDbContext<User>
    {
        public SolaContext(DbContextOptions<SolaContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMedia> ProjectMedia { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Inquiry> Inquiries { get; set; }

    }
}
