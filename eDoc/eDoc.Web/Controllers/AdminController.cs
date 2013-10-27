using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            ViewBag.Statuses = this.Data.Statuses.All().ToList();

            return View(GetDocumentAsVM(currentDocument));
        }

        [HttpPost]
        public ActionResult Answer(int id, int status, string comment)
        {
            var currentDocument = this.Data.Documents.GetById(id);
            currentDocument.Status = this.Data.Statuses.GetById(status);
            currentDocument.Comment = comment;
            currentDocument.Type = this.Data.Documents.GetById(id).Type;
            this.Data.Documents.Update(currentDocument);
            this.Data.SaveChanges();
            ViewBag.Statuses = this.Data.Statuses.All().ToList();

            return View(GetDocumentAsVM(currentDocument));
        }
    }
}