using eDoc.Data;
using eDoc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eDoc.Web
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, eDoc.Data.Migrations.Configuration>());
            //Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            using (var context = new ApplicationDbContext())
            {
                // context.Database.Initialize(true);
                if (context.Statuses.Count() == 0)
                {
                    context.Statuses.Add(new Status { Name = "Pending" });
                }
                 if (context.DocumentTypes.Count() == 0)
                {
                    foreach(var dt in new[]{
                        new DocumentType { Name = "Application" },
                        new DocumentType { Name = "Declaration" },
                        new DocumentType { Name = "Other" }
                    }){
                    context.DocumentTypes.Add(dt);
                    }
                }
                 context.SaveChanges();

            }
            Settings.Initialize();
            //*/
        }
    }
}
