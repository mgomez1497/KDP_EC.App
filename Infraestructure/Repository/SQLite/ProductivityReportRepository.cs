using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class ProductivityReportRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public ProductivityReportRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<ProductivityReport>();
        }

        public async Task SaveProductivityReportsLocally(List<ProductivityReport> productivityReports)
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<ProductivityReport>();
            await _db.InsertAllAsync(productivityReports);
        }

        public async Task<List<ProductivityReport>> GetLocalProductivityReportsById()
        {
            await InitializeAsync();
            return await _db.Table<ProductivityReport>().ToListAsync();
        }

        public async Task<List<ProductivityReport>> GetLocalProductivityReportsByFarmIdAndYear(Guid FarmId, int year)
        {
            await InitializeAsync();
            var localProductivityReports = await _db.Table<ProductivityReport>()
                                                    .Where(r => r.FarmId == FarmId && r.Year == year)
                                                    .ToListAsync();
            return localProductivityReports;
        }
    }
}
