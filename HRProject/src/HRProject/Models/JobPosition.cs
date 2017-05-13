using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Models
{
    public class JobPosition
    {
        [Key]
        [Required]
        public int JobId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PartTime_FullTime { get; set; }
        public string Keywords { get; set; }

        public virtual ICollection<UserInJobs> UserInJobs { get; set; }

        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        //public ICollection<User> UsersApplied { get; set; }
    }
}
