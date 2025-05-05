using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gis.BLL.Interface;
using Gis.BLL.Repositries;
using Gis.DAL.Data;

namespace Gis.BLL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMosqueRepository MosqueRepository {  get;}
        public IRestaurantRepository RestaurantRepository { get; }
        public IPharmacyRepository PharmacyRepository { get; }
        public IMarketRepositorey MarketRepository { get; }


        private readonly GisDbContext _context;



        public UnitOfWork(GisDbContext context)
        {
            _context = context;
            MosqueRepository = new MosqueRepository(_context);
            RestaurantRepository = new RestaurantRepository(_context);
            PharmacyRepository = new PharmacyRepository(_context);
            MarketRepository = new MarketRepository(_context);
            //MarketRepositorey

        }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
