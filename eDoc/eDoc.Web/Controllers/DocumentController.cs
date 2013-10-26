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

        // GET: /User/
        public ActionResult Index()
        {
            var items = new HashSet<DocumentIndexVM>();

            foreach (var item in this.Data.Documents.All())
            {
                items.Add(new DocumentIndexVM
                {
                    Id = item.Id,
                    AuthorName = item.Author.UserName,
                    Date = item.Date,
                    Status = Enum.GetName(typeof(Status), item.Status),
                    Type = Enum.GetName(typeof(DocumentType), item.Type)
                });
            }
            return View(items);
        }

        // GET: /User/Details/5
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
            return View(document);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            ViewBag.DocumentTypes = this.Data.DocumentTypes.All().ToList();
            return View();
        }

        [HttpGet]
        public ActionResult SelectType(string type)
        {
            return PartialView("_" + type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document)
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
                        Content = document.Content,
                        Status = Status.Pending,
                        Type = document.Type
                    };

                    this.Data.Documents.Add(docToAdd);
                    this.Data.SaveChanges();
                }
            }

            return View(document);
        }

        // GET: /User/Edit/5
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
            return View(document);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Document document)
        {
            if (ModelState.IsValid)
            {
                this.Data.Documents.Update(document);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: /User/Delete/5
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
            return View(document);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = this.Data.Documents.GetById((int)id);
            this.Data.Documents.Delete(document);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
