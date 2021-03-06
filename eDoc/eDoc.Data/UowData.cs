﻿using eDoc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDoc.Data
{
    public class UowData : IUowData
    {
        private readonly ApplicationDbContext context;

        public ApplicationDbContext Context
        {
            get { return context; }
        } 

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UowData()
            : this(new ApplicationDbContext())
        {
        }

        public UowData(ApplicationDbContext context)
        {
            this.context = context;
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public IRepository<Models.Document> Documents
        {
            get { return this.GetRepository<Document>(); }
        }

        public IRepository<Models.ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }


        public IRepository<DocumentType> DocumentTypes
        {
            get { return this.GetRepository<DocumentType>(); }
        }

        public IRepository<Status> Statuses
        {
            get { return this.GetRepository<Status>(); }
        }
    }
}
