using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using eDoc.Models;
using eDoc.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using eDoc.Web.ViewModels;


namespace eDoc.Web.Controllers
{
    public class DocumentController : BaseController
    {
        private HashSet<DocumentIndexVM> GetDocumentsAsVM(IQueryable<Document> documents)
        {
            var items = new HashSet<DocumentIndexVM>();
            foreach (var item in documents)
            {
                items.Add(new DocumentIndexVM
                {
                    Id = item.Id,
                    AuthorName = item.Author.UserName,
                    Date = item.Date,
                    Content = item.Content,
                    Status = item.Status.Name,
                    Type = item.Type.Name
                });
            }

            return items;
        }

        private DocumentIndexVM GetDocumentAsVM(Document doc)
        {
            return new DocumentIndexVM()
            {
                Id = doc.Id,
                Date = doc.Date,
                Content = doc.Content,
                AuthorName = doc.Author.UserName,
                Status = doc.Status.Name,
                Type = doc.Type.Name
            };
        }

        public ActionResult Index()
        {
            return View(GetDocumentsAsVM(this.Data.Documents.All()));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = this.Data.Documents.GetById((int)id);
            if (document == null)
            {
                return HttpNotFound();
            }

            return View(GetDocumentAsVM(document));
        }

        [HttpGet]
        public ActionResult SelectType(string type)
        {
            return PartialView("_" + type);
        }

        public ActionResult Create()
        {
            ViewBag.DocumentTypes = this.Data.DocumentTypes.All().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string content, int type)
        {
            if (ModelState.IsValid)
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    string userId = this.User.Identity.GetUserId();

                    var user = this.Data.Users.All().FirstOrDefault(x => x.Id == userId);
                    Document docToAdd = new Document
                    {
                        Author = user,
                        Date = DateTime.Now,
                        Content = content,
                        Status = this.Data.Statuses.All().FirstOrDefault(),
                        Type = this.Data.DocumentTypes.All().FirstOrDefault(x => x.Id == type)
                    };

                    this.Data.Documents.Add(docToAdd);
                    this.Data.SaveChanges();
                }
            }

            return View("Index", GetDocumentsAsVM(this.Data.Documents.All()));
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = this.Data.Documents.GetById((int)id);
            if (document == null)
            {
                return HttpNotFound();
            }

            ViewBag.DocumentTypes = this.Data.DocumentTypes.All().ToList();
            ViewBag.Statuses = this.Data.Statuses.All().ToList();

            return View(GetDocumentAsVM(document));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentEditVM document)
        {
            if (ModelState.IsValid)
            {
                var currentDocument = this.Data.Documents.GetById(document.Id);
                currentDocument.Content = document.Content;
                currentDocument.Status = this.Data.Statuses.GetById(document.Status);
                currentDocument.Type = this.Data.DocumentTypes.GetById(document.Type);
                this.Data.Documents.Update(currentDocument);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index", GetDocumentsAsVM(this.Data.Documents.All()));
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = this.Data.Documents.GetById((int)id);
            if (document == null)
            {
                return HttpNotFound();
            }

            return View(GetDocumentAsVM(document));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = this.Data.Documents.GetById((int)id);
            this.Data.Documents.Delete(document);
            this.Data.SaveChanges();
            return RedirectToAction("Index", GetDocumentsAsVM(this.Data.Documents.All()));
        }
    }
}
