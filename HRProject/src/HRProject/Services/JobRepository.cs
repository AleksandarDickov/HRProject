using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRProject.Models;
using Microsoft.EntityFrameworkCore;

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
                return _context.JobPositions.Include(c => c.Companies)
                    .Where(c => c.JobId == id).FirstOrDefault();
            }
            return _context.JobPositions.Where(c => c.JobId == id).FirstOrDefault();
        }

        public IEnumerable<JobPosition> GetJobs()
        {
            return _context.JobPositions.OrderBy(c => c.Name).ToList();
        }

        public void Remove(int id)
        {
            var entity = _context.JobPositions.First(t => t.JobId == id);
            _context.JobPositions.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateJob(JobPosition job)
        {
            _context.JobPositions.Update(job);
            _context.SaveChanges();
        }
    }
}
