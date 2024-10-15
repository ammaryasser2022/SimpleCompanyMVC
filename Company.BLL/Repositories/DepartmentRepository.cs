using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;

namespace Company.BLL.Repositories
{

    // CLR -- 1 allocte place in heap  -- 2 - default value ( private att = null ) -- 3- call user D const in exist (no exist)
    // --4- make ref --> object 
    // now private att == null ----> in GetAll when clr call it (_context.Departement == null.Department ) -> Exception
    // i need the clr creat object from AppDbcontex before creat object from DepartmentRepository to call functions
    // by make a constractor to ask clr to make an object from AppDbcontext
    // but U still need to allow Dependancy Injections  : 
    // now creation object of DepartmentRepository depending on creating object from AppDBContext first -

    public class DepartmentRepository : GenericRepository<Department>,  IDepartmentRepository
    {



        public DepartmentRepository(AppDBContext context) :base(context)
        {

        }

		public IEnumerable<Department> GetByCode(string code)
		{
            return _context.Departments.Where(D=>D.Code.ToLower().Contains(code.ToLower())).ToList();
		}
	}
}
