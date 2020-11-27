using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAranda.Entitis;
using PruebaAranda.Entitis.Enum;
using PruebaArandaApi.Helpers;
using PruebaArandaApi.Interface;

namespace PruebaArandaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InformationUserController : ControllerBase
    {
        private IInformationUserService _informationUserService;
        private IEventLogService _eventLogService;
        bool isSuccessfull = true;
        string response = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InformationUserService"></param>
        public InformationUserController(IInformationUserService informationUserService, IEventLogService eventLogService)
        {
            _informationUserService = informationUserService;
            _eventLogService = eventLogService;
        }

        /// <summary>
        /// Crea un usuario nuevo
        /// </summary>
        /// <param name="informationUserView">lista de vista</param>
        /// <returns></returns>
        [HttpPost("CreateInformationUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> CreateInformationUser([FromBody] InformationUserView informationUserView)
        {
            try
            {
                int result = await _informationUserService.CreateInfoUser(informationUserView);
                if (result == 0)
                {
                    response = Convert.ToString("No se ha podido crear el Usuario");
                    return NotFound(new EventMessage { Message = response });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                response = ex.Message;
                return BadRequest(new EventMessage { Message = "Error en el servicio CreateIdentityUser - Contacte al Adminsitrador" });
            }
            finally
            {
                await _eventLogService.CreateEventLog(ObjectType.InformationUser.ToString(), isSuccessfull, response, Request);
            }
        }

        /// <summary>
        /// Modifica un usuario nuevo
        /// </summary>
        /// <param name="informationUserView">lista de vista</param>
        /// <returns></returns>
        [HttpPost("UpdateInformationUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateInformationUser([FromBody] InformationUserView informationUserView)
        {
            try
            {
                int result = await _informationUserService.UpdateInfoUser(informationUserView);
                if (result == 0)
                {
                    response = Convert.ToString("No se ha podido modificar el Usuario");
                    return NotFound(new EventMessage { Message = response });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                response = ex.Message;
                return BadRequest(new EventMessage { Message = "Error en el servicio CreateIdentityUser - Contacte al Adminsitrador" });
            }
            finally
            {
                await _eventLogService.CreateEventLog(ObjectType.InformationUser.ToString(), isSuccessfull, response, Request);
            }
        }

        /// <summary>
        /// Obtener un usuario por ID
        /// </summary>
        /// <param name="informationUserID">ID del usuario</param>
        /// <returns></returns>
        [HttpGet("GetInformationUserById/{informationUserID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetInformationUserById([FromRoute] Guid informationUserID)
        {
            try
            {
                var identityUserView = await _informationUserService.GetInfoUserById(informationUserID);
                if (identityUserView == null)
                {
                    response = Convert.ToString("No se ha encontrado el usuario");
                    return NotFound(new EventMessage { Message = response });
                }
                return Ok(identityUserView);
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                response = ex.Message;
                return BadRequest(new EventMessage { Message = "Error en el servicio GetInformationUserById - Contacte al Adminsitrador" });
            }
            finally
            {
                await _eventLogService.CreateEventLog(ObjectType.InformationUser.ToString(), isSuccessfull, response, Request);
            }
        }

        /// <summary>
        /// Obtener los usuarios por nombre o rol
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetInfoUserByNameOrRolsId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetInfoUserByNameOrRolsId(string name, Guid? rolsId)
        {
            try
            {
                var identityUserView = await _informationUserService.GetInfoUserByNameOrRolsId(name,rolsId);
                if (identityUserView == null)
                {
                    response = Convert.ToString("No se ha encontrado ningún usuario");
                    return NotFound(new EventMessage { Message = response });
                }
                return Ok(identityUserView);
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                response = ex.Message;
                return BadRequest(new EventMessage { Message = "Error en el servicio GetInfoUserByNameOrRolsId - Contacte al Adminsitrador" });
            }
            finally
            {
                await _eventLogService.CreateEventLog(ObjectType.InformationUser.ToString(), isSuccessfull, response, Request);
            }
        }

        /// <summary>
        /// Elimina un usuario por id
        /// </summary>
        /// <param name="informationUserId">Id usuario</param>
        /// <returns></returns>
        [HttpPost("RemoveInformationUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RemoveIdentityUser([FromBody] Guid informationUserId)
        {
            try
            {
                await _informationUserService.RemoveInfoUser(informationUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                response = ex.Message;
                return BadRequest(new EventMessage { Message = "Error en el servicio RemoveInfoUser - Contacte al Adminsitrador" });
            }
            finally
            {
                await _eventLogService.CreateEventLog(ObjectType.InformationUser.ToString(), isSuccessfull, response, Request);
            }
        }
    }
}
