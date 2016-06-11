# WebApiOwinAutofacDemo
Webapi 2 uses Owin and Autofac, a simple demo to hook up Autofac with Owin and Webapi

Step 1
Add nuget packages for Autofac and Autofac.Owin to your project

Step 2
Setup Autofac container in Startup.cs

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

Step 3
Remove the following codes in Startup.cs of method ConfigureAuth(IAppBuilder app)
```
         app.CreatePerOwinContext(ApplicationDbContext.Create);
         app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
```
Step 4
Remove the original code to get ApplicationUserManager
```
         var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
```
Add new code to use Autofac to resolve ApplicationUserManager
```
         var userManager = context.OwinContext.GetAutofacLifetimeScope().Resolve<ApplicationUserManager>();
```
