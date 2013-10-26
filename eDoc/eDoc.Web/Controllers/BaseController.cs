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
    public class BaseController : Controller
    {
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
                    EmailVarified = item.EmailValidated,
                    GsmVarified = item.PhoneValidated
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
                EmailVarified = doc.EmailValidated,
                GsmVarified = doc.PhoneValidated
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