using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CommentContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
    }
}
