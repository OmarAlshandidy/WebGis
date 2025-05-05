using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gis.BLL.Interface;
using Gis.DAL.Data;
using Gis.DAL.Models;
using Microsoft.EntityFrameworkCore;
using static Gis.BLL.Interface.IGenericRepository;

namespace Gis.BLL.Repositries
{
    public class PharmacyRepository:GenericRepository<Pharmacy>,IPharmacyRepository
    {
        private readonly GisDbContext _context;

        public PharmacyRepository(GisDbContext context) : base(context)
        {
            _context = context;
        }

        public  async Task<List<Pharmacy>> GetByNameAsync(string name)
        {
            return  await _context.Pharmacies.Where(P => P.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
