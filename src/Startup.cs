using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using WebApiOwinAutofacDemo.Models;

[assembly: OwinStartup(typeof(WebApiOwinAutofacDemo.Startup))]

namespace WebApiOwinAutofacDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.Register(c => new UserStore<ApplicationUser>(c.Resolve<ApplicationDbContext>())).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = new DpapiDataProtectionProvider("ApplicationName")
            });
            builder.RegisterType<ApplicationUserManager>().InstancePerLifetimeScope();

            var container = builder.Build();

            // Register the Autofac middleware FIRST. This also adds
            // Autofac-injected middleware registered with the container.
            app.UseAutofacMiddleware(container);

            // ...then register your other middleware not registered
            // with Autofac.

            
            ConfigureAuth(app);
        }

    }
}
