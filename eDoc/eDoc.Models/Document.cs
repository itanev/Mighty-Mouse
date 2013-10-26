using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDoc.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public virtual Status Status { get; set; }

        [Required]
        public virtual DocumentType Type { get; set; }

        public bool PhoneValidated { get; set; }

        public bool EmailValidated { get; set; }

        public string PhoneCode { get; set; }

        public string EmailCode { get; set; }

        public string Title { get; set; }
    } 
}
