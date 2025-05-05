using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gis.DAL.Models;
using static Gis.BLL.Interface.IGenericRepository;

namespace Gis.BLL.Interface
{
    public interface IMosqueRepository: IGenericRepository<Mosque>
    {
        Task<List<Mosque>> GetByNameAsync(string name);

    }
}
