﻿using eDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDoc.Web.ViewModels
{
    public class DocumentIndexVM : IDocumentValidationData
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Content { get; set; }

        public string Comment { get; set; }

        public bool PhoneValidated { get; set; }

        public bool EmailValidated { get; set; }

        public bool TokenValidated { get; set; }

        public string PhoneCode { get; set; }

        public string EmailCode { get; set; }

        public string TokenInput { get; set; }

        public string TokenCode { get; set; }
    }
}