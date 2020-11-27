using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAranda.Entitis;
using PruebaArandaApi.Helpers;
using PruebaArandaApi.Interface;

namespace PruebaArandaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        /// <summary>
        /// Autenticar del usuario
        /// </summary>
        /// <param name="userParam">Usuario</param>
        /// <returns>Valido o no</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> LogIn([FromBody] LoginRequestView userParam)
        {
            try
            {
                var user = await _loginService.LogIn(userParam.Name, userParam.Password);
                if (user == null)
                {
                    return NotFound(new EventMessage { Message = "Usuario o contraseña incorrecto" });
                }
                if (user.IsActive)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(new EventMessage { Message = "El usuario no se encuentra activo" });
                }

            }
            catch (Exception)
            {
                return BadRequest(new EventMessage { Message = "Error en el servicio Login - Contacte con el adminsitrador" });
            }
        }

        /// <summary>
        /// Cerrar la sesión del usuario
        /// </summary>
        /// <returns>Valido</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult LogOut()
        {
            try
            {
                var claim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.UserData);
                Guid userID = claim == null ? Guid.Empty : Guid.Parse(claim.Value);
                _loginService.LogOut(userID);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new EventMessage { Message = "Error en el servicio LogOut - Contacte con el administrador" });
            }
        }

        /// <summary>
        /// Cerrar la sesión del usuario por tiempo
        /// </summary>
        /// <returns>Valido</returns>
        [Authorize]
        [HttpGet("TimeOut")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EventMessage), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult TimeOut()
        {
            try
            {
                var claim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.UserData);
                Guid userID = claim == null ? Guid.Empty : Guid.Parse(claim.Value);
                _loginService.TimeOut(userID);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new EventMessage { Message = "Error en el servicio TimeOut - Contacte con el administrador" });
            }
        }
    }
}
