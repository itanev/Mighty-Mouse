using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDoc.Web.ViewModels
{
    public class DocumentEditVM
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}