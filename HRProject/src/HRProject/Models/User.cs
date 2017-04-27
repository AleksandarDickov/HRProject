using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Models
{
    public class User : IdentityUser
    {
        //[Key]
        //[Required]
        //public int UserId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PartTime_FullTime { get; set; }
        public string WorkExperience { get; set; }
        public string Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string NoteField { get; set; }
        
        public ICollection<JobPosition> Keywords { get; set; }

        public UserTipe user { get; set; }
    }

    public enum UserTipe
    {
        SuperUser,
        RegularUser,
        HrManager,
    }
}
