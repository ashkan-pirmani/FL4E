using Microsoft.AspNetCore.Mvc;

namespace FL_Server.Areas.DataScientist.Controllers
{

    public class MountCenter : Controller
    {

        public IActionResult Data()
        {

            ViewBag.Message = TempData["Message"];
            return View();
        }


        public IActionResult Script()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files)
        {

            foreach (var file in files)
            { 

            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "//wwwroot//Scripts//");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            if (!basePathExists) Directory.CreateDirectory(basePath);
            //var fileName = Path.GetFileNameWithoutExtension(files.FileName);
            var filePath = Path.Combine(basePath, file.FileName);
            //var extension = Path.GetExtension(files.FileName);


            if (!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //FileOnFileSystemModel fileModel = new FileOnFileSystemModel
                //{
                //    CreatedOn = DateTime.UtcNow,
                //    FileType = file.ContentType,
                //    Extension = extension,
                //    Name = fileName,
                //    Description = description,
                //    FilePath = filePath
                //};
            }
            }

            TempData["Message"] = "File successfully uploaded to File System.";
            return Redirect("~/Home/Index");

        }

    }
}
