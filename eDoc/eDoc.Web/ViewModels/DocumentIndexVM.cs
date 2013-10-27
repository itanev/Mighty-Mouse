using eDoc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace eDoc.Web.ViewModels
{
    public class DocumentIndexVM : IDocumentValidationData
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        public int StatusId { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Вид")]
        public string Type { get; set; }

        [DisplayName("Съдържание")]
        public string Content { get; set; }

        [DisplayName("Коментар на отговора")]
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