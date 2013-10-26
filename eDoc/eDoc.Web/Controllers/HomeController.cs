using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDoc.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var user = this.Data.Users.All().FirstOrDefault(x => x.UserName == this.User.Identity.Name);
                return View(user);
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}