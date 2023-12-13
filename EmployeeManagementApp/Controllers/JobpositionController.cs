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
    public class JobpositionController : Controller
    {
        private readonly ApplicationDbContext _context;
        public JobpositionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jobposition
        public async Task<IActionResult> Index()
        {
            ModelState.Clear();
            var applicationDbContext = _context.JobPositions.Include(j => j.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Jobposition/Details/5
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

        // GET: Jobposition/Create
        public IActionResult Create(int id)
        {
            ViewData["EmployeeId"] = id;
            return View();
        }


        // Custom validation method
        private bool EmployeeHasActiveJob(int employeeId)
        {
            return _context.JobPositions.Any(j => j.EmployeeId == employeeId && j.IsActive);
        }

        // POST: Jobposition/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobName,StartDate,EndDate,Salary,IsActive,EmployeeId")] JobpositionModel jobpositionModel)
        {
            if (ModelState.IsValid)
            {
                // Custom validation: Check if the employee already has an active job position
                if (EmployeeHasActiveJob(jobpositionModel.EmployeeId) && jobpositionModel.IsActive)
                {
                    ViewData["EmployeeId"] = jobpositionModel.EmployeeId;
                    ModelState.AddModelError("IsActive", "Employee already has an active job position.");
                    return View(jobpositionModel);
                }

                _context.Add(jobpositionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Employee", new { id = jobpositionModel.EmployeeId });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", jobpositionModel.EmployeeId);
            return View(jobpositionModel);
        }

        // GET: Jobposition/Edit/5
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

            var employeeModel = await _context.Employees
                .Include(e => e.JobPositions)
                .FirstOrDefaultAsync(m => m.Id == jobpositionModel.EmployeeId);
            ViewData["EmployeeFullname"] = employeeModel.FirstName + " " + employeeModel.MiddleName + " " + employeeModel.LastName;
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", jobpositionModel.EmployeeId);
            return View(jobpositionModel);
        }

        // POST: Jobposition/Edit/5
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

            if (EmployeeHasActiveJob(jobpositionModel.EmployeeId) && jobpositionModel.IsActive)
            {
                ViewData["EmployeeId"] = jobpositionModel.EmployeeId;
                ModelState.AddModelError("IsActive", "Employee already has an active job position.");
                return View(jobpositionModel);
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
                return RedirectToAction("Details", "Employee", new { id = jobpositionModel.EmployeeId });
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", jobpositionModel.EmployeeId);
            return View(jobpositionModel);
        }

        // GET: Jobposition/Delete/5
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

        // POST: Jobposition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobpositionModel = await _context.JobPositions.FindAsync(id);
            _context.JobPositions.Remove(jobpositionModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Employee", new { id = jobpositionModel.EmployeeId });
        }

        private bool JobpositionModelExists(int id)
        {
            return _context.JobPositions.Any(e => e.Id == id);
        }
    }
}
