using HRProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRProject.Controllers;

namespace HRProject.Services
{
    interface IJobRepository
    {
        IEnumerable<JobPosition> GetJobs();
        JobPosition GetJob(int jobId);
        void AddJob(JobPosition job);
        void UpdateJob(JobPosition job);
        JobPosition Find(int id);
        void Remove(int id);
    }
}
