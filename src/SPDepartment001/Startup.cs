using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SPDepartment001.Infrastructure.Validators;
using SPDepartment001.Models.Data;
using SPDepartment001.Models.DB;
using SPDepartment001.Models.Identity;

namespace SPDepartment001
{
    public class Startup
    {
        IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration["Data:SPDepartment:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(
                options => options.UseSqlServer(Configuration["Data:SPDepartmentIdentity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;

                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();

            //services.AddTransient<IEmployeeRepository, FakeEmployeeRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IEmployeeAccountRepository, EmployeeAccountRepository>();
            services.AddTransient<IDepositRepository, DepositRepository>();
            services.AddTransient<IDepartmentEventRepository, DepartmentEventRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentity();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=CurrentUser}/{action=Info}/{id?}");
            });

            //AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            //SeedEmployeeUserData.EnsurePopulated(app);
        }
    }
}
