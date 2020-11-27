using Microsoft.AspNetCore.Http;
using PruebaAranda.Entitis;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PruebaArandaApi.Interface
{
    public interface IEventLogService
    {
        Task CreateEventLog(EventLogView eventLogView);
        Task CreateEventLog(string objType, bool isSuccessfull, string response, HttpRequest request, [CallerMemberName] string caller = null);
        Task<List<EventLogView>> GetEventLog(DateTime date);
    }
}
