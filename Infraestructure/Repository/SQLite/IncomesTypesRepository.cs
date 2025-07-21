using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class IncomesTypesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public IncomesTypesRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<IncomesTypes>();
        }

        public async Task SaveIncomesTypesLocally(List<IncomesTypes> incomesTypes)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<IncomesTypes>();
            await _db.InsertAllAsync(incomesTypes);
            await GetAllIncomesTypes();
        }
        public async Task<List<IncomesTypes>> GetAllIncomesTypes()
        {
            await InitializeAsync();
            var result = await _db.Table<IncomesTypes>().ToListAsync();
            return result;
        }

        public async Task<List<IncomesTypes>> GetLocalIncomesTypesById(Guid incomeTypeId)
        {
            await InitializeAsync();
            var localIncomesTypes = await _db.Table<IncomesTypes>()
                                             .Where(i => i.Id == incomeTypeId)
                                             .ToListAsync();
            return localIncomesTypes;
        }

        public async Task<List<IncomesTypes>> GetLocalIncomesTypes()
        {
            await InitializeAsync();
            return await _db.Table<IncomesTypes>().ToListAsync();
        }



    }
}
