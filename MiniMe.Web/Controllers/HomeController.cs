using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniMe.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            for (int index = 0; index < 5757; index++ )
                ViewBag.Message += "Bacon";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
