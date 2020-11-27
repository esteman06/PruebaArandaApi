using PruebaAranda.Entitis;
using System;
using System.Threading.Tasks;

namespace PruebaArandaApi.Interface
{
    public interface ILoginService
    {
        Task<IdentityUserView> LogIn(string username, string password);
        void LogOut(Guid userID);
        void TimeOut(Guid userID);
    }
}
