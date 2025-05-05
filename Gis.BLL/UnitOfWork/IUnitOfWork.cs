using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gis.BLL.Interface;
using Gis.BLL.Repositries;

namespace Gis.BLL.UnitOfWork
{

    public interface IUnitOfWork : IAsyncDisposable
    {
     
        public IMosqueRepository  MosqueRepository { get; }
        public IRestaurantRepository RestaurantRepository { get; }
        public  IPharmacyRepository PharmacyRepository { get; }
        public IMarketRepositorey   MarketRepository { get; }

        Task<int> CompleteAsync();
    }
}
