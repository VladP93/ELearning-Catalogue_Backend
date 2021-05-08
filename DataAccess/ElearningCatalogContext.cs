using Microsoft.EntityFrameworkCore;
using Business;

namespace DataAccess
{
    public class ElearningCatalogContext : DbContext
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
            modelBuilder.Entity<CourseInstructor>().HasKey(ci => new {ci.CourseId, ci.InstructorId});
        }
    }
}