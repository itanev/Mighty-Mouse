using eDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDoc.Data
{
    public interface IUowData
    {
        IRepository<Document> Documents { get; }

        IRepository<ApplicationUser> Users { get; }
        
        int SaveChanges();
    }
}
