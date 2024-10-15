using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Identity.Client;

namespace Company.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department> // Any Repository should be like this 
    {
		//IEnumerable<Department> GetAll();
		//Department Get(int Id);

		//int Add(Department entity);  // return int that it returns from SaveChanges After Add  / >0 added
		//int Update(Department entity);
		//int Delete(Department entity);

		IEnumerable<Department> GetByCode(string code);
    }
}
