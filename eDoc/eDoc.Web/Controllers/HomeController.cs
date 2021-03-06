﻿using System;
using System.Collections.Generic;
using System.IO;
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
                var user = GetCurrentUser();
                return View(user);
            }

            return View();
        }

        public ActionResult GetTokenAssembly(string userName)
        {
            var user = GetCurrentUser();
            if (user.IsTokenAssemblyDownloaded == true && false)
            {
                // todo: return error
                return Content("Already downloaded!");
            }
            var assembly = Utils.GetTokenAssembly(user.UserName);

            user.IsTokenAssemblyDownloaded = true;
            this.Data.SaveChanges();

            return File(
               new MemoryStream(assembly),
               "application/octet-stream", "token-"+user.UserName+".exe"); 
        }

   
    }
}