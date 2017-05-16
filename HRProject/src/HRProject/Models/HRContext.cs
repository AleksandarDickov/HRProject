using HRProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRProject
{
    public class HRContext : IdentityDbContext<User>
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Company> Companys { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<User> RegUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserInJobs>()
                .HasKey(uIj => new { uIj.JobId, uIj.UserId });

            modelBuilder.Entity<UserInJobs>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserInJobs)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserInJobs>()
                .HasOne(bc => bc.JobPosition)
                .WithMany(c => c.UserInJobs)
                .HasForeignKey(bc => bc.JobId);
        }
    }
}
