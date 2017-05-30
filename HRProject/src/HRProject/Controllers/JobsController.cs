using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRProject;
using HRProject.Models;

namespace HRProject.Controllers
{
    public class JobsController : Controller
    {
        private readonly HRContext _context;

        public JobsController(HRContext context)
        {
            _context = context;    
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var hRContext = _context.JobPositions.Include(j => j.CreatedBy);
            return View(await hRContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosition = await _context.JobPositions
                .Include(j => j.CreatedBy)
                .SingleOrDefaultAsync(m => m.JobId == id);
            if (jobPosition == null)
            {
                return NotFound();
            }

            return View(jobPosition);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.RegUsers, "Id", "Id");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,Name,Description,City,Country,PartTime_FullTime,Keywords,CreatedById")] JobPosition jobPosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobPosition);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CreatedById"] = new SelectList(_context.RegUsers, "Id", "Id", jobPosition.CreatedById);
            return View(jobPosition);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosition = await _context.JobPositions.SingleOrDefaultAsync(m => m.JobId == id);
            if (jobPosition == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.RegUsers, "Id", "Id", jobPosition.CreatedById);
            return View(jobPosition);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobId,Name,Description,City,Country,PartTime_FullTime,Keywords,CreatedById")] JobPosition jobPosition)
        {
            if (id != jobPosition.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobPosition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobPositionExists(jobPosition.JobId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CreatedById"] = new SelectList(_context.RegUsers, "Id", "Id", jobPosition.CreatedById);
            return View(jobPosition);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosition = await _context.JobPositions
                .Include(j => j.CreatedBy)
                .SingleOrDefaultAsync(m => m.JobId == id);
            if (jobPosition == null)
            {
                return NotFound();
            }

            return View(jobPosition);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobPosition = await _context.JobPositions.SingleOrDefaultAsync(m => m.JobId == id);
            _context.JobPositions.Remove(jobPosition);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool JobPositionExists(int id)
        {
            return _context.JobPositions.Any(e => e.JobId == id);
        }
    }
}
