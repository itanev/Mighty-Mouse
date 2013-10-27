using eDoc.Data;
using eDoc.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eDoc.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, eDoc.Data.Migrations.Configuration>());
           // Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            using (var context = new ApplicationDbContext())
            {
                // context.Database.Initialize(true);
                if (context.Statuses.Count() == 0)
                {
                    context.Statuses.Add(new Status { Name = "Unverified" });
                    context.Statuses.Add(new Status { Name = "Pending" });
                    context.Statuses.Add(new Status { Name = "Approved" });
                    context.Statuses.Add(new Status { Name = "Rejected" });
                }
                if (context.DocumentTypes.Count() == 0)
                {
                    foreach (var dt in new[]{
                        new DocumentType { Name = "Application" },
                        new DocumentType { Name = "Declaration" },
                        new DocumentType { Name = "Other" }
                    })
                    {
                        context.DocumentTypes.Add(dt);
                    }
                }
                if (context.Users.Count() == 0)
                {
                    var userAdmin = new ApplicationUser()
                    {
                        UserName = "admin",
                        Email = "admin@mightymouse.com",
                        PhoneNumber = "0000",
                        Logins = new Collection<UserLogin> { new UserLogin { LoginProvider = "Local", ProviderKey = "admin", } },
                        Roles = new Collection<UserRole> {new UserRole {Role = new Role("Admin")}}
                    };

                    context.Users.Add(userAdmin);
                    context.UserSecrets.Add(new UserSecret("admin", "ACQbq83L/rsvlWq11Zor2jVtz2KAMcHNd6x1SN2EXHs7VuZPGaE8DhhnvtyO10Nf5Q=="));
                }
                 context.SaveChanges();

            }
            Settings.Initialize();
        }
    }
}
