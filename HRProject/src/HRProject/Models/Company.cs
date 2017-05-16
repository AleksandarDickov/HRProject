using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Models
{
    public class Company
    {
        [Key]
        [Required]
        public int CompanyId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Phone { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }

         public ICollection<JobPosition> Jobs { get; set; }
    }
}
