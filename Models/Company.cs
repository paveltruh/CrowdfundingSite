using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Foto { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        public int TargetAmount { get; set; }
        public int CollectedAmount { get; set; } = 0;
        public CompanyCategory Category { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }
    public enum CompanyCategory
    {
        Arts,
        Comics,
        Crafts,
        Dance,
        Design,
        Fashion,
        FilmAndVideo,
        Food,
        Games,
        Journalism,
        Music,
        Photografy,
        Publishing,
        Technology,
        Theater
    }
}
