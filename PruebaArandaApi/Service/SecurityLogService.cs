using Microsoft.AspNetCore.Http;
using PruebaAranda.DataModel.Model;
using PruebaArandaApi.Interface;
using System;

namespace PruebaArandaApi.Service
{
    public class SecurityLogService : ISecurityLogService
    {
        private readonly PruebaArandaContext _context;
        private IHttpContextAccessor _accessor;

        public SecurityLogService(PruebaArandaContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public void RegisterLogIn(Guid userID)
        {
            Registre(ActivityType.LogIn, userID);
        }

        public void RegisterLogOut(Guid userID)
        {
            Registre(ActivityType.LogOut, userID);
        }

        public void RegisterTimeOut(Guid userID)
        {
            Registre(ActivityType.TimeOut, userID);
        }

        internal void Registre(ActivityType activityType, Guid userID)
        {
            try
            {
                var ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                SecurityLog newSecurityLog = new SecurityLog()
                {
                    SecurityLogId = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Activity = activityType.ToString(),
                    IdentityUserId = userID,
                    RemoteIpaddress = ip
                };
                _context.SecurityLog.Add(newSecurityLog);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //Revisar si se debe generar el LOG
                throw new Exception("Error security log: " + ex.Message);
            }
        }
    }
    public enum ActivityType
    {
        LogIn,
        LogOut,
        TimeOut
    }
}
