using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public ICollection<Company> Companies { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }
}
