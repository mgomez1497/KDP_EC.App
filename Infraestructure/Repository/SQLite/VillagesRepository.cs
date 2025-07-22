using KDP_EC.App.Infraestructure.API;
using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class VillagesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public VillagesRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Villages>();
        }

        public async Task<List<Villages>> ObtenerVeredasporId(string Id)
        {
            var api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/Villages/");
            try
            {
                var villagesResult = await api.GetAsync<List<Villages>>($"getVillages?id={Id}");
                return villagesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las veredas: {ex.Message}");
                return new List<Villages>();
            }


        }

        public async Task GuardarVeredasLocal(List<Villages> villages)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Villages>();
            await _db.InsertAllAsync(villages);
            

        }

        public async Task<List<Villages>> GetLocalVeredas(Guid villageId)
        {
            await InitializeAsync();
            var localVillages = await _db.Table<Villages>()
                                         .Where(v => v.Id == villageId)
                                         .ToListAsync();
            return localVillages;
        }
    }
}
