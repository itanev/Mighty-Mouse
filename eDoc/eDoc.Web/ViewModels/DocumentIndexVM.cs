using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDoc.Web.ViewModels
{
    public class DocumentIndexVM
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Content { get; set; }

        public bool PhoneValidated { get; set; }

        public bool EmailValidated { get; set; }

        public string PhoneCode { get; set; }

        public string EmailCode { get; set; }

        public string TokenInput { get; set; }

    }
}