using PruebaAranda.Entitis;
using System;
using System.Threading.Tasks;

namespace PruebaArandaApi.Interface
{
    public interface IIdentityUserService
    {
        Task<int> CreateIdentityUser(IdentityUserView identityUserView);
        Task<int> UpdateIdentityUser(IdentityUserView identityUserView);
        Task<int> RemoveIdentityUser(Guid identityUserId);
    }
}
