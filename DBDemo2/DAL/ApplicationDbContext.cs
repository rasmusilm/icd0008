
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        
        private static string ConnectionString =
            "Server=barrel.itcollege.ee;User Id=student;Password=Student.Pass.1;Database=student_railmj_dbdemo1;MultipleActiveResultSets=true";
        
        
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<CourseDeclaration>? CourseDeclarations { get; set; } = default!;
        public DbSet<Grade>? Grades { get; set; } = default!;
        public DbSet<Homework>? Homeworks { get; set; } = default!;
        public DbSet<Person>? Persons { get; set; } = default!;

        // not recommended - do not hardcode DB conf!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => !e.IsOwned())
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}