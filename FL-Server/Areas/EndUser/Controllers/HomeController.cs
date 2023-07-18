using FL_Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FL_Server.Controllers
{
    [Area("EndUser")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Clients()
        {
            return View();
        }




        public IActionResult DockerBuild()

        {

            string filePathExecute = "wwwroot/Shell/Docker-build.sh";
            FileInfo fileInfo = new FileInfo(filePathExecute);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = $"\"{fileInfo.FullName}\"";
            Process process = Process.Start(startInfo);
            return View();
        }

        public IActionResult DockerRun()

        {

            string filePathExecute = "wwwroot/Shell/Docker-run.sh";
            FileInfo fileInfo = new FileInfo(filePathExecute);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = $"\"{fileInfo.FullName}\"";
            Process process = Process.Start(startInfo);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}