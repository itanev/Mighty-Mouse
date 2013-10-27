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
using Typesafe.Mailgun;
using System.Net.Mail;
using System.Data.Entity.Validation;

namespace eDoc.Web.Controllers
{
    public class DocumentController : BaseController
    {
        public ActionResult Index()
        {
            if (this.User.IsInRole("Admin"))
            {
                // TODO: Fix. Not good!
                var docs = this.Data.Documents.All().ToList().Where(Settings.Validate).ToList();
                return View(GetDocumentsAsVM(docs.AsQueryable()));
            }

            var userId = this.User.Identity.GetUserId();
            var allDocsOfCurrUser = this.Data.Documents.All().Where(x => x.AuthorId == userId);
            return View(GetDocumentsAsVM(allDocsOfCurrUser));
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
        public ActionResult Create(string content, int type, string title)
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
                        Type = this.Data.DocumentTypes.GetById(type),
                        Status = this.Data.Statuses.All().FirstOrDefault(s => s.Name.ToLower() == "pending"),
                        PhoneCode = Utils.GetConfirmationCode("phone" + user.UserName, 8),
                        EmailCode = Utils.GetConfirmationCode("email" + user.UserName, 8),
                    };

                    if (Settings.ValidateToken)
                    {
                        docToAdd.TokenInput = Utils.GetConfirmationCode("token" + user.UserName, 8);
                        docToAdd.TokenCode = Utils.GetTokenConfirmationCode(user.UserName, docToAdd.TokenInput);
                        //docToAdd.TokenAssembly = Utils.GetTokenAssembly(docToAdd.TokenCode);
                        //docToAdd.TokenCode = Utils.GetTokenConfirmationCode(docToAdd.TokenInput);
                    }



                    this.Data.Documents.Add(docToAdd);
                    this.Data.SaveChanges();
                    try
                    {
                        if (Settings.ValidateSms)
                            Utils.SendSms(user.PhoneNumber, @"Document #" + docToAdd.Id + ": confirmation code is " + docToAdd.PhoneCode + ".");

                        if (Settings.ValidateEmail)
                            Utils.SendEmail(user.PhoneNumber, "Confirm document #" + docToAdd.Id,
                                "Your confirmation code for document #" + docToAdd.Id + " is " + docToAdd.EmailCode + ".");
                    }
                    catch
                    {
                        this.Data.SaveChanges();
                        this.Data.Documents.Delete(docToAdd);
                        throw;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EmailVerify(string code, int documentId)
        {
            var doc = this.Data.Context.Documents.Find(documentId);

            if (doc.EmailCode == code)
            {
                doc.EmailValidated = true;
            }

            this.Data.SaveChanges();

            return RedirectToAction("Details", new { id = documentId });
        }

        [HttpPost]
        public ActionResult GsmVerify(string code, int documentId)
        {
            var doc = this.Data.Documents.GetById(documentId);

            if (doc.PhoneCode == code)
            {
                doc.PhoneValidated = true;
            }
            this.Data.SaveChanges();

            return RedirectToAction("Details", new { id = documentId });
        }


        [HttpPost]
        public ActionResult TokenVerify(string code, int documentId)
        {
            var doc = this.Data.Documents.GetById(documentId);

            if (doc.TokenCode == code)
            {
                doc.TokenValidated = true;
            }
            this.Data.SaveChanges();

            return RedirectToAction("Details", new { id = documentId });
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

            return View("Index");
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = this.Data.Documents.GetById((int)id);
            this.Data.Documents.Delete(document);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
