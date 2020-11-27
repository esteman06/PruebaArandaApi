using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PruebaAranda.DataModel.Model;
using PruebaAranda.Entitis;
using PruebaArandaApi.Helpers;
using PruebaArandaApi.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaArandaApi.Service
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly AppSettings _appSettings;
        private readonly PruebaArandaContext _context;
        public IdentityUserService(IOptions<AppSettings> appSettings, PruebaArandaContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public async Task<int> CreateIdentityUser(IdentityUserView identityUserView)
        {
            int result = 0;
            try
            {
                IdentityUser identityUser = await _context.IdentityUser.Where(x => x.IdentityUserId.Equals(identityUserView.IdentityUserId)).FirstOrDefaultAsync();
                if (identityUser == null)
                {
                    result = 1;
                    Guid newGuid = Guid.NewGuid();
                    IdentityUser newIdentityUser = new IdentityUser
                    {
                        IdentityUserId = newGuid,
                        Name = identityUserView.Name,
                        Password = identityUserView.Password,
                        IsActive = identityUserView.IsActive,
                        IsTempPassword = identityUserView.IsTempPassword
                    };
                    _context.IdentityUser.Add(newIdentityUser);
                    await _context.SaveChangesAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }

            return result;
        }
        public async Task<int> UpdateIdentityUser(IdentityUserView identityUserView)
        {
            int result = 0;
            try
            {
                IdentityUser identityUser = await _context.IdentityUser.Where(x => x.IdentityUserId.Equals(identityUserView.IdentityUserId)).FirstOrDefaultAsync();
                if (identityUser != null)
                {
                    result = 1;
                    identityUser.Name = identityUserView.Name;
                    identityUser.Password = identityUserView.Password;
                    identityUser.IsActive = identityUserView.IsActive;
                    identityUser.IsTempPassword = identityUserView.IsTempPassword;
                    _context.Entry(identityUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }

            return result;
        }
        public async Task<int> RemoveIdentityUser(Guid identityUserId)
        {
            int result = 0;
            try
            {
                IdentityUser identityUser = await _context.IdentityUser.Where(x => x.IdentityUserId.Equals(identityUserId)).FirstOrDefaultAsync();
                if (identityUser != null)
                {
                    result = 1;

                    _context.IdentityUser.Remove(identityUser);
                    await _context.SaveChangesAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }

            return result;
        }
    }
}
