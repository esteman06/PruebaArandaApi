using System;

namespace PruebaArandaApi.Interface
{
    public interface ISecurityLogService
    {
        void RegisterLogIn(Guid userID);
        void RegisterLogOut(Guid userID);
        void RegisterTimeOut(Guid userID);
    }
}
