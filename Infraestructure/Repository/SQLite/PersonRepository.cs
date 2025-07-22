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
    public class PersonRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public PersonRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync(); // Si tu clase base lo maneja
            await _db.CreateTableAsync<Person>();
        }
        // Aquí puedes agregar métodos para interactuar con la tabla de personas

        public async Task<List<Person>> GetPersons()
        {
            var api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/Person/");
            try
            {
                var personsResult = await api.GetAsync<List<Person>>("getPersons");
                return personsResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las personas: {ex.Message}");
                return new List<Person>();
            }
        }
        public async Task SavePersonsLocally(List<Person> persons)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Person>();
            await _db.InsertAllAsync(persons);
            await GetLocalPersons();
        }
        public async Task<List<Person>> GetLocalPersons()
        {
            await InitializeAsync();
            return await _db.Table<Person>().ToListAsync();
        }
    }
    
}
