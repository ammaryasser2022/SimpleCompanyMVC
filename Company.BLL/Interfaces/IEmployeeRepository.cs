using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Models;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
        //IEnumerable<Employee> GetAll();
        //Employee Get(int id);
        //int Add(Employee entity) ;
        //int Update(Employee entity));
        //int Delete(Employee entity));


        Task<IEnumerable<Employee>> GetByNameAsync(string name); 
    }
}
