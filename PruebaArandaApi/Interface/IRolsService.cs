using PruebaAranda.Entitis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaArandaApi.Interface
{
    public interface IRolsService
    {
        Task<List<RolsView>> GetAllRols();
    }
}
