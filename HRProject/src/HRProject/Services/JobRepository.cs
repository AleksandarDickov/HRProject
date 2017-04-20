using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRProject.Models;

namespace HRProject.Services
{
    public class JobRepository : IJobRepository
    {
        private HRContext _context;

        public JobRepository(HRContext context)
        {
            _context = context;
        }

        public void AddJob(JobPosition job)
        {
            _context.JobPositions.Add(job);
            _context.SaveChanges();
        }

        public JobPosition Find(int id)
        {
            return _context.JobPositions.FirstOrDefault(t => t.JobId == id);
        }

        public JobPosition GetJob(int jobId)
        {
            return _context.JobPositions.Where(c => c.JobId == jobId).FirstOrDefault();
        }

        public JobPosition GetJob(int id, bool includeCompany)
        {
            if (includeCompany)
            {
                return _context.JobPositions.Include(c => c.)
                    .Where(c => c.CompanyId == id).FirstOrDefault();
            }
            return _context.Companys.Where(c => c.CompanyId == id).FirstOrDefault();
        }

        public IEnumerable<JobPosition> GetJobs()
        {
            return _context.JobPositions.OrderBy(c => c.Name).ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateJob(JobPosition job)
        {
            throw new NotImplementedException();
        }
    }
}
