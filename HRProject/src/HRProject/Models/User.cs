using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PartTime_FullTime { get; set; }
        public string WorkExperience { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status StatusOfUser { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string NoteField { get; set; }
        [NotMapped]
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<UserInJobs> UserInJobs { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual ICollection<JobPosition> CreatedJobs { get; set; }

        //public ICollection<JobPosition> AppliedJobs { get; set; }

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        available,
        assigned,
        frozen
    }
}
