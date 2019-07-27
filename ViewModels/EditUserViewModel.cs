using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
