using HRProject.Models;
using HRProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Controllers
{
    [Route("api/job")]
    public class JobController : Controller
    {
        private IJobRepository _jobRepository;
        private HRContext _ctx;

        public JobController(HRContext ctx)
        {
            _ctx = ctx;
            _jobRepository = new JobRepository(ctx);
        }

        [HttpGet("GetJobs")]
        public IActionResult GetJobs()
        {
            var jobEntity = _jobRepository.GetJobs();



            return Ok(jobEntity);
        }

        [HttpPost]
        public IActionResult AddJob([FromBody] JobPosition job)
        {
            if (job == null)
            {
                return BadRequest();
            }
            try
            {
                _jobRepository.AddJob(job);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok("Sve je u redu");
        }

        [HttpPut("{jobId}")]
        public IActionResult Update(int jobId, [FromBody] JobPosition updateJob)
        {
            if (updateJob == null || updateJob.JobId != jobId)
            {
                return BadRequest();
            }

            var newJob = _jobRepository.GetJob(jobId);
            if (newJob == null)
            {
                return NotFound();
            }

            newJob.Name = updateJob.Name;
            newJob.Country = updateJob.Country;
            newJob.Description = updateJob.Description;
            newJob.City = updateJob.City;
            newJob.JobId = updateJob.JobId;
            newJob.PartTime_FullTime = updateJob.PartTime_FullTime;
            newJob.Keywords = updateJob.Keywords;

            _jobRepository.UpdateJob(newJob);
            return new NoContentResult();
        }

        [HttpDelete("{jobId}")]
        public IActionResult Delete(int jobId)
        {
            var job = _jobRepository.Find(jobId);
            if (job == null)
            {
                return NotFound();
            }

            _jobRepository.Remove(jobId);
            return new NoContentResult();
        }

        [HttpGet("{jobId}")]
        public IActionResult GetJob(int jobId)
        {
            var job = _jobRepository.GetJob(jobId);

            if (job == null)
            {
                return NotFound();
            }

            _jobRepository.GetJob(jobId);

            return Ok(job);
        }

        [HttpGet("bonus")]
        public IActionResult SearchAndSort([FromQuery]string searchString, [FromQuery] string sortBy, [FromQuery] int page, [FromQuery] int jobsPerPage = 3)
        {
            var job = from p in _ctx.JobPositions
                          select p;

            if (searchString != null)
            {
                job = job.Where(p => p.Name.Contains(searchString)
                                        || p.City.Contains(searchString));
            }

            if (sortBy == "Descending")
            {
                job = job.OrderByDescending(p => p.Name);
            }
            else if (sortBy == "Ascending")
            {
                job = job.OrderBy(p => p.Name);
            }

            if (page > 0)
            {
                job = job.Skip((page - 1) * jobsPerPage).Take(jobsPerPage);
            }

            return Ok(job);
        }
    }
}
