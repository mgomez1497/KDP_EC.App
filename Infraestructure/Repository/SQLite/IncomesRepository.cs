using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class IncomesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public IncomesRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Incomes>();
        }

        public async Task SaveIncomesLocally(List<Incomes> incomes)
        {
            await InitializeAsync();
            foreach (var income in incomes)
            {
                income.Id = Guid.NewGuid();
                income.CreatedAt = DateTime.UtcNow;
                await _db.InsertAsync(income);
            }
        }

        public async Task<List<Incomes>> GetAllIncomes()
        {
            await InitializeAsync();
            var result = await _db.Table<Incomes>().ToListAsync();
            return result;
        }

        public async Task<List<Incomes>> GetLocalIncomes()
        {
            await InitializeAsync();
            return await _db.Table<Incomes>().ToListAsync();
        }

        public async Task<List<Incomes>> GetLocalIncomesByFarmIdAndYear(Guid farmId, int year)
        {
            await InitializeAsync();
            var allIncomes = await _db.Table<Incomes>()
                .Where(i => i.FarmId == farmId)
                .ToListAsync();
            var filteredIncomes = allIncomes?
                .Where(i => i != null && i.Date != null && i.Date.Value.Year == year)
                .ToList();
            return filteredIncomes;
        }

        public async Task<List<Incomes>> GetLocalIncomesByFarmId(Guid farmId)
        {
            await InitializeAsync();
            var allIncomes = await _db.Table<Incomes>()
               .Where(i => i.FarmId == farmId)
               .ToListAsync();
            return allIncomes;
        }
    }
}
