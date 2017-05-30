using HRProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject
{
    public class UserInJobs
    {
        public int JobId { get; set; }
     
        public string UserId { get; set; }
        
        public virtual JobPosition JobPosition { get; set; }
        
        public virtual User User { get; set; }
    }
}
