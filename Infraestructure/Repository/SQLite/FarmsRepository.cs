
using KDP_EC.App.Infraestructure.API;
using KDP_EC.App.Infraestructure.Database;
using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.Core.Models;
using KDP_EC.Core.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmModel = KDP_EC.Core.Models.Farms;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class FarmsRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public FarmsRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Farms>();
        }

        public async Task SaveFarmsLocally(List<Farms> farms)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Farms>();
            await _db.InsertAllAsync(farms);

            await GetLocalFarms();
        }

        public async Task<List<Farms>> GetLocalFarms()
        {
            await InitializeAsync();
            return await _db.Table<Farms>().ToListAsync();
        }

        public async Task<List<Farms>> GetLocalFarmsById(Guid farmId)
        {
            await InitializeAsync();
            var localFarms = await _db.Table<Farms>()
                                      .Where(f => f.Id == farmId)
                                      .ToListAsync();
            return localFarms;
        }

        public async Task UpdateFarm(Farms farm)
        {
            await InitializeAsync();
            await _db.UpdateAsync(farm);
        }

        public async Task<bool> ActualizarUbicacionFincaAP(Guid farmId)
        {
            var farm = await _db.Table<Farms>().FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm != null)
            {
                var updateModel = new FarmUpdateViewModel
                {
                    Id = farm.Id,
                    Latitude = farm.Latitude,
                    Longitude = farm.Longitude,
                    UpdatedAt = DateTime.Now
                };

                var repo = new FarmsRepositoryAPI();
                var resultado = await repo.SincronizarFinca(updateModel);
                return resultado;
            }

            
            return false;
        }
    }
}
