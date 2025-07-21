using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class FloweringRecordsRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public FloweringRecordsRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
           
            await _db.CreateTableAsync<FloweringRecords>();

        }

        public async Task SaveFloweringRecordsLocally(List<FloweringRecords> floweringRecords)
        {
            await InitializeAsync();
            foreach (var record in floweringRecords)
            {
                var existing = await _db.Table<FloweringRecords>().Where(x => x.Id == record.Id).FirstOrDefaultAsync();
                if (existing != null)
                {
                    record.CreatedAt = existing.CreatedAt;
                    record.UpdatedAt = DateTime.UtcNow;
                    await _db.UpdateAsync(record);
                }
                else
                {
                    record.CreatedAt = DateTime.UtcNow;
                    
                    await _db.InsertAsync(record);
                }
            }
        }
        public async Task<List<FloweringRecords>>GetLocalFloweringRecords()
        {
            await InitializeAsync();
            return await _db.Table<FloweringRecords>().ToListAsync();
        }



      




    }
}
