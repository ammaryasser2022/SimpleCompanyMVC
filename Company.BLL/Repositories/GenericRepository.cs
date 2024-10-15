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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity     //** 
    {
        private protected readonly AppDBContext _context;
        public GenericRepository(AppDBContext context)
        {
            _context = context; 
            
        }
        public async Task<IEnumerable<T>> GetAllAsync()
      
        {
            if (typeof(T) == typeof(Employee))
            {                                                       
                return  (IEnumerable<T>) await _context.Employees.Include(E=>E.WorkFor).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();  // hna el T may be any thing no just Employee And Department
            }

           

        }
        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<int> AddAsync(T entity)
        {
            //_context.Add(entity);
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            //_context.Update(entity);
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(T entity)
        {
            //_context.Remove(entity);
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

      

       

     
    }
}
