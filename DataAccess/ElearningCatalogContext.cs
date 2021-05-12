using Business;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ElearningCatalogContext : IdentityDbContext<User>
    {
        public DbSet<Commentary> Commentary { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseInstructor> CourseInstructor { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Price> Price { get; set; }
        
        public ElearningCatalogContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CourseInstructor>().HasKey(ci => new {ci.CourseId, ci.InstructorId});
        }
    }
}