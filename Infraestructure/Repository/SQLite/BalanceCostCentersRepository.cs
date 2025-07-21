using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class BalanceCostCentersRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public BalanceCostCentersRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<BalanceCostCenters>();
        }

        public async Task SaveBalanceCostCentersLocally(List<BalanceCostCenters> balanceCostCenters)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<BalanceCostCenters>();
            await _db.InsertAllAsync(balanceCostCenters);
        }


        public async Task<List<BalanceCostCenters>> GetLocalBalanceCostCentersById()
        {
            await InitializeAsync();
            return await _db.Table<BalanceCostCenters>().ToListAsync();
        }


        public async Task<List<BalanceCostCenters>> GetLocalBalanceCostCentersByFarmIdAndYear(Guid FarmId, int year)
        {
            await InitializeAsync();
            var localBalanceCostCenters = await _db.Table<BalanceCostCenters>()
                                                   .Where(s => s.FarmId == FarmId && s.Year == year)
                                                   .ToListAsync();
            return localBalanceCostCenters;
        }

    }
}
