using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using EmployeeManagementApp.Data;

namespace EmployeeManagementApp.Controllers
{
    public class JobpositionModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobpositionModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobpositionModel
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobPositions.Include(j => j.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobpositionModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobpositionModel = await _context.JobPositions
                .Include(j => j.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobpositionModel == null)
            {
                return NotFound();
            }

            return View(jobpositionModel);
        }

        // GET: JobpositionModel/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: JobpositionModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobName,StartDate,EndDate,Salary,IsActive,EmployeeId")] JobpositionModel jobpositionModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobpositionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", jobpositionModel.EmployeeId);
            return View(jobpositionModel);
        }

        // GET: JobpositionModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobpositionModel = await _context.JobPositions.FindAsync(id);
            if (jobpositionModel == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", jobpositionModel.EmployeeId);
            return View(jobpositionModel);
        }

        // POST: JobpositionModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobName,StartDate,EndDate,Salary,IsActive,EmployeeId")] JobpositionModel jobpositionModel)
        {
            if (id != jobpositionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobpositionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobpositionModelExists(jobpositionModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", jobpositionModel.EmployeeId);
            return View(jobpositionModel);
        }

        // GET: JobpositionModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobpositionModel = await _context.JobPositions
                .Include(j => j.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobpositionModel == null)
            {
                return NotFound();
            }

            return View(jobpositionModel);
        }

        // POST: JobpositionModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobpositionModel = await _context.JobPositions.FindAsync(id);
            _context.JobPositions.Remove(jobpositionModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobpositionModelExists(int id)
        {
            return _context.JobPositions.Any(e => e.Id == id);
        }
    }
}
