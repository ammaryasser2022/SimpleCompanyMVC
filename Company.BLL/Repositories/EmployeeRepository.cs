using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository: GenericRepository<Employee> , IEmployeeRepository
    {
        //private readonly AppDBContext  _context ;         // now no need  

        public EmployeeRepository(AppDBContext context) :base(context)
        // Error when a class inherit mn class
        {
            // _context = context;   // now no need         // any constractor inside class by default     
        }                                                   //will chain on parameterless constractor on parent
                                                            //(GenericRepository<Employee> )  .. and no paraless
                                                            //constractor there but there is parameter cons
                                                            // so chain on it by base here


        public async Task<IEnumerable<Employee>> GetByNameAsync(string name)
        {
           return await _context.Employees
                                    .Where(E=>E.Name.ToLower().Contains(name.ToLower()))
                                    .Include(E=>E.WorkFor)
                                    .ToListAsync();
        }


    }
}
