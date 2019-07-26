using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateCompanyViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Foto{ get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime Deadline { get; set; }
        public int TargetAmount { get; set; }
    }
}
