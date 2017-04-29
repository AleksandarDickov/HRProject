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
    }
}
