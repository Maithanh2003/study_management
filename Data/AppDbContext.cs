using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassSubject> ClassSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map enum to string
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Class>().ToTable("classes");
            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<ClassSubject>().HasKey(cs => new { cs.ClassId, cs.SubjectId });
        }
    }
}
