using FL_Server.Data;
using FL_Server.Models.Models;
using FL_Server.Models.Models.DataModel.DataCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FL_Server.Areas.DataScientist.Controllers
{
    [Area("DataScientist")]
    public class AnalysisCenterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;


        public AnalysisCenterController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        // GET: AnalysisCenters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AnalysisCenter.Include(a => a.StudyCenter);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AnalysisCenters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisCenter = await _context.AnalysisCenter
                .Include(a => a.StudyCenter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisCenter == null)
            {
                return NotFound();
            }

            return View(analysisCenter);
        }

        // GET: AnalysisCenters/Create
        public IActionResult Create()
        {
            ViewData["StudyCenterID"] = new SelectList(_context.StudyCenter, "Id", "Id");
            return View();
        }

        // POST: AnalysisCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,StudyCenterID")] AnalysisCenter analysisCenter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analysisCenter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudyCenterID"] = new SelectList(_context.StudyCenter, "Id", "Id", analysisCenter.StudyCenterID);
            return View(analysisCenter);
        }

        // GET: AnalysisCenters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisCenter = await _context.AnalysisCenter.FindAsync(id);
            if (analysisCenter == null)
            {
                return NotFound();
            }
            ViewData["StudyCenterID"] = new SelectList(_context.StudyCenter, "Id", "Id", analysisCenter.StudyCenterID);
            return View(analysisCenter);
        }

        // POST: AnalysisCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,StudyCenterID")] AnalysisCenter analysisCenter)
        {
            if (id != analysisCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysisCenter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisCenterExists(analysisCenter.Id))
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
            ViewData["StudyCenterID"] = new SelectList(_context.StudyCenter, "Id", "Id", analysisCenter.StudyCenterID);
            return View(analysisCenter);
        }

        // GET: AnalysisCenters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisCenter = await _context.AnalysisCenter
                .Include(a => a.StudyCenter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisCenter == null)
            {
                return NotFound();
            }

            return View(analysisCenter);
        }

        // POST: AnalysisCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var analysisCenter = await _context.AnalysisCenter.FindAsync(id);
            _context.AnalysisCenter.Remove(analysisCenter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisCenterExists(int id)
        {
            return _context.AnalysisCenter.Any(e => e.Id == id);
        }


        public IActionResult Central()
        {

            var basePathOutput = Path.Combine(Directory.GetCurrentDirectory() + "//Outcome//");
            bool basePathOutputExists = System.IO.Directory.Exists(basePathOutput);
            if (!basePathOutputExists) Directory.CreateDirectory(basePathOutput);

            string[] filepaths = Directory.GetFiles(Path.Combine(this._environment.WebRootPath, basePathOutput));

            List<DataCoreFileOnSystemModel> list = new List<DataCoreFileOnSystemModel>();
            foreach (string filepath in filepaths)
            {
                list.Add(new DataCoreFileOnSystemModel { Name = Path.GetFileName(filepath) });
            }
            return View(list);

        }

        public FileResult DownloadCentral(string filename)
        {
            var basePathOutput = Path.Combine(Directory.GetCurrentDirectory() + "//Outcome//");
            bool basePathOutputExists = System.IO.Directory.Exists(basePathOutput);
            if (!basePathOutputExists) Directory.CreateDirectory(basePathOutput);

            string path = Path.Combine(this._environment.WebRootPath, basePathOutput) + filename;
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", filename);
        }

    }
}
