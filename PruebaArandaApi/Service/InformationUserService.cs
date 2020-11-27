using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PruebaAranda.DataModel.Model;
using PruebaAranda.Entitis;
using PruebaArandaApi.Helpers;
using PruebaArandaApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaArandaApi.Service
{
    public class InformationUserService : IInformationUserService
    {
        private readonly AppSettings _appSettings;
        private readonly PruebaArandaContext _context;
        public InformationUserService(IOptions<AppSettings> appSettings, PruebaArandaContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public async Task<int> CreateInfoUser(InformationUserView informationUserView)
        {
            int result = 0;
            try
            {
                InformationUser informationUser = await _context.InformationUser.Where(x => x.IdentityUserId.Equals(informationUserView.IdentityUserId)).FirstOrDefaultAsync();
                if (informationUser == null)
                {
                    result = 1;

                    Guid newIdentityUserGuid = Guid.NewGuid();
                    IdentityUser newIdentityUser = new IdentityUser
                    {
                        IdentityUserId = newIdentityUserGuid,
                        Name = informationUserView.FirstName,
                        Password = new LoginUtils().Encrypt(informationUserView.Password),
                        IsActive = true,
                        IsTempPassword = true,
                    };
                    _context.IdentityUser.Add(newIdentityUser);
                    await _context.SaveChangesAsync();

                    Guid newGuid = Guid.NewGuid();
                    InformationUser newInformationUser = new InformationUser
                    {
                        InformationUserId = newGuid,
                        IdentityUserId = newIdentityUserGuid,
                        FirstName = informationUserView.FirstName,
                        LastName = informationUserView.LastName,
                        Address = informationUserView.Address,
                        Phone = informationUserView.Phone,
                        Email = informationUserView.Email,
                        Age = informationUserView.Age,
                        RolsId = informationUserView.RolsId
                    };
                    _context.InformationUser.Add(newInformationUser);
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
        public async Task<int> UpdateInfoUser(InformationUserView informationUserView)
        {
            int result = 0;
            try
            {
                InformationUser informationUser = await _context.InformationUser.Where(x => x.IdentityUserId.Equals(informationUserView.IdentityUserId)).FirstOrDefaultAsync();
                if (informationUser != null)
                {
                    result = 1;

                    IdentityUser identityUser = await _context.IdentityUser.Where(x => x.IdentityUserId.Equals(informationUserView.IdentityUserId)).FirstOrDefaultAsync();
                    if (identityUser != null)
                    {
                        result = 1;
                        identityUser.Name = informationUserView.FirstName;
                        identityUser.Password = informationUserView.Password;
                        _context.Entry(identityUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    informationUser.FirstName = informationUserView.FirstName;
                    informationUser.LastName = informationUserView.LastName;
                    informationUser.Address = informationUserView.Address;
                    informationUser.Phone = informationUserView.Phone;
                    informationUser.Email = informationUserView.Email;
                    informationUser.Age = informationUserView.Age;
                    _context.Entry(informationUser).State = EntityState.Modified;
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
        public async Task<InformationUserView> GetInfoUserById(Guid informationUserId)
        {
            try
            {
                var informationUser = await _context.InformationUser.Where(x => x.InformationUserId.Equals(informationUserId)).FirstOrDefaultAsync();
                if (informationUser != null)
                {
                    

                    InformationUserView informationUserView = MapInformationUser(informationUser);
                    return informationUserView;
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }
            return null;
        }
        public async Task<List<InformationUserView>> GetInfoUserByNameOrRolsId(string name, Guid? rolsId)
        {
            try
            {
                List<InformationUserView> informationUserViews = new List<InformationUserView>();
                List<InformationUser> tempList;
                
                if (name == null && rolsId == null) 
                {
                    tempList = await _context.InformationUser.ToListAsync();
                }
                else 
                {
                    tempList = await _context.InformationUser.Where(x => (x.FirstName.Contains(name) || x.LastName.Contains(name)) || x.RolsId.Equals(rolsId)).ToListAsync();
                }
                
                
                if (tempList.Count > 0)
                {
                    foreach (InformationUser item in tempList)
                    {
                        InformationUserView informationUserView = MapInformationUser(item);
                        informationUserViews.Add(informationUserView);
                    }
                }
                return informationUserViews.OrderBy(T => T.FirstName).ToList();
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }
            return null;
        }
        public async Task<int> RemoveInfoUser(Guid InformationUserId)
        {
            int result = 0;
            try
            {
                InformationUser informationUser = await _context.InformationUser.Where(x => x.InformationUserId.Equals(InformationUserId)).FirstOrDefaultAsync();
                if (informationUser != null)
                {
                    result = 1;

                    _context.InformationUser.Remove(informationUser);
                    await _context.SaveChangesAsync();

                    IdentityUser identityUser = await _context.IdentityUser.Where(x => x.IdentityUserId.Equals(informationUser.IdentityUserId)).FirstOrDefaultAsync();
                    if (identityUser != null)
                    {
                        _context.IdentityUser.Remove(identityUser);
                        await _context.SaveChangesAsync();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }

            return result;
        }
        private InformationUserView MapInformationUser(InformationUser informationUser)
        {
            InformationUserView informationUserView = new InformationUserView()
            {
                InformationUserId = informationUser.InformationUserId,
                IdentityUserId = informationUser.IdentityUserId,
                FirstName = informationUser.FirstName,
                LastName = informationUser.LastName,
                Address = informationUser.Address,
                Phone = informationUser.Phone,
                Email = informationUser.Email,
                Age = informationUser.Age,
                RolsId = informationUser.RolsId
            };
            return informationUserView;
        }

    }
}
