using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class ActivitiesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public ActivitiesRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Activities>();
        }
        public async Task SaveActivitiesLocally(List<Activities> activities)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Activities>();
            await _db.InsertAllAsync(activities);
            await GetAllActivities();
        }
        public async Task<List<Activities>> GetAllActivities()
        {
            await InitializeAsync();
            var result = await _db.Table<Activities>().ToListAsync();
            return result;
        }

        public async Task<List<Activities>> GetLocalActivities()
        {
            await InitializeAsync();
            return await _db.Table<Activities>().ToListAsync();
        }

        public async Task<List<Activities>> GetActivitiesByCostCenterId(Guid CostCenterId)
        {
            await InitializeAsync();

            var localactivities = await _db.Table<Activities>()
                .Where(A => A.CostCenterId == CostCenterId)
                .ToListAsync();
            return localactivities;

        }


    }
}
