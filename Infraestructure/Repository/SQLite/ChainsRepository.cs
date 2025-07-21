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
    public class ChainsRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public ChainsRepository()
        {
            _httpClient = new HttpClient();
        }


        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Chains>();
        }

        public async Task<List<Chains>> GetChains()
        {
            var api = new ConsumoApi("https://localhost:7149/api/Chain/");
            try
            {
                var chainsResult = await api.GetAsync<List<Chains>>("getChains");
                return chainsResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las cadenas: {ex.Message}");
                return new List<Chains>();
            }
        }

        public async Task SaveChainsLocally(List<Chains> chains)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Chains>();
            await _db.InsertAllAsync(chains);
            await GetLocalChains();
        }

        public async Task<List<Chains>> GetLocalChains()
        {
            await InitializeAsync();
            return await _db.Table<Chains>().ToListAsync();
        }
    }
}
