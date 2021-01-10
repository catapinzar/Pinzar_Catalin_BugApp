using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pinzar_Catalin_BugApp.Models;

namespace Pinzar_Catalin_BugApp.Data
{
    public class VerificationContext:DbContext
    {
        public VerificationContext(DbContextOptions<VerificationContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<Bug> Bugs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<App>().ToTable("App");
            modelBuilder.Entity<Bug>().ToTable("Bug");
        }
    }
}
