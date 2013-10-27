using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDoc.Models
{
    public class Document : IDocumentValidationData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Съдържание")]
        public string Content { get; set; }

        [DisplayName("Име автор")]
        public string AuthorId { get; set; }

        [DisplayName("Име автор")]
        public virtual ApplicationUser Author { get; set; }

        [DisplayName("Статус")]
        public virtual Status Status { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [DisplayName("Тип документ")]
        public virtual DocumentType Type { get; set; }

        [DisplayName("Телефон валидиран?")]
        public bool PhoneValidated { get; set; }

        [DisplayName("Email валидиран?")]
        public bool EmailValidated { get; set; }

        [DisplayName("Код телефон")]
        public bool TokenValidated { get; set; }

        public string PhoneCode { get; set; }

        [DisplayName("Код Email")]
        public string EmailCode { get; set; }

        [DisplayName("TokenIn")]
        public string TokenInput { get; set; }

        [DisplayName("Код Token")]
        public string TokenCode { get; set; }

        [DisplayName("Коментар на отговора")]
        public string Comment { get; set; }
    }
}
