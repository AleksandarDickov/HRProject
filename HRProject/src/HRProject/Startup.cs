using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HRProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using HRProject.Services;
using Newtonsoft.Json;

namespace HRProject
{
    public class Startup
    {
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        //    if (env.IsEnvironment("Development"))
        //    {
        //        // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
        //        builder.AddApplicationInsightsSettings(developerMode: true);
        //    }

        //    builder.AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}

      //  public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }); ;

            services.AddAuthorization(options =>
            {

                options.AddPolicy("SuperUser",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("SuperUser");
                    });

                options.AddPolicy("HrManager",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("HrManager");
                   });

                options.AddPolicy("RegUser",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("RegUser");
                   });

                options.AddPolicy("SuperUser, HrManager",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("SuperUser", "HrManager");
                   });

                options.AddPolicy("SuperUser, HrManager,RegUser",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("RegUser", "SuperUser", "HrManager");
                   });
            });

            // Add framework services.
         //   services.AddApplicationInsightsTelemetry(Configuration);


            //services.AddIdentity<User, IdentityRole>(config =>
            //{
            //    config.User.RequireUniqueEmail = true;
            //    config.Password.RequiredLength = 8;
            //    config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            //})
            //.AddEntityFrameworkStores<DbContext>();

            //services.AddLogging();
            

            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=HRContext;Trusted_Connection=True;";
            services.AddDbContext<HRContext>(o => o.UseSqlServer(connectionString));

            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<HRContext>()
               .AddDefaultTokenProviders();

            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 2;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;

                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/Job") &&
                        ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            })
          .AddEntityFrameworkStores<HRContext>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            loggerFactory.AddConsole();
            app.UseIdentity();
            createRolesandUsers(roleManager, userManager);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            //app.UseApplicationInsightsRequestTelemetry();
            //app.UseIdentity();
            //app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc(config =>
            {
                 config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Auth", action = "Login" });
            });
        }
        public async void createRolesandUsers(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            if (!roleManager.RoleExistsAsync("SuperUser").Result)
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "SuperUser";
                await roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new User();
                user.UserName = "Aca";
                user.Email = "aca@gmail.com";

                string userPWD = "sifra";

                var chkUser = await userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = await userManager.AddToRoleAsync(user, "SuperUser");
                }
            }

            var res = await roleManager.RoleExistsAsync("HrManager");
            // creating Creating Manager role    
            if (res == false)
            {
                var role = new IdentityRole();
                role.Name = "HrManager";
                await roleManager.CreateAsync(role);

                var user = new User();
                user.UserName = "Jela";
                user.Email = "jela@gmail.com";

                string userPWD = "sifra123";

                var chkUser = await userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = await userManager.AddToRoleAsync(user, "HrManager");
                }
            }

            res = await roleManager.RoleExistsAsync("RegularUser");
            // creating Creating Employee role    
            if (res == false)
            {
                var role = new IdentityRole();
                role.Name = "RegularUser";
                await roleManager.CreateAsync(role);
            }

        }
    }
}
