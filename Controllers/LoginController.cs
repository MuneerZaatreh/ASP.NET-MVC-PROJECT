using Expose_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expose_Tracker.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index(int Password, Transaction transaction)
        {
            if(Password==325191757)
            {
               // return View("~/Views/DashBoard/Index.cshtml");
                return  new RedirectResult(@"~/DashBoard");
            }
            TempData["AlertMessage"] = "The Password Is Wrong";
            return RedirectToAction("Index");
        }
    }
}
