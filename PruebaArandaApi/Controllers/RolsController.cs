using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAranda.Entitis.Enum;
using PruebaArandaApi.Helpers;
using PruebaArandaApi.Interface;

namespace PruebaArandaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RolsController : ControllerBase
    {
        private IRolsService _rols;
        private IEventLogService _eventLogService;
        bool isSuccessfull = true;
        string response = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InformationUserService"></param>
        public RolsController(IRolsService rols, IEventLogService eventLogService)
        {
            _rols = rols;
            _eventLogService = eventLogService;
        }
        /// <summary>
        /// Obtener los roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllRols()
        {
            try
            {
                var identityUserView = await _rols.GetAllRols();
                if (identityUserView == null)
                {
                    response = Convert.ToString("No se ha encontrado ningún rol");
                    return NotFound(new EventMessage { Message = response });
                }
                return Ok(identityUserView);
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                response = ex.Message;
                return BadRequest(new EventMessage { Message = "Error en el servicio GetAllRols - Contacte al Adminsitrador" });
            }
            finally
            {
                await _eventLogService.CreateEventLog(ObjectType.Rols.ToString(), isSuccessfull, response, Request);
            }
        }
    }
}
