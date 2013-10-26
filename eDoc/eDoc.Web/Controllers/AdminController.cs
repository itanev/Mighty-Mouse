using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDoc.Web.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Answer(int id)
        {
            var currentDocument = this.Data.Documents.GetById(id);

            return View(GetDocumentAsVM(currentDocument));
        }

        [HttpPost]
        public ActionResult Answer()
        {
            return View();
        }
    }
}