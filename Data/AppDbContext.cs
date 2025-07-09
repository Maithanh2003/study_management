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
            // modelBuilder.Entity<User>().ToTable("users");
            // modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Class>().ToTable("classes");
            // modelBuilder.Entity<Subject>().ToTable("subjects");
            // modelBuilder.Entity<ClassSubject>().ToTable("classsubjects");

            modelBuilder.Entity<ClassSubject>().HasKey(cs => new { cs.ClassId, cs.SubjectId });

            // modelBuilder.Entity<Student>()
            //     .HasOne(s => s.CurrentClass)
            //     .WithMany()
            //     .HasForeignKey(s => s.CurrentClassId)
            //     .OnDelete(DeleteBehavior.SetNull);

            // modelBuilder.Entity<Student>()
            //     .HasOne(s => s.User)
            //     .WithMany()
            //     .HasForeignKey(s => s.UserId)
            //     .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
