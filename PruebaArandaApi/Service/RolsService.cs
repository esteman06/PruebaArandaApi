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
    public class RolsService : IRolsService
    {
        private readonly AppSettings _appSettings;
        private readonly PruebaArandaContext _context;
        public RolsService(IOptions<AppSettings> appSettings, PruebaArandaContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public async Task<List<RolsView>> GetAllRols()
        {
            try
            {
                List<RolsView> rolsViews = new List<RolsView>();
                var tempList = await _context.Rols.ToListAsync();
                if (tempList.Count > 0)
                {
                    foreach (Rols item in tempList)
                    {
                        RolsView rolsView = MapRols(item);
                        rolsViews.Add(rolsView);
                    }
                }
                return rolsViews.OrderBy(T => T.RolNameApplication).ToList();
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }
            return null;
        }

        private RolsView MapRols(Rols rols)
        {
            RolsView rolsView = new RolsView()
            {
                RolsId = rols.RolsId,
                RolNameSystem = rols.RolNameSystem,
                RolNameApplication = rols.RolNameApplication
            };
            return rolsView;
        }
    }
}
