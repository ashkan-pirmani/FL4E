using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FL_Clients.Areas.EndUser.Controllers
{
    public class RunController : Controller
    {

        private HomeViewModel HomeVM;

        public IActionResult Index()
        {

            return View();
        }



        public IActionResult Run()
        {


            //string filePathExecute = "wwwroot/Shell/Run.sh";
            //FileInfo fileInfo = new FileInfo(filePathExecute);
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.CreateNoWindow = false;
            //startInfo.UseShellExecute = true;
            //startInfo.FileName = "/bin/bash";
            //startInfo.Arguments = $"\"{fileInfo.FullName}\"";
            //Process process = Process.Start(startInfo);

            return View();
        }   
        
        public async Task<IActionResult> Port([Bind("IP,ClientID")] HomeViewModel viewModel)
        {
            var IP = viewModel.IP;
            var ClientID = viewModel.ClientID;

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
            await process.StandardInput.WriteLineAsync("cd 'wwwroot/Scripts' && echo 'cd '/app/wwwroot/Scripts' && python3 client.py --agent_id='" + ClientID + " --server_address="+IP+ " > Run.sh" );
            //await process.StandardInput.WriteLineAsync("echo 'python3 server.py --server_address='"+IP+ " > Run.sh");
            //var output = await process.StandardOutput.ReadLineAsync();


            Thread.Sleep(10);


            string filePathExecute = "/app/wwwroot/Scripts/Run.sh";
            FileInfo fileInfo = new FileInfo(filePathExecute);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = $"\"{fileInfo.FullName}\"";
            Process process2 = Process.Start(startInfo);





            return Redirect("~/Run");

        }



        string RunCommand(string command, string args)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error)) { return output; }
            else { return error; }
        }

    }
}
