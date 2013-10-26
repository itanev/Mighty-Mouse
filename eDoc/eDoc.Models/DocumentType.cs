using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace eDoc.Models
{
    public class DocumentType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public DocumentType()
        {
            this.Documents = new HashSet<Document>();
        }
    }
}
