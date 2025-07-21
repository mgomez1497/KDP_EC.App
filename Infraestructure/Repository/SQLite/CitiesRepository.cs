using KDP_EC.App.Infraestructure.API;
using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class CitiesRepository :SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public CitiesRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync(); // Si tu clase base lo maneja
            await _db.CreateTableAsync<Cities>();
        }
        public async Task<List<Cities>> GetCities()
        {
            var api = new ConsumoApi("https://localhost:7149/api/Cities/");
            try
            {
                var citiesResult = await api.GetAsync<List<Cities>>("getCities");
                return citiesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las ciudades: {ex.Message}");
                return new List<Cities>();
            }
        }
        public async Task SaveCitiesLocally(List<Cities> cities)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Cities>();
            await _db.InsertAllAsync(cities);
        }
        public async Task<List<Cities>> GetLocalCities(Guid cityId)
        {
            await InitializeAsync();
            var localCities = await _db.Table<Cities>()
                                       .Where(c => c.Id == cityId)
                                       .ToListAsync();
            return localCities;
        }

        public async Task<List<Cities>> GetLocalCitiesByState(Guid stateId)
        {
            await InitializeAsync();
            var localCities = await _db.Table<Cities>()
                                       .Where(c => c.StateId == stateId)
                                       .ToListAsync();

            Debug.WriteLine("Departamento seleccionado: " + stateId);

            var testCities = await _db.Table<Cities>().ToListAsync();
            foreach (var c in testCities.Take(10))
            {
                Debug.WriteLine($"City: {c.Name} - StateId: {c.StateId}");
            }
            return localCities;
        }
    }
}
