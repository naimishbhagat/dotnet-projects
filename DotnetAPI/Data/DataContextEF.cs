using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        private IConfiguration _config;
   
        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<User> Users {get; set;}
        public virtual DbSet<UserSalary> UserSalary {get; set;}
        public virtual DbSet<UserJobInfo> UserJobInfo {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    options=>options.EnableRetryOnFailure());
            
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<User>()
                .ToTable("Users","TutorialAppSchema")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UserSalary>()
                .HasKey(u => u.UserId);
            
            modelBuilder.Entity<UserJobInfo>()
                .HasKey(u => u.UserId);
        }

    }
       
};