using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class Lots_VarietysRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public Lots_VarietysRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Lots_Varietys>();
            
        }
        public async Task SaveLots_VarietysLocally(List<Lots_Varietys> lots_Varietys)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Lots_Varietys>();
            await _db.InsertAllAsync(lots_Varietys);
            await GetAllLots_Varietys();
        }
        public async Task<List<Lots_Varietys>> GetAllLots_Varietys()
        {
            await InitializeAsync();
            var result = await _db.Table<Lots_Varietys>().ToListAsync();
            return result;
        }

        public async Task<List<Lots_Varietys>> GetLocalLots_VarietysById(Guid lotVarietyId)
        {
            await InitializeAsync();
            var localLots_Varietys = await _db.Table<Lots_Varietys>()
                                               .Where(l => l.Id == lotVarietyId)
                                               .ToListAsync();
            return localLots_Varietys;
        }
    }
}
