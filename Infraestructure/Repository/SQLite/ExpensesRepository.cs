using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class ExpensesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public ExpensesRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Expenses>();

        }
        public async Task SaveExpensesLocally(List<Expenses> expenses)
        {
            await InitializeAsync();

            foreach (var expense in expenses)
            {
                expense.Id = Guid.NewGuid();
                expense.CreatedAt = DateTime.UtcNow;
                await _db.InsertAsync(expense);
            }
        }
        public async Task<List<Expenses>> GetAllExpenses()
        {
            await InitializeAsync();
            var result = await _db.Table<Expenses>().ToListAsync();
            return result;
        }
        public async Task<List<Expenses>> GetLocalExpenses()
        {
            await InitializeAsync();
            return await _db.Table<Expenses>().ToListAsync();
        }

        public async Task<List<Expenses>> GetLocalExpensesByFarmIdAndYear(Guid farmId, int year)
        {
            await InitializeAsync();


            var allExpenses = await _db.Table<Expenses>()
                .Where(e => e.FarmId == farmId)
                .ToListAsync();


            var filteredExpenses = allExpenses?
                .Where(e => e != null && e.Date != null && e.Date.Value.Year == year)
                .ToList();


            return filteredExpenses ?? new List<Expenses>();
        }

        public async Task<List<Expenses>> GetLocalExpensesByFarmId(Guid farmId)
        {
            await InitializeAsync();
            var allExpenses = await _db.Table<Expenses>()
                .Where(e => e.FarmId == farmId)
                .ToListAsync();
            return allExpenses ?? new List<Expenses>();



        }
    }
}
