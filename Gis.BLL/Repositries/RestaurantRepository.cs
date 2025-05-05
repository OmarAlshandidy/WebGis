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
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        private readonly GisDbContext _context;

        public RestaurantRepository(GisDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Restaurant>> GetByNameAsync(string name)
        {
            return await _context.Restaurants.Where(M => M.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
