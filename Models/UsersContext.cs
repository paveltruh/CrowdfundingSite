using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UsersContext : IdentityDbContext<User>
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Donation> Donations { get; set; }
    }
}
