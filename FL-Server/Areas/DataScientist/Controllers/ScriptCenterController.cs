using FL_Server.Data;
using FL_Server.Models;
using FL_Server.Models.Models;
using FL_Server.Models.Models.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FL_Server.Areas.DataScientist.Controllers
{
    [Area("DataScientist")]
    [Authorize]
    public class ScriptCenterController : Controller
    {


        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;



        public ScriptCenterController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Upload()
        {

            return View();
        }

        public async Task<IActionResult> Index()
        {

            var model = new FileUploadViewModel()
            {
                ApplicationUser = await context.ApplicationUser.ToListAsync(),
                FilesOnDatabase = await context.FilesOnDatabase.ToListAsync(),
                studyCatalogueList = await context.StudyCatalogue.ToListAsync(),
                modelCatalogueList = await context.ModelCatalogue.ToListAsync()
            };

            ViewData["ApplicationUserId"] = new SelectList(context.ApplicationUser, "Id", "LastName");
            ViewData["StudyCatalogueId"] = new SelectList(context.StudyCatalogue, "Id", "Title");
            ViewData["ModelCatalogueId"] = new SelectList(context.ModelCatalogue, "Id", "Title");
            ViewBag.Message = TempData["Message"];
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UploadToDatabase([Bind("ModelCatalogueId,StudyCatalogueId")] List<IFormFile> files, string description,FileUploadViewModel fileUploadViewModel)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var modelcatalogueID = fileUploadViewModel.filesOnDatabase.ModelCatalogueId;
                var studycatalogueID = fileUploadViewModel.filesOnDatabase.StudyCatalogueId;
                
                var fileModel = new FileOnDatabaseModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    ApplicationUserId = userId,
                    ModelCatalogueId = modelcatalogueID,
                    StudyCatalogueId = studycatalogueID


                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                
                context.FilesOnDatabase.Add(fileModel);
                context.SaveChanges();
            }
            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnDatabase = await context.FilesOnDatabase.ToListAsync();
            //viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.FilesOnDatabase.Remove(file);
            var fileID = await context.StudyCenter.Where(x => x.FileModelId == id).FirstOrDefaultAsync();
            context.StudyCenter.Update(fileID);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }


    }
}
