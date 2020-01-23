using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Donation> Donations { get; set; }
        //public DbSet<Comment> Comments { get; set; }
    }
}
