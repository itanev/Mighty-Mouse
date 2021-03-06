﻿using eDoc.Data;
using eDoc.Models;
using eDoc.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDoc.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected eDoc.Models.ApplicationUser GetCurrentUser()
        {
            var user = this.Data.Users.All().FirstOrDefault(x => x.UserName == this.User.Identity.Name);
            return user;
        }

        protected IUowData Data;

        protected HashSet<DocumentIndexVM> GetDocumentsAsVM(IQueryable<Document> documents)
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
                    Type = item.Type.Name,
                    TokenValidated = item.TokenValidated,
                    EmailValidated = item.EmailValidated,
                    PhoneValidated = item.PhoneValidated,
                    Comment = item.Comment,
                    TokenCode = item.TokenCode,
                    TokenInput = item.TokenInput,
                });
            }

            return items;
        }

        protected DocumentIndexVM GetDocumentAsVM(Document doc)
        {
            return new DocumentIndexVM()
            {
                Id = doc.Id,
                Date = doc.Date,
                Content = doc.Content,
                AuthorName = doc.Author.UserName,
                Status = doc.Status.Name,
                Type = doc.Type.Name,
                EmailValidated = doc.EmailValidated,
                Comment = doc.Comment,
                TokenValidated = doc.TokenValidated,
                PhoneValidated = doc.PhoneValidated,
                TokenInput = doc.TokenInput,
                TokenCode = doc.TokenCode,
            };
        }

        public BaseController(IUowData data)
        {
            this.Data = data;
        }

        public BaseController()
            : this(new UowData())
        {
        }
	}
}