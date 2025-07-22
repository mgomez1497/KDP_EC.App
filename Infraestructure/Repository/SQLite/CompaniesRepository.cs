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
    public class CompaniesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public CompaniesRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync(); // Si tu clase base lo maneja
            await _db.CreateTableAsync<Company>();
        }

        public async Task<List<Company>> GetCompanies()
        {
            var api = new ConsumoApi("https://localhost:7149/api/Company/");

            try
            {
                var companiesResult = await api.GetAsync<List<Company>>("getCompanies");
                return companiesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las compañías: {ex.Message}");
                return new List<Company>(); 
            }
        }

        public async Task SaveCompaniesLocally(List<Company> companies)
        {
            await InitializeAsync();

            await _db.DeleteAllAsync<Company>();

            foreach (var company in companies)
            {
                await _db.InsertAsync(company);
            }
        }
        public async Task<List<Company>> GetLocalCompanies(Guid companyId)
        {
            await InitializeAsync();
            var localCompanies = await _db.Table<Company>()
                                         .Where(c => c.Id == companyId)
                                         .ToListAsync();
            return localCompanies;
        }
    }
}
