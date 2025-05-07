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
    public class StudentHousingRepository : GenericRepository<StudentHousing>, IStudentHousingRepository
    {
        private readonly GisDbContext _context;

        public StudentHousingRepository(GisDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<StudentHousing>> GetByNameAsync(string name)
        {
            return await _context.StudentHousings.Where(P => P.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
