using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDoc.Models
{
    public class ApplicationUser : User
    {
        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        private ICollection<Document> documents;

        public virtual ICollection<Document> Documents
        {
            get { return this.documents; }
            set { this.documents = value; }
        }

        public ApplicationUser()
        {
            this.documents = new HashSet<Document>();
        }
    }
}
