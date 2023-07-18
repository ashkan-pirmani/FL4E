using FL_Server.Data;
using FL_Server.Models;
using FL_Server.Models.Models;
using FL_Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FL_Server.Areas.DataScientist.Controllers
{
    [Area("DataScientist")]
    [Authorize]
    public class ModelCenterController : Controller
    {

        private readonly ApplicationDbContext _context;


        public ModelCenterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudyCenters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ModelCenter.Include(s => s.ApplicationUser).Include(s => s.FileModel).Include(s => s.ModelCatalogue).Include(s=>s.StudyCenter);
            return View(await applicationDbContext.ToListAsync());
        }









        // GET: StudyCenters
        public async Task<IActionResult> Analysis(int? id, HomeViewModel viewModel)
        {
            var applicationDbContext = _context.ModelCenter.Include(s => s.ApplicationUser).Include(s => s.FileModel).Include(s => s.ModelCatalogue).Include(s => s.StudyCenter);
            var catalogueID = _context.ModelCenter.Include(s => s.ApplicationUser).Include(s => s.FileModel).Where(s => s.ModelCatalogue.Id == id).Include(s => s.StudyCenter);
            var catalogue = await _context.ModelCatalogue.FirstOrDefaultAsync(m => m.Id == id);
            var check = _context.ModelCenter.ToList().Find(m => m.ModelCatalogueId == id);



            if (id == null)
            {
                return NotFound();
            }
         


            if (check == null)
            {
                return NotFound();
            }



            viewModel = new HomeViewModel()
            {

                modelCenterList = await catalogueID.ToListAsync(),
                ModelCatalogue = catalogue

            };


            return View(viewModel);
        }









        public async Task<IActionResult> DSS(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var modelCenter = await _context.ModelCenter.FindAsync(id);
            if (modelCenter == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", modelCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", modelCenter.FileModelId);
            ViewData["ModelCatalogueId"] = new SelectList(_context.ModelCatalogue, "Id", "Title", modelCenter.ModelCatalogueId);
            ViewData["StudyCenterId"] = new SelectList(_context.StudyCenter, "Id", "Title", modelCenter.StudyCenterID);
            return View(modelCenter);

        }



        // GET: StudyCenters/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var idd = await _context.ModelCenter.FirstOrDefaultAsync(m => m.Id == id);
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (idd.ModelType == false)

            {
                if (idd.ApplicationUserId == user | User.IsInRole(SD.AdminUser))
                {

                    if (id == null)
                    {
                        return NotFound();
                    }

                    var modelCenter = await _context.ModelCenter
                        .Include(s => s.ApplicationUser)
                        .Include(s => s.FileModel)
                        .Include(s => s.StudyCenter)
                        .Include(s => s.ModelCatalogue)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (modelCenter == null)
                    {
                        return NotFound();
                    }

                    return View(modelCenter);

                }

                return LocalRedirect("/Identity/Account/AccessDenied");

            }


            else
            {



                if (id == null)
                {
                    return NotFound();
                }

                var modelCenter = await _context.ModelCenter
                    .Include(s => s.ApplicationUser)
                    .Include(s => s.FileModel)
                    .Include(s => s.StudyCenter)
                    .Include(s => s.ModelCatalogue)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (modelCenter == null)
                {
                    return NotFound();
                }

                return View(modelCenter);

            }
        }


        // GET: StudyCenters/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email");
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name");
            ViewData["ModelCatalogueId"] = new SelectList(_context.ModelCatalogue, "Id", "Title");
            ViewData["StudyCenterId"] = new SelectList(_context.StudyCenter, "Id" , "Title", _context.StudyCenter.Include(m=>m.StudyCatalogue.Title));
            return View();
        }


        // POST: StudyCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelCatalogueId,ModelType,ApplicationUserId,FileModelId,LastUpdate,DSS,StudyCenterID,Title")] ModelCenter modelCenter)
        {
            modelCenter.LastUpdate = DateTime.UtcNow;
            modelCenter.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Add(modelCenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", modelCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", modelCenter.FileModelId);
            ViewData["ModelCatalogueId"] = new SelectList(_context.ModelCatalogue, "Id", "Title", modelCenter.ModelCatalogueId);
            ViewData["StudyCenterId"] = new SelectList(_context.StudyCenter, "Id", "Title", modelCenter.StudyCenterID);
            return View(modelCenter);
        }



        // GET: StudyCenters/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCenter = await _context.ModelCenter.FindAsync(id);
            if (modelCenter == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", modelCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", modelCenter.FileModelId);
            ViewData["ModelCatalogueId"] = new SelectList(_context.ModelCatalogue, "Id", "Title", modelCenter.ModelCatalogueId);
            ViewData["StudyCenterId"] = new SelectList(_context.StudyCenter, "Id", "Title", modelCenter.StudyCenterID);
            return View(modelCenter);
        }



        // POST: StudyCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelCatalogueId,ModelType,ApplicationUserId,FileModelId,LastUpdate,DSS,StudyCenterID,Title")] ModelCenter modelCenter)
        {
            if (id != modelCenter.Id)
            {
                return NotFound();
            }


            try
            {
                modelCenter.LastUpdate = DateTime.UtcNow;
                modelCenter.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Update(modelCenter);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyCenterExists(modelCenter.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", modelCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", modelCenter.FileModelId);
            ViewData["ModelCatalogueId"] = new SelectList(_context.ModelCatalogue, "Id", "Title", modelCenter.ModelCatalogueId);
            ViewData["StudyCenterId"] = new SelectList(_context.StudyCenter, "Id", "Title", modelCenter.StudyCenterID);
            return View(modelCenter);
        }

        // GET: StudyCenters/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelCenter = await _context.ModelCenter
                .Include(s => s.ApplicationUser)
                .Include(s => s.FileModel)
                .Include(s => s.ModelCatalogue)
                .Include(s => s.StudyCenter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelCenter == null)
            {
                return NotFound();
            }

            return View(modelCenter);
        }


        // POST: StudyCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelCenter = await _context.ModelCenter.FindAsync(id);
            _context.ModelCenter.Remove(modelCenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyCenterExists(int id)
        {
            return _context.StudyCenter.Any(e => e.Id == id);
        }




    }
}
