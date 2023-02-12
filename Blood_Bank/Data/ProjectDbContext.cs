using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blood_Bank.Models;

namespace Blood_Bank.Data
{
    public class ProjectDbContext: DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        public DbSet<User>? User { get; set; }
        public DbSet<UserRoles>? UserRoles { get; set; }
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<BloodGroup>? BloodGroup { get; set; }
    }
}
