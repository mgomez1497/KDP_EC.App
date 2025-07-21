using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class CostCenterRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public CostCenterRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<CostCenter>();
        }
        public async Task SaveCostCentersLocally(List<CostCenter> costCenters)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<CostCenter>();
            await _db.InsertAllAsync(costCenters);
            await GetAllCostCenters();
        }
        public async Task<List<CostCenter>> GetAllCostCenters()
        {
            await InitializeAsync();
            var result = await _db.Table<CostCenter>().ToListAsync();
            return result;
        }
        public async Task<List<CostCenter>> GetLocalCostCenters()
        {
            await InitializeAsync();
            return await _db.Table<CostCenter>().ToListAsync();
        }

        public async Task<List<CostCenter>> GetLocalCostCenter(Guid StageOfCultId)
        {
            await InitializeAsync();
            var localCostCenters = await _db.Table<CostCenter>()
                .Where(c => c.StageOfCultId == StageOfCultId)
                .ToListAsync();
            return localCostCenters;
        }
    }
}
