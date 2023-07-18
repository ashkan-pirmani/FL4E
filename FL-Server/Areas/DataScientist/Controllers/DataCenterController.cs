using FL_Server.Data;
using FL_Server.Models;
using FL_Server.Models.Models.DataModel;
using FL_Server.Models.Models.DataModel.DataCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace FL_Server.Areas.DataScientist.Controllers
{
    [Area("DataScientist")]
    [Authorize]
    public class DataCenterController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;



        public DataCenterController(ApplicationDbContext context, UserManager<IdentityUser> userManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            this.context = context;
            _userManager = userManager;
            _environment = environment;

        }


        public async Task<IActionResult> Index()
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





            var model = new DataUploadViewModel()
            {
                ApplicationUser = await context.ApplicationUser.ToListAsync(),
                DataDictionary = await context.DataDictionaryOnDatabase.ToListAsync(),
                DataCleanFunction = await context.DataCleanFunctionOnDatabase.ToListAsync(),
                DataInputs = await context.DataInputModelOnDatabase.ToListAsync(),
                studyCatalogueList = await context.StudyCatalogue.ToListAsync(),
                modelCatalogueList = await context.ModelCatalogue.ToListAsync(),
                DataCore = list ,
                DataDictionaryFile = await context.DataDictionaryOnFileSystem.ToListAsync(),
            };

            ViewData["ApplicationUserId"] = new SelectList(context.ApplicationUser, "Id", "Email");
            ViewData["StudyCatalogueId"] = new SelectList(context.StudyCatalogue, "Id", "Title");
            ViewData["ModelCatalogueId"] = new SelectList(context.ModelCatalogue, "Id", "Title");
            ViewData["DataDictionaryId"] = new SelectList(context.DataDictionaryOnDatabase, "Id", "Title");
            
            ViewBag.Message = TempData["Message"];
            return View(model);
        }

        // GET: DataCenter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataOnDatabaseModel = await context.DataDictionaryOnDatabase.FindAsync(id);
            if (dataOnDatabaseModel == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(context.ApplicationUser, "Id", "LastName", dataOnDatabaseModel.ApplicationUserId);
            ViewData["StudyCatalogueId"] = new SelectList(context.StudyCatalogue, "Id", "Title", dataOnDatabaseModel.StudyCatalogueId);
            return View(dataOnDatabaseModel);
        }

        // POST: DataCenter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UsersEngaged,Status,Extension,Description,LastUploadTime,IntialUploadTime,ApplicationUserId,StudyCatalogueId,ModelCatalogueId")] DataDictionaryOnDatabaseModel dataOnDatabaseModel)
        {
            if (id != dataOnDatabaseModel.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    var results = context.DataDictionaryOnDatabase.SingleOrDefault(b=>b.Id==id);
                    results.LastUploadTime = DateTime.UtcNow;
                    results.Description = dataOnDatabaseModel.Description;
                    results.Title = dataOnDatabaseModel.Title;
                    results.UsersEngaged = dataOnDatabaseModel.UsersEngaged;
                    results.ApplicationUserId = dataOnDatabaseModel.ApplicationUserId;
                    results.StudyCatalogueId = dataOnDatabaseModel.StudyCatalogueId;
                    results.Status = dataOnDatabaseModel.Status;
                    context.Update(results);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataOnDatabaseModelExists(dataOnDatabaseModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ApplicationUserId"] = new SelectList(context.ApplicationUser, "Id", "LastName", dataOnDatabaseModel.ApplicationUserId);
            ViewData["StudyCatalogueId"] = new SelectList(context.StudyCatalogue, "Id", "Title", dataOnDatabaseModel.StudyCatalogueId);
            return View(dataOnDatabaseModel);
        }



        // GET: DataCenter/Details/5
        public async Task<IActionResult> Details(int? id, DataUploadViewModel viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataOnDatabaseModel = await context.DataDictionaryOnDatabase
                .Include(d => d.ApplicationUser)
                .Include(d => d.StudyCatalogue)
                .FirstOrDefaultAsync(m => m.Id == id);

            var dataCleaning = await context.DataCleanFunctionOnDatabase
                .Include(d => d.DataDictionaryOnDatabaseModel)
                .FirstOrDefaultAsync(m => m.DataDictionaryOnDatabaseModelId == id);

            var dataInput = await context.DataInputModelOnDatabase
               .Include(d => d.DataDictionaryOnDatabaseModel)
               .FirstOrDefaultAsync(m => m.DataDictionaryOnDatabaseModelId == id);

            viewModel = new DataUploadViewModel()
            {

                dataDictionary = dataOnDatabaseModel,
                dataInputs = dataInput,
                dataCleanFunction = dataCleaning,
                DataCleanFunction = context.DataCleanFunctionOnDatabase.Where(m => m.DataDictionaryOnDatabaseModelId == dataOnDatabaseModel.Id).ToList(),
                DataInputs = context.DataInputModelOnDatabase.Where(m => m.DataDictionaryOnDatabaseModelId == dataOnDatabaseModel.Id).ToList(),
                DataDictionary =  context.DataDictionaryOnDatabase.ToList(),
                ApplicationUser =  context.ApplicationUser.ToList()



            };


            if (dataOnDatabaseModel == null)
            {
                return NotFound();
            }

            
            ViewData["DataDictionaryId"] = new SelectList(context.DataDictionaryOnDatabase, "Id", "Title", dataOnDatabaseModel.Id);
            ViewData["UserEngagedName"] = context.ApplicationUser.Where(m => m.Id == dataOnDatabaseModel.UsersEngaged).FirstOrDefault()?.LastName;
            ViewData["DataCleanFunction"] = context.DataCleanFunctionOnDatabase.Where(m => m.DataDictionaryOnDatabaseModelId == dataOnDatabaseModel.Id).FirstOrDefault()?.Id;
            ViewData["DataInput"] = context.DataInputModelOnDatabase.Where(m => m.DataDictionaryOnDatabaseModelId == dataOnDatabaseModel.Id).FirstOrDefault()?.Id;

            return View(viewModel);
        }


        

        [HttpPost]
        public async Task<IActionResult> DataDictionaryUpload([Bind("StudyCatalogueId,ApplicationUserId,Id,DataDictionaryOnDatabaseModelId")] List<IFormFile> files, DataUploadViewModel dataUploadViewModel)
        {
            foreach (var file in files)
            {
                var dataDictionary = dataUploadViewModel.dataDictionary.StudyCatalogueId;
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "//Data//StudyCatalogue"+ dataDictionary+"//");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var status = dataUploadViewModel.dataDictionary.Status;
                var title = dataUploadViewModel.dataDictionary.Title;
                var description = dataUploadViewModel.dataDictionary.Description;
                var userengaged = dataUploadViewModel.dataDictionary.ApplicationUserId;
                var studycatalogueID = dataUploadViewModel.dataDictionary.StudyCatalogueId;
                var filePath = Path.Combine(basePath, file.FileName);


                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);

                    }
                    var fileModelData = new DataDictionaryOnDatabaseModel
                    {
                        IntialUploadTime = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Status = status,
                        Title = title,
                        Name = fileName,
                        Description = description,
                        ApplicationUserId = userId,
                        UsersEngaged = userengaged,
                        StudyCatalogueId = studycatalogueID
                    };

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        fileModelData.DataDictionary = dataStream.ToArray();
                    }

                    context.DataDictionaryOnDatabase.Add(fileModelData);
                    context.SaveChanges();


                    var dID = context.DataDictionaryOnDatabase.Where(context => context.Name == fileName & context.Description == description & context.Title==title).FirstOrDefault()?.Id;

                    var fileModelSystem = new DataDictionaryOnFileSystemModel
                    {
                        IntialUploadTime = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Status = status,
                        Title = title,
                        Name = fileName,
                        Description = description,
                        ApplicationUserId = userId,
                        UsersEngaged = userengaged,
                        DataDictionaryPath = filePath,
                        StudyCatalogueId = studycatalogueID,
                        DataDictionaryOnDatabaseModelId = dID
                    };

                   
                    context.DataDictionaryOnFileSystem.Add(fileModelSystem);
                    context.SaveChanges();

                }
            }
            TempData["Message"] = "Data successfully uploaded to Database";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DataCleanFunctionUpload([Bind("StudyCatalogueId,ApplicationUserId,Id")] List<IFormFile> files, DataUploadViewModel dataUploadViewModel)
        {
            foreach (var file in files)
            {
                var dataDictionary = dataUploadViewModel.dataCleanFunction.DataDictionaryOnDatabaseModelId;
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "//Data//DataClean//"+dataDictionary+"//");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var status = dataUploadViewModel.dataCleanFunction.Status;
                var title = dataUploadViewModel.dataCleanFunction.Title;
                var description = dataUploadViewModel.dataCleanFunction.Description;
                var dataDictionaryID = dataUploadViewModel.dataCleanFunction.DataDictionaryOnDatabaseModelId;
                var filePath = Path.Combine(basePath, file.FileName);



                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);

                    }
                    var fileModelData = new DataCleanFunctionOnDatabaseModel 
                    {
                        IntialUploadTime = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Status = status,
                        Title = title,
                        Name = fileName,
                        Description = description,
                        DataDictionaryOnDatabaseModelId = dataDictionaryID
                    };

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        fileModelData.DataCleanFunction = dataStream.ToArray();
                    }

                    context.DataCleanFunctionOnDatabase.Add(fileModelData);
                    context.SaveChanges();


                    var dID = context.DataCleanFunctionOnDatabase.Where(context => context.Name == fileName & context.Description == description & context.Title == title).FirstOrDefault().Id;


                    var fileModelSystem = new DataCleanFunctionOnFileSystemModel
                    {
                        IntialUploadTime = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Status = status,
                        Title = title,
                        Name = fileName,
                        Description = description,
                        DataCleanFunctionPath = filePath,
                        DataDictionaryOnDatabaseModelId = dataDictionaryID,
                        DataCleanFunctionOnDatabaseModelId = dID
                    };

                    
                    context.DataCleanFunctionOnFileSystem.Add(fileModelSystem);
                    context.SaveChanges();

                }
            }
            TempData["Message"] = "Data successfully uploaded to Database";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DataInputUpload([Bind("StudyCatalogueId,ApplicationUserId,Id")] List<IFormFile> files, DataUploadViewModel dataUploadViewModel)
        {
            foreach (var file in files)
            {
                var dataDictionary = dataUploadViewModel.dataInputs.DataDictionaryOnDatabaseModelId;
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "//Data//DataInput//" +dataDictionary + "//");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var title = dataUploadViewModel.dataInputs.Title;
                var status = dataUploadViewModel.dataInputs.Status;
                var description = dataUploadViewModel.dataInputs.Description;
                var filePath = Path.Combine(basePath, file.FileName);
                var dataDictionaryID = dataUploadViewModel.dataInputs.DataDictionaryOnDatabaseModelId;
                var datacleantoolpath = context.DataCleanFunctionOnFileSystem.Where(context => context.DataDictionaryOnDatabaseModelId == dataDictionaryID).FirstOrDefault()?.DataCleanFunctionPath;
                var datadictionaryPath = context.DataDictionaryOnFileSystem.Where(context => context.DataDictionaryOnDatabaseModelId == dataDictionaryID).FirstOrDefault()?.DataDictionaryPath;
                var studycatalogueID = context.DataDictionaryOnFileSystem.Where(context => context.DataDictionaryOnDatabaseModelId == dataDictionaryID).FirstOrDefault()?.StudyCatalogueId;

                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);

                    }
                    var fileModelData = new DataInputModelOnDatabaseModel
                    {
                        IntialUploadTime = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Title = title,
                        Name = fileName,
                        Status = status,
                        Description = description,
                        ApplicationUserId = userId,
                        DataDictionaryOnDatabaseModelId = dataDictionaryID

                    };

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        fileModelData.DataInput = dataStream.ToArray();
                    }

                    context.DataInputModelOnDatabase.Add(fileModelData);
                    context.SaveChanges();


                    var dID = context.DataInputModelOnDatabase.Where(context => context.Name == fileName & context.Description == description & context.Title == title).FirstOrDefault()?.Id;



                    var fileModelSystem = new DataInputModelOnFileSystemModel
                    {
                        IntialUploadTime = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Title = title,
                        Name = fileName,
                        Status =status,
                        Description = description,
                        DataInput = filePath,
                        ApplicationUserId = userId,
                        DataDictionaryOnDatabaseModelId = dataDictionaryID,
                        DataInputModelOnDatabaseModelId = dID


                    };

                  
                    context.DataInputModelOnFileSystem.Add(fileModelSystem);
                    context.SaveChanges();

                }



                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false

                    }
                };

                process.Start();

                var basePathOutput = Path.Combine(Directory.GetCurrentDirectory() + "//Outcome//");
                bool basePathOutputExists = System.IO.Directory.Exists(basePathOutput);
                if (!basePathOutputExists) Directory.CreateDirectory(basePathOutput);

                await process.StandardInput.WriteLineAsync("cd 'Outcome' && echo 'cd '/app/Outcome' && python3 '"+ datacleantoolpath + " --user_id="+ userId + " --core_csv_source=" + datadictionaryPath+ " --input_csv="+filePath+ " --core_csv_target="+ datadictionaryPath+" '|' tee " + dataDictionaryID+".txt > Run.sh");


                Thread.Sleep(10);


                string filePathExecute = "/app/Outcome/Run.sh";
                FileInfo fileInfo = new FileInfo(filePathExecute);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = true;
                startInfo.FileName = "/bin/bash";
                startInfo.Arguments = $"\"{fileInfo.FullName}\"";
                Process process2 = Process.Start(startInfo);



                string DoneU = @"/app/Outcome/DoneU.txt";
                var CheckDoneU = (System.IO.File.Exists(DoneU));
                if (CheckDoneU == false)
                {
                    int delayForComupte = 0;
                    Thread.Sleep(delayForComupte);
                    while (CheckDoneU == false)
                    {

                        Thread.Sleep(delayForComupte);
                        delayForComupte++;
                        CheckDoneU = (System.IO.File.Exists(DoneU));
                        if (delayForComupte > 200)
                        {
                            break;
                            TempData["Message"] = "Data upload Failed";

                            return RedirectToAction("Index");
                        }

                    }
                }

                if (CheckDoneU == true)
                {
                    //await process.StandardInput.WriteLineAsync("cd /app/Outcome && rm Run.sh");

                    var DataCorefile = new DataCoreFileOnSystemModel
                    {

                        DataCorePath = datadictionaryPath + ".csv",
                        Name = dataDictionaryID.ToString()+".csv",
                        Title = title,
                        FileType = file.ContentType,
                        Extension = extension,
                        Description = description,
                        Status = status,
                        UploadedBy = userId,
                        CreatedOn = DateTime.UtcNow,
                        DataDictionaryOnDatabaseModelId = dataDictionaryID,
                        StudyCatalogueId = studycatalogueID
                       

                };

                    context.DataCoreFileOnSystem.Add(DataCorefile);
                    context.SaveChanges();

                    

                    await process.StandardInput.WriteLineAsync("cd /app/Outcome && rm DoneU.txt");

                }





                
                await process.StandardInput.WriteLineAsync("cd /app/Outcome && rm DoneU.txt");





            }





            TempData["Message"] = "Data successfully uploaded to Database";
        
            return RedirectToAction("Index");
        }




        private async Task<DataUploadViewModel> LoadAllFiles()
        {
            var viewModel = new DataUploadViewModel();
            viewModel.DataDictionary = await context.DataDictionaryOnDatabase.ToListAsync();
            viewModel.DataCleanFunction = await context.DataCleanFunctionOnDatabase.ToListAsync();
            viewModel.DataInputs = await context.DataInputModelOnDatabase.ToListAsync();
            return viewModel;
        }

     


        public async Task<IActionResult> DownloadDataDictionaryFromDatabase(int id)
        {

            var file = await context.DataDictionaryOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.DataDictionary, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DownloadDataDictionaryFromSystem(int id)
        {

            var file = await context.DataDictionaryOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.DataDictionaryPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }



        public async Task<IActionResult> DownloadDataCleanFunctionFromDatabase(int id)
        {

            var file = await context.DataCleanFunctionOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.DataCleanFunction, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DownloadDataCleanFunctionFromSystem(int id)
        {

            var file = await context.DataCleanFunctionOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.DataCleanFunctionPath, file.FileType, file.Name + file.Extension);
        }




        public async Task<IActionResult> DownloadDataInputFromDatabase(int id)
        {

            var file = await context.DataInputModelOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.DataInput, file.FileType, file.Name + file.Extension);
        }
        public async Task<IActionResult> DownloadDataInputFromSystem(int id)
        {

            var file = await context.DataInputModelOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.DataInput, file.FileType, file.Name + file.Extension);
        }


        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> DeleteDataDictionary(int id)
        {

            var file = await context.DataDictionaryOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            var fileSystem = await context.DataDictionaryOnFileSystem.Where(x => x.DataDictionaryOnDatabaseModelId == id).FirstOrDefaultAsync();
            var fileclean = await context.DataCleanFunctionOnDatabase.Where(x => x.DataDictionaryOnDatabaseModelId == id).FirstOrDefaultAsync();
            var filSystemeclean = await context.DataCleanFunctionOnFileSystem.Where(x => x.DataDictionaryOnDatabaseModelId == id).FirstOrDefaultAsync();
            var dataInput = await context.DataInputModelOnDatabase.Where(x => x.DataDictionaryOnDatabaseModelId == id).ToListAsync();
            var dataSystemInput = await context.DataInputModelOnFileSystem.Where(x => x.DataDictionaryOnDatabaseModelId == id).ToListAsync();
            var dataCorefile = await context.DataCoreFileOnSystem.Where(x => x.DataDictionaryOnDatabaseModelId == id).ToListAsync();

           



            context.DataDictionaryOnDatabase.Remove(file);

            if (System.IO.File.Exists(fileSystem.DataDictionaryPath))
            {
                System.IO.File.Delete(fileSystem.DataDictionaryPath);
            }


            context.DataDictionaryOnFileSystem.Remove(fileSystem);

            if (fileclean != null & filSystemeclean != null)
            {
                context.DataCleanFunctionOnDatabase.Remove(fileclean);
                context.DataCleanFunctionOnFileSystem.Remove(filSystemeclean);

            }
            if (dataInput != null & dataSystemInput !=null )
            {
                context.DataInputModelOnDatabase.RemoveRange(dataInput);
                context.DataInputModelOnFileSystem.RemoveRange(dataSystemInput);

            }

            if (dataCorefile != null)
            {
                context.DataCoreFileOnSystem.RemoveRange(dataCorefile);
            }

            //var fileID = await context.StudyCenter.Where(x => x.FileModelId == id).FirstOrDefaultAsync();
            //context.StudyCenter.Update(fileID);
            context.SaveChanges();




            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> DeleteDataCleanFunction(int id)
        {

            var file = await context.DataCleanFunctionOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            var clean = await context.DataCleanFunctionOnFileSystem.Where(x => x.DataCleanFunctionOnDatabaseModelId == id).FirstOrDefaultAsync();
            context.DataCleanFunctionOnDatabase.Remove(file);

            if (System.IO.File.Exists(clean.DataCleanFunctionPath))
            {
                System.IO.File.Delete(clean.DataCleanFunctionPath);
            }

            context.DataCleanFunctionOnFileSystem.Remove(clean);
            //var fileID = await context.StudyCenter.Where(x => x.FileModelId == id).FirstOrDefaultAsync();
            //context.StudyCenter.Update(fileID);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "AdminUser")]
        public async Task<IActionResult> DeleteDataInput(int id)
        {

            var file = await context.DataInputModelOnDatabase.Where(x => x.Id == id).ToListAsync();
            var dataInput = await context.DataInputModelOnFileSystem.Where(x => x.DataInputModelOnDatabaseModelId == id).ToListAsync();
            var datacore = await context.DataCoreFileOnSystem.Where(x => x.DataDictionaryOnDatabaseModelId == file.FirstOrDefault().DataDictionaryOnDatabaseModelId).ToListAsync();
            context.DataInputModelOnDatabase.RemoveRange(file);
            context.DataInputModelOnFileSystem.RemoveRange(dataInput);
            context.DataCoreFileOnSystem.RemoveRange(datacore);

            var countdatacore = datacore.Count();

            if (System.IO.File.Exists(dataInput.FirstOrDefault()?.DataInputPath))
            {
                System.IO.File.Delete(dataInput.FirstOrDefault()?.DataInputPath);
            }
            if (System.IO.File.Exists(datacore.FirstOrDefault()?.DataCorePath) & countdatacore == 1)
            {
                System.IO.File.Delete(datacore.FirstOrDefault()?.DataCorePath);

            }


            //var fileID = await context.StudyCenter.Where(x => x.FileModelId == id).FirstOrDefaultAsync();
            //context.StudyCenter.Update(fileID)
            context.SaveChanges();
            TempData["Message"] = $"Removed Data Input successfully from Database.";
            return RedirectToAction("Index");
        }



        private bool DataOnDatabaseModelExists(int id)
        {
            return context.DataDictionaryOnDatabase.Any(e => e.Id == id);
        }



        public IActionResult Central(int id)
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
