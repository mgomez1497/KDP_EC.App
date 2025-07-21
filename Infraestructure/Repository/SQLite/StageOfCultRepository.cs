using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class StageOfCultRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public StageOfCultRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<StageOfCult>();
        }
        public async Task SaveStageOfCultsLocally(List<StageOfCult> stageOfCults)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<StageOfCult>();
            await _db.InsertAllAsync(stageOfCults);
            await GetAllStageOfCults();
        }
        public async Task<List<StageOfCult>> GetAllStageOfCults()
        {
            await InitializeAsync();
            var result = await _db.Table<StageOfCult>().ToListAsync();
            return result;
        }
        public async Task<List<StageOfCult>> GetLocalStageOfCults()
        {
            await InitializeAsync();
            return await _db.Table<StageOfCult>().ToListAsync();
        }
        
    }
}
