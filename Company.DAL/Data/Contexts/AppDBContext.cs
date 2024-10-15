using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Data.Configration;
using Company.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.DAL.Data.Contexts
{
    public class AppDBContext :  IdentityDbContext<ApplicationUser>  // :IdentityDbContext  --> DA EL 3ady but i create in modules a ApplicationUser 
                                                                     // to add more prop to IdentityUser                             
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)   // h3tmd 3la options in connection string
        {                                                                           // w al8y el OnConfiguring

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new DepartmentConfigrations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
            

            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = AMMAR_YASSER  ; Database = CompanyMVC ; Trusted_Connection = True  ; TrustServerCertificate =True ");

        //}
    
        
        
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees     { get; set; }


    
    }
}
