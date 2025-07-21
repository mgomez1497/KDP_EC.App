using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class Lots_TypeRepository: SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public Lots_TypeRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Lots_Type>();
        }

        public async Task SaveLots_TypesLocally(List<Lots_Type> lots_Types)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Lots_Type>();
            await _db.InsertAllAsync(lots_Types);

            await GetAllLots_Types();
        }

        public async Task<List<Lots_Type>> GetAllLots_Types()
        {
            await InitializeAsync();
            var result = await _db.Table<Lots_Type>().ToListAsync();
            return result;
        }

        public async Task<List<Lots_Type>> GetLocalLots_TypesById(Guid lotTypeId)
        {
            await InitializeAsync();
            var localLots_Types = await _db.Table<Lots_Type>()
                                           .Where(l => l.Id == lotTypeId)
                                           .ToListAsync();
            return localLots_Types;
        }


    }
}
