#nullable disable

using FL_Server.Data;
using FL_Server.Models;
using FL_Server.Models.Models;
using FL_Server.Models.Models.DataModel.DataCore;
using FL_Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FL_Server.Areas.Controllers
{
    [Area("DataScientist")]
    [Authorize]
    public class StudyCenterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudyCenterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudyCenters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StudyCenter.Include(s => s.ApplicationUser).Include(s => s.FileModel).Include(s => s.StudyCatalogue);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: StudyCenters
        public async Task<IActionResult> Analysis(int? id, HomeViewModel viewModel)
        {
            var applicationDbContext = _context.StudyCenter.Include(s => s.ApplicationUser).Include(s => s.FileModel).Include(s => s.StudyCatalogue);
            var catalogueID = _context.StudyCenter.Include(s => s.ApplicationUser).Include(s => s.FileModel).Where(s => s.StudyCatalogue.Id == id);
            var catalogue = await _context.StudyCatalogue.FirstOrDefaultAsync(m => m.Id == id);
            var check = _context.StudyCenter.ToList().Find(m=>m.StudyCatalogueId == id);


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

                studyCenterList = await catalogueID.ToListAsync(),
                StudyCatalogue = catalogue,
            };


            return View(viewModel);
        }





        //public async Task<IActionResult> Analysis(int? id)
        //{

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var studyCenter = await _context.StudyCenter.FindAsync(id);
        //    if (studyCenter == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", studyCenter.ApplicationUserId);
        //    ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", studyCenter.FileModelId);
        //    ViewData["StudyCatalogueId"] = new SelectList(_context.StudyCatalogue, "Id", "StudyLead", studyCenter.StudyCatalogueId);
        //    return View(studyCenter);

        //}



        public IActionResult Launch(HomeViewModel viewModel)
        {
            var IP = viewModel.AnalysisCenter.IP;
            var Port = viewModel.AnalysisCenter.Port;


            return Redirect("http://"+IP + ":" + Port+"/");

        }









        // GET: StudyCenters/Details/5
        public async Task<IActionResult> Details(int? id, HomeViewModel viewModel)
        {
            var idd = await _context.StudyCenter.FirstOrDefaultAsync(m => m.Id == id);
            var user =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userengaged = _context.StudyCenter.Where(m => m.ApplicationUserId == user);


            if (idd.StudyType == false)
            {

                if (idd.ApplicationUserId == user | User.IsInRole(SD.AdminUser))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var studyCenter = await _context.StudyCenter
                        .Include(s => s.ApplicationUser)
                        .Include(s => s.FileModel)
                        .Include(s => s.StudyCatalogue)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    viewModel = new HomeViewModel()
                    {

                        StudyCenter = studyCenter


                    };


                    if (studyCenter == null)
                    {
                        return NotFound();
                    }

                    return View(viewModel);
                }

                return LocalRedirect("/Identity/Account/AccessDenied");



            }

            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var studyCenter = await _context.StudyCenter
                    .Include(s => s.ApplicationUser)
                    .Include(s => s.FileModel)
                    .Include(s => s.StudyCatalogue)
                    .FirstOrDefaultAsync(m => m.Id == id);

                viewModel = new HomeViewModel()
                {

                    StudyCenter = studyCenter


                };


                if (studyCenter == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }

          
        }

        // GET: StudyCenters/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email");
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name");
            ViewData["StudyCatalogueId"] = new SelectList(_context.StudyCatalogue, "Id", "Title");
            return View();
        }

        // POST: StudyCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudyCatalogueId,StudyType,ApplicationUserId,FileModelId,LastUpdate,Title")] StudyCenter studyCenter)
        {
            studyCenter.LastUpdate = DateTime.UtcNow;
            _context.Add(studyCenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", studyCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", studyCenter.FileModelId);
            ViewData["StudyCatalogueId"] = new SelectList(_context.StudyCatalogue, "Id", "Title", studyCenter.StudyCatalogueId);
            return View(studyCenter);
        }

        // GET: StudyCenters/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyCenter = await _context.StudyCenter.FindAsync(id);
            if (studyCenter == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", studyCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", studyCenter.FileModelId);
            ViewData["StudyCatalogueId"] = new SelectList(_context.StudyCatalogue, "Id", "Title", studyCenter.StudyCatalogueId);
            return View(studyCenter);
        }

        // POST: StudyCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudyCatalogueId,StudyType,ApplicationUserId,FileModelId,LastUpdate,Title")] StudyCenter studyCenter)
        {
            if (id != studyCenter.Id)
            {
                return NotFound();
            }


            try
            {
                studyCenter.LastUpdate = DateTime.UtcNow;
                _context.Update(studyCenter);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyCenterExists(studyCenter.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Email", studyCenter.ApplicationUserId);
            ViewData["FileModelId"] = new SelectList(_context.FilesOnDatabase, "Id", "Name", studyCenter.FileModelId);
            ViewData["StudyCatalogueId"] = new SelectList(_context.StudyCatalogue, "Id", "Title", studyCenter.StudyCatalogueId);
            return View(studyCenter);
        }

        // GET: StudyCenters/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyCenter = await _context.StudyCenter
                .Include(s => s.ApplicationUser)
                .Include(s => s.FileModel)
                .Include(s => s.StudyCatalogue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studyCenter == null)
            {
                return NotFound();
            }

            return View(studyCenter);
        }

        // POST: StudyCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studyCenter = await _context.StudyCenter.FindAsync(id);
            
            _context.StudyCenter.Remove(studyCenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyCenterExists(int id)
        {
            return _context.StudyCenter.Any(e => e.Id == id);
        }




        







    }
}
