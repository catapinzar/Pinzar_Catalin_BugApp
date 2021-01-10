using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pinzar_Catalin_BugApp.Data;
using Pinzar_Catalin_BugApp.Models;

namespace Pinzar_Catalin_BugApp.Controllers
{
    public class BugsController : Controller
    {
        private readonly VerificationContext _context;

        public BugsController(VerificationContext context)
        {
            _context = context;
        }

        // GET: Bugs
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SeveritySortParm"] = sortOrder == "Severity" ? "severity_desc" : "Severity";
            ViewData["CurrentFilter"] = searchString;

            var bugs = from bug in _context.Bugs
                        select bug;

            if(!String.IsNullOrEmpty(searchString))
            {
                bugs = bugs.Where(bug => bug.Name.Contains(searchString));
            }

            bugs = sortOrder switch
            {
                "name_desc" => bugs.OrderByDescending(bug => bug.Name),
                "Severity" => bugs.OrderBy(bug => bug.Severity),
                "severity_desc" => bugs.OrderByDescending(bug => bug.Severity),
                _ => bugs.OrderBy(bug => bug.Name),
            };
            return View(await bugs.AsNoTracking().ToListAsync());
        }

        // GET: Bugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs
                .Include(app => app.Apps)
                .ThenInclude(employee => employee.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }

        // GET: Bugs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Severity,Status")] Bug bug)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(bug);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists ");
            }
            return View(bug);
        }

        // GET: Bugs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs.FindAsync(id);
            if (bug == null)
            {
                return NotFound();
            }
            return View(bug);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bugToUpdate = await _context.Bugs.FirstOrDefaultAsync(bug => bug.ID == id);
            if (await TryUpdateModelAsync<Bug>(
            bugToUpdate,
            "",
            bug => bug.Name, bug => bug.Description, bug => bug.Severity, bug => bug.Status))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(bugToUpdate);
        }

        // GET: Bugs/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (bug == null)
            {
                return NotFound();
            }

            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again.";
            }

            return View(bug);
        }

        // POST: Bugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bug = await _context.Bugs.FindAsync(id);
            if(bug == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Bugs.Remove(bug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.ID == id);
        }
    }
}
