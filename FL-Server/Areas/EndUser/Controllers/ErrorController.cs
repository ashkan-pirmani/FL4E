using Microsoft.AspNetCore.Mvc;

namespace FL_Server.Areas.EndUser.Controllers
{
    [Area("EndUser")]
    public class ErrorController : Controller
    {
        [Route("EndUser/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Error";
                    break;
            }


            return View("Index");
        }
    }
}
