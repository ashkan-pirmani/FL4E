using FL_Server.Data;
using FL_Server.Models.Models;
using FL_Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FL_Server.Areas.DataScientist.Controllers
{
    [Area("DataScientist")]
    [Authorize]
    public class ModelCatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModelCatalogueController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: StudyCatalogues
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModelCatalogue.ToListAsync());
        }


        // GET: StudyCatalogues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var idd = await _context.ModelCatalogue.FirstOrDefaultAsync(m => m.Id == id);

            if  (idd.Status == true | User.IsInRole(SD.AdminUser))
            {

                if (id == null)
                {
                    return NotFound();
                }

                var modelCatalogue = await _context.ModelCatalogue
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (modelCatalogue == null)
                {
                    return NotFound();
                }

                return View(modelCatalogue);


              
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



        // POST: ModelCatalogues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Kind,Description,StudyLead,Status,StartDate")] ModelCatalogue modelCatalogue)
        {
            if (ModelState.IsValid)
            {
                modelCatalogue.StartDate = DateTime.UtcNow;
                _context.Add(modelCatalogue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modelCatalogue);
        }



        // GET: StudyCatalogues/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCatalogue = await _context.ModelCatalogue.FindAsync(id);
            if (modelCatalogue == null)
            {
                return NotFound();
            }
            return View(modelCatalogue);
        }




        // POST: StudyCatalogues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Kind,Description,StudyLead,Status,StartDate")] ModelCatalogue modelCatalogue)
        {
            if (id != modelCatalogue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelCatalogue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelCatalogueExists(modelCatalogue.Id))
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
            return View(modelCatalogue);
        }



        // GET: StudyCatalogues/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCatalogue = await _context.ModelCatalogue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelCatalogue == null)
            {
                return NotFound();
            }

            return View(modelCatalogue);
        }


        // POST: StudyCatalogues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelCatalogue = await _context.ModelCatalogue.FindAsync(id);
            _context.ModelCatalogue.Remove(modelCatalogue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelCatalogueExists(int id)
        {
            return _context.StudyCatalogue.Any(e => e.Id == id);
        }


    }
}
