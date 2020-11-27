using PruebaAranda.Entitis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaArandaApi.Interface
{
    public interface IInformationUserService
    {
        Task<int> CreateInfoUser(InformationUserView infoUserView);
        Task<int> UpdateInfoUser(InformationUserView infoUserView);
        Task<InformationUserView> GetInfoUserById(Guid informationUserId);
        Task<List<InformationUserView>> GetInfoUserByNameOrRolsId(string name, Guid? rolsId);
        Task<int> RemoveInfoUser(Guid InfoUserId);
    }
}
