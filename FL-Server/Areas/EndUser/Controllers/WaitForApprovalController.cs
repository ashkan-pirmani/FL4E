using Microsoft.AspNetCore.Mvc;

namespace FL_Server.Areas.EndUser
{
    [Area("EndUser")]
    public class WaitForApprovalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
