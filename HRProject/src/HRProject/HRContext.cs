using HRProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject
{
    public class HRContext : DbContext
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Company> Companys { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
