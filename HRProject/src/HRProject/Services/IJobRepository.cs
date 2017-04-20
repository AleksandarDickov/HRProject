using HRProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Services
{
    interface IJobRepository
    {
        IEnumerable<JobPosition> GetJobs();
        Company GetCompany(int jobId);
        Company GetCompany(int id, bool includeCompany);
        void AddJob(JobPosition job);
        void UpdateJob(JobPosition job);
        JobPosition Find(int id);
        void Remove(int id);
    }
}
