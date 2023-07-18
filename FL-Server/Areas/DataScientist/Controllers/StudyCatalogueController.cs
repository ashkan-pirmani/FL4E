using FL_Server.Data;
using FL_Server.Models;
using FL_Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FL_Server.Areas.DataScientist.Controllers
{
    [Area("DataScientist")]
    [Authorize]
    public class StudyCatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudyCatalogueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudyCatalogues
        public async Task<IActionResult> Index()
        {
            var viewmodel = new HomeViewModel
            {
                studyCatalogueList = await _context.StudyCatalogue.ToListAsync(),
                dataCoreFileOnSystemModel = await _context.DataCoreFileOnSystem.ToListAsync(),
            };
            return View(viewmodel);
        }

        // GET: StudyCatalogues/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var idd = await _context.StudyCatalogue.FirstOrDefaultAsync(m => m.Id == id);

            if (idd.Status == true | User.IsInRole(SD.AdminUser))
            {

                if (id == null)
                {
                    return NotFound();
                }

                var studyCatalogue = await _context.StudyCatalogue
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (studyCatalogue == null)
                {
                    return NotFound();
                }
                var viewModel = new HomeViewModel
                {
                    StudyCatalogue = studyCatalogue,
                    DataCoreFileOnSystemModel = _context.DataCoreFileOnSystem.SingleOrDefault(m => m.StudyCatalogueId == id),
                    dataCoreFileOnSystemModel = _context.DataCoreFileOnSystem.ToList()
                };


                return View(viewModel);

            }

            else
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }

            
        }

        // GET: StudyCatalogues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudyCatalogues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StudyLead,Status,StartDate")] StudyCatalogue studyCatalogue)
        {
            if (ModelState.IsValid)
            {
                studyCatalogue.StartDate = DateTime.UtcNow;
                _context.Add(studyCatalogue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studyCatalogue);
        }

        // GET: StudyCatalogues/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyCatalogue = await _context.StudyCatalogue.FindAsync(id);
            if (studyCatalogue == null)
            {
                return NotFound();
            }
            return View(studyCatalogue);
        }

        // POST: StudyCatalogues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StudyLead,Status,StartDate")] StudyCatalogue studyCatalogue)
        {
            if (id != studyCatalogue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyCatalogue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyCatalogueExists(studyCatalogue.Id))
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
            return View(studyCatalogue);
        }

        // GET: StudyCatalogues/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyCatalogue = await _context.StudyCatalogue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studyCatalogue == null)
            {
                return NotFound();
            }

            return View(studyCatalogue);
        }

        // POST: StudyCatalogues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studyCatalogue = await _context.StudyCatalogue.FindAsync(id);
            _context.StudyCatalogue.Remove(studyCatalogue);

            var filemodelstudycatalogue = await _context.FilesOnDatabase.Where(m => m.StudyCatalogueId == id).ToListAsync();
            _context.FilesOnDatabase.RemoveRange(filemodelstudycatalogue);
            var filemodelstudycenter = await _context.StudyCenter.Where(m => m.StudyCatalogueId == id).ToArrayAsync();
            _context.StudyCenter.RemoveRange(filemodelstudycenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyCatalogueExists(int id)
        {
            return _context.StudyCatalogue.Any(e => e.Id == id);
        }
    }
}
