using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public int AmountOfDonation { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
