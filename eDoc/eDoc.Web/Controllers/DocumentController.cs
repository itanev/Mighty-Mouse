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

namespace eDoc.Web.Controllers
{
    public class DocumentController : BaseController
    {
        public ActionResult Index()
        {
            return View(GetDocumentsAsVM(this.Data.Documents.All()));
        }

        public ActionResult Pending()
        {
            var documents = this.Data.Documents.All().Where(x => x.EmailValidated && x.PhoneValidated);
            return View(GetDocumentsAsVM(documents));
        }

        public ActionResult Answer()
        {
            return View();
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
                    // todo: send email
                    string fromNumber;
                    string accountSid;
                    string authToken;
                    Settings.GetSmsSettings(out fromNumber, out accountSid, out authToken);
                    string mailgunAccount;
                    string mailgunKey;
                    string fromEmail;
                    Settings.GetEmailSettings(out mailgunAccount, out mailgunKey, out fromEmail);
                    var smsClient = new Twilio.TwilioRestClient(accountSid, authToken);
                    smsClient.SendSmsMessage(fromNumber, user.PhoneNumber,
                        @"Your confirmation code is " + docToAdd.PhoneCode + ".");

                    // https://api.mailgun.net/v2
                    // http://documentation.mailgun.com/quickstart.html#sending-messages

                    var client = new MailgunClient(mailgunAccount, mailgunKey);

                    var message = new System.Net.Mail.MailMessage(fromEmail, user.Email);
                    message.Sender = new MailAddress(fromEmail);

                    message.From = message.Sender;
                    message.Subject = "MightyMouse Document Confirmation - " + title;

                    message.Body = "Your confirmation code is " + docToAdd.EmailCode + ".";

                    var result = client.SendMail(message);
                    this.Data.Documents.Add(docToAdd);
                    this.Data.SaveChanges();
                }
            }

            return View("Index", GetDocumentsAsVM(this.Data.Documents.All()));
        }

        [HttpPost]
        public ActionResult EmailVerify(string code, int id)
        {
            var doc = this.Data.Documents.GetById(id);

            if (doc.EmailCode == code)
            {
                doc.EmailValidated = true;
            }

            return View("Details", doc);
        }

        [HttpPost]
        public ActionResult GsmVerify(string code, int id)
        {
            var doc = this.Data.Documents.GetById(id);

            if (doc.PhoneCode == code)
            {
                doc.PhoneValidated = true;
            }

            return View("Details", doc);
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
