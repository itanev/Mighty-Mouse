using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDoc.Web.Controllers
{
    [Authorize(Roles="Admin")]
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

            string body = 
@"<p>Your document #" + id + @" has been answered:<br/> 
Status: " + currentDocument.Status.Name;
            if(!string.IsNullOrWhiteSpace(comment)){
                body += "<br/>Comment: " + comment;
            }
            body += "</p><p>You can see details about the document by logging into the system.</p>";
            var user = currentDocument.Author;
            Utils.SendEmail(user.Email, "Document #" + id + " answered", body, user.UserName);
            return RedirectToAction("Index", "Home");
        }
    }
}