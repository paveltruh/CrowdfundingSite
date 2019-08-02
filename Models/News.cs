using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class News
    {
        public int Id { get; set; }

        public string Heading { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
    