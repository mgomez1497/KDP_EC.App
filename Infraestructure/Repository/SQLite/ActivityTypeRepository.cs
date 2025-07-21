using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class ActivityTypeRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public ActivityTypeRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<ActivityType>();
        }

        public async Task SaveActivityTypesLocally(List<ActivityType> activityTypes)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<ActivityType>();
            await _db.InsertAllAsync(activityTypes);
            await GetLocalActivityTypes();
        }

        public async Task<List<ActivityType>>GetAllActivityType()
        {
            await InitializeAsync();
            var result = await _db.Table<ActivityType>().ToListAsync();
            return result;
        }

        public async Task<List<ActivityType>> GetLocalActivityTypes()
        {
            await InitializeAsync();
            return await _db.Table<ActivityType>().ToListAsync();
        }


    }
}
