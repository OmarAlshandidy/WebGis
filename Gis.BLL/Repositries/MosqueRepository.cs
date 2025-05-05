using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gis.BLL.Interface;
using Gis.DAL.Data;
using Gis.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Gis.BLL.Repositries
{
    public class MosqueRepository:GenericRepository<Mosque>, IMosqueRepository
    {
        private readonly GisDbContext _context;

        public MosqueRepository(GisDbContext context):base(context) 
        {
            _context = context;
        }

        public async Task<List<Mosque>> GetByNameAsync(string name)
        {
                   return await _context.Mosques.Where(M => M.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
