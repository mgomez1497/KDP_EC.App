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
    public class StatesRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public StatesRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync(); // Si tu clase base lo maneja
            await _db.CreateTableAsync<States>();
        }
        public async Task<List<States>> GetStates()
        {
            var api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/States/");
            try
            {
                var statesResult = await api.GetAsync<List<States>>("getStates");
                return statesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los estados: {ex.Message}");
                return new List<States>();
            }
        }
        public async Task SaveStatesLocally(List<States> states)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<States>();
            await _db.InsertAllAsync(states);

        }
        public async Task<List<States>> GetLocalStates()
        {
            await InitializeAsync();
            var localStates = await _db.Table<States>().ToListAsync();
            return localStates;
        }
    }
}
