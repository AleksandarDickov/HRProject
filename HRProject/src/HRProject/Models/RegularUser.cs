using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Models
{
    public class RegularUser
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
        public string SurName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PartTime_FullTime { get; set; }
        public string WorkExperience { get; set; }

        //  public JobPosition Keywords { get; set; }
    }
}
