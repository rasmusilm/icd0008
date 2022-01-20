using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class BSDBContext : DbContext
    {
        private static string ConnectionString =
            "Server=barrel.itcollege.ee;User Id=student;Password=Student.Pass.1;Database=student_railmj_bs_real;MultipleActiveResultSets=true";

        public DbSet<BrainDbDTO> Games { get; set; } = default!;

        public DbSet<PlayerDbDTO> Players { get; set; } = default!;

        public DbSet<GameConfigDbDTO> Configurations { get; set; } = default!;

        public DbSet<Settings> Settings { get; set; } = default!;

        public DbSet<MoveHistoryDTO> History { get; set; } = default!;
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