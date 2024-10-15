using System;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Company.PL.Mapping.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // any of them to add Dependancy Injections 
            //builder.Services.AddScoped<AppDBContext>(); //Allow DI For AppDBContext 
            //builder.Services.AddSingleton()
            //builder.Services.AddTransient()

            // Used to Add DI for DbContexts Classes Direct // its an Extention Method 
            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings : DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));  // da el connection string in appsetting folder 
            }); //default Scoped
                //


            builder.Services.AddScoped< IDepartmentRepository, DepartmentRepository>(); //Allow DI for _departmentrepository
            builder.Services.AddScoped< IEmployeeRepository, EmployeeRepository>(); //Allow DI for _departmentrepository
            builder.Services.AddAutoMapper(typeof(EmployeeProfile)); //Allow DI for IMapper //Defaukt ITransiant 

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDBContext>()// FOR ADD DI for stores 
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
