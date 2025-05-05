using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gis.DAL.Models;

namespace Gis.BLL.Interface
{
    public interface IGenericRepository
    {
        public interface IGenericRepository<T> where T : BaseEntity
        {
            Task<IEnumerable<T>> GetAllAsync();
            Task<T?> GetAsync(int id);
            Task AddAsync(T model);
            void Update(T model);
            void Delete(T model);
        }
    }
}
