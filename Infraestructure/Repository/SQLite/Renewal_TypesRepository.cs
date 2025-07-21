using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Interfaces;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class Renewal_TypesRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public Renewal_TypesRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Renewal_Types>();
        }

        public async Task SaveRenewal_TypesLocally(List<Renewal_Types> renewal_Types)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Renewal_Types>();
            await _db.InsertAllAsync(renewal_Types);
            await GetAllRenewal_Types();
        }

        public async Task<List<Renewal_Types>> GetAllRenewal_Types()
        {
            await InitializeAsync();
            var result = await _db.Table<Renewal_Types>().ToListAsync();
            return result;
        }

        public async Task<List<Renewal_Types>> GetLocalRenewal_TypesById(Guid renewalTypeId)
        {
            await InitializeAsync();
            var localRenewal_Types = await _db.Table<Renewal_Types>()
                                              .Where(r => r.Id == renewalTypeId)
                                              .ToListAsync();
            return localRenewal_Types;



        }
    }
}
