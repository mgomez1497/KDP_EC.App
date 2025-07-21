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
    public class CountriesRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public CountriesRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Countries>();
        }
        public async Task<List<Countries>> GetCountries()
        {
            var api = new ConsumoApi("https://localhost:7149/api/Country/");

            try
            {
                var countriesResult = await api.GetAsync<List<Countries>>("getCountries");
                return countriesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los paises: {ex.Message}");
                return new List<Countries>();
            }
        }

        public async Task SaveCoutriesLocally(List<Countries> countries)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Countries>();
            await _db.InsertAllAsync(countries);
            await GetLocalCountries();
        }

        public async Task<List<Countries>> GetLocalCountries()
        {
            await InitializeAsync();
            return await _db.Table<Countries>().ToListAsync();
        }
    }
}
