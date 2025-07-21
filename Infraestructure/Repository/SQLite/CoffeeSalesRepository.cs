using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class CoffeeSalesRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public CoffeeSalesRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<CoffeeSalesRep>();
        }

        public async Task SaveCoffeeSalesLocally(List<CoffeeSalesRep> coffeeSales)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<CoffeeSalesRep>();
            await _db.InsertAllAsync(coffeeSales);
        }

        public async Task<List<CoffeeSalesRep>> GetLocalCoffeeSalesById()
        {
            await InitializeAsync();
            return await _db.Table<CoffeeSalesRep>().ToListAsync();
        }

        public async Task <List<CoffeeSalesRep>> GetLocalCoffeeSalesByFarmIdAndYear(Guid FarmId, int year)
        {
            await InitializeAsync();
            var localCoffeeSales = await _db.Table<CoffeeSalesRep>()
                                            .Where(s => s.FarmId == FarmId && s.Year == year)
                                            .ToListAsync();
            return localCoffeeSales;
        }

    }
}
