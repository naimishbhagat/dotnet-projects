using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelsApp.Models;

namespace ModelApp.Data
{
    public class DataContextEF : DbContext
    {
        public DbSet<Computer>? Computer { get; set;}
        private IConfiguration _config;
        private string _connectionString = "";

        public DataContextEF(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(_connectionString,
                    options=>options.EnableRetryOnFailure());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            modelBuilder.Entity<Computer>()
                //.HasNoKey()
                .HasKey(c => c.ComputerId);
                //.ToTable("Computer", "TutorialAppSchema");
        }
    }
};