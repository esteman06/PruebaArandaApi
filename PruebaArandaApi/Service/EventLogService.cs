using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using PruebaAranda.DataModel.Model;
using PruebaAranda.Entitis;
using PruebaArandaApi.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PruebaArandaApi.Service
{
    public class EventLogService : IEventLogService
    {
        /// <summary>
        /// Administra que objetos están habilitados para auditar
        /// </summary>
        private static Dictionary<string, bool> auditObjects;
        private readonly PruebaArandaContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public EventLogService(PruebaArandaContext context)
        {
            _context = context;
        }

        public async Task CreateEventLog(EventLogView eventLogView)
        {
            try
            {
                EventLog newEventLog = MapEventLogView(eventLogView);
                _context.EventLog.Add(newEventLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                //Escribir el error en un LOG 
                //throw new Exception("Error creating EventLog: " + ex.Message);
            }
        }

        /// <summary>
        /// Registrar un evento en el log
        /// </summary>
        /// <param name="cloudUserId">ID del usuario</param>
        /// <param name="objType">Quien</param>
        /// <param name="isSuccessfull">Satisfactorio</param>
        /// <param name="response">Respuesta</param>
        /// <param name="parameters">Parámetros</param>
        /// <param name="actionType">Método del llamado</param>        
        public async Task CreateEventLog(string objType, bool isSuccessfull, string response, HttpRequest request, [CallerMemberName] string caller = null)
        {
            if (auditObjects != null && auditObjects.ContainsKey(objType) && auditObjects[objType])
            {
                ClaimsPrincipal principal = request.HttpContext.User as ClaimsPrincipal;
                var claim = ((ClaimsIdentity)principal.Identity).FindFirst(ClaimTypes.UserData);
                Guid _userID = claim == null ? Guid.Empty : Guid.Parse(claim.Value);
                if (_userID != Guid.Empty)
                {
                    string parameters = null;
                    if (request.Method == HttpMethod.Post.ToString() || request.Method == HttpMethod.Put.ToString())
                    {
                        string body = null;
                        request.EnableRewind();
                        if (request.QueryString.ToString() == "")
                        {
                            parameters = "Url : " + request.Path.ToString() + " Body :";
                        }
                        else
                        {
                            parameters = "Url : " + request.Path.ToString() + request.QueryString.ToString() + " Body :";
                        }
                        using (var reader = new StreamReader(request.Body))
                        {
                            body = reader.ReadToEnd();
                            request.Body.Seek(0, SeekOrigin.Begin);
                            body = reader.ReadToEnd();
                        }
                        parameters = parameters + " " + body;
                    }
                    else if (request.Method == HttpMethod.Get.ToString() || request.Method == HttpMethod.Delete.ToString())
                    {
                        parameters = request.Path;
                    }
                    EventLogView loadEventLogView = new EventLogView()
                    {
                        EventLogId = Guid.NewGuid(),
                        IdentityUserId = _userID,
                        Date = DateTime.Now.ToUniversalTime().ToString(),
                        //Quien
                        ObjectType = objType,
                        //Nombre del método
                        Action = request.Method + "/" + caller,
                        //Valido o no
                        IsSuccessfull = isSuccessfull,
                        //Mensaje
                        Response = response,
                        //Atributos enviados
                        Parameters = parameters
                    };
                    await CreateEventLog(loadEventLogView);
                }
            }
        }

        /// <summary>
        /// Obtener el EventLog donde IsSuccessfull es falso
        /// </summary>
        /// param name="date"
        /// <returns></returns>
        public async Task<List<EventLogView>> GetEventLog(DateTime date)
        {
            List<EventLogView> eventLogViews = new List<EventLogView>();
            var tempList = await _context.EventLog.Where(x => x.IsSuccessfull.Equals(false) && x.Date.Date.Equals(date)).OrderByDescending(x => x.Date).Include(x => x.IdentityUser).ToListAsync();
            if (tempList.Count > 0)
            {
                foreach (var item in tempList)
                {
                    EventLogView eventLogView = MapEventlog(item);
                    eventLogViews.Add(eventLogView);
                }
            }
            return eventLogViews;
        }

        #region
        private EventLog MapEventLogView(EventLogView eventLogView)
        {
            EventLog newEventLog = new EventLog()
            {
                EventLogId = eventLogView.EventLogId.GetValueOrDefault(),
                IdentityUserId = eventLogView.IdentityUserId,
                Date = Convert.ToDateTime(eventLogView.Date),
                ObjectType = eventLogView.ObjectType,
                IsSuccessfull = eventLogView.IsSuccessfull,
                Response = eventLogView.Response,
                Parameters = eventLogView.Parameters,
                Action = eventLogView.Action
            };
            return newEventLog;
        }

        private EventLogView MapEventlog(EventLog eventLog)
        {
            EventLogView newEventLogView = new EventLogView()
            {
                EventLogId = eventLog.EventLogId,
                IdentityUserId = eventLog.IdentityUserId,
                Email = eventLog.IdentityUser.Name,
                Date = eventLog.Date.ToString(),
                ObjectType = eventLog.ObjectType,
                IsSuccessfull = eventLog.IsSuccessfull,
                Response = eventLog.Response,
                Parameters = eventLog.Parameters,
                Action = eventLog.Action
            };
            return newEventLogView;
        }
        #endregion
    }
}
