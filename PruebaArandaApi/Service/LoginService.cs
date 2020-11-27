using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PruebaAranda.DataModel.Model;
using PruebaAranda.Entitis;
using PruebaArandaApi.Helpers;
using PruebaArandaApi.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PruebaArandaApi.Service
{
    public class LoginService : ILoginService
    {
        private readonly AppSettings _appSettings;
        private readonly PruebaArandaContext _context;
        private IHttpContextAccessor _accessor;

        public LoginService(IOptions<AppSettings> appSettings, PruebaArandaContext context, IHttpContextAccessor accessor)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _accessor = accessor;
        }

        public async Task<IdentityUserView> LogIn(string username, string password)
        {
            try
            {
                //Encriptar el password
                password = new LoginUtils().Encrypt(password);

                var user = await _context.IdentityUser.FirstOrDefaultAsync(x => x.Name.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Password.Trim().Equals(password.Trim()));
                if (user != null)
                {
                    Guid rolId = await _context.InformationUser.Where(x => x.IdentityUserId.Equals(user.IdentityUserId)).Select(x => x.RolsId).FirstOrDefaultAsync();
                    string rolName = await _context.Rols.Where(x => x.RolsId.Equals(rolId)).Select(x => x.RolNameApplication).FirstOrDefaultAsync();
                    var auths = await _context.RolAuthorizations.Where(x => x.RolId.Equals(rolId)).ToListAsync();
                    bool authEdit = false;
                    bool authQuery = false;
                    bool authCreate = false;
                    bool authDelete = false;

                    if(auths != null) 
                    { 
                        foreach(var auth in auths) 
                        {
                            string authName = await _context.Authorization.Where(x => x.AuthorizationId.Equals(auth.AuthorizationId)).Select(x => x.AuthorizationNameSystem).FirstOrDefaultAsync();
                            if (authName == "Edit") 
                            {
                                authEdit = true;
                            }
                            if (authName == "Query")
                            {
                                authQuery = true;
                            }
                            if (authName == "Create")
                            {
                                authCreate = true;
                            }
                            if (authName == "Delete")
                            {
                                authDelete = true;
                            }
                        }
                    }


                    int timeExpiredToken = 360;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Name.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddHours(timeExpiredToken),

                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    string tokenText = tokenHandler.WriteToken(token);
                    IdentityUserView newIdentityUserView = new IdentityUserView()
                    {
                        IdentityUserId = user.IdentityUserId,
                        Name = user.Name,
                        IsActive = user.IsActive,
                        IsTempPassword = user.IsActive,
                        Token = tokenText,
                        RolsId = rolId,
                        RolName = rolName,
                        AuthEdit = authEdit,
                        AuthCreate = authCreate,
                        AuthDelete = authDelete,
                        AuthQuery = authQuery
                    };
                    ////Registrar el ingreso
                    SecurityLogService securityLogService = new SecurityLogService(_context, _accessor);
                    securityLogService.RegisterLogIn(user.IdentityUserId);
                    return newIdentityUserView;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public void LogOut(Guid userID)
        {
            //Registrar el ingreso
            SecurityLogService securityLogService = new SecurityLogService(_context, _accessor);
            securityLogService.RegisterLogOut(userID);
        }

        public void TimeOut(Guid userID)
        {
            //Registrar el ingreso
            SecurityLogService securityLogService = new SecurityLogService(_context, _accessor);
            securityLogService.RegisterTimeOut(userID);
        }

    }
}
