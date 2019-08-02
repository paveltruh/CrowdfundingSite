using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateNewsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Heading { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

    }
}
