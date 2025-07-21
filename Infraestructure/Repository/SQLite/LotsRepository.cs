using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class LotsRepository:SqLiteConnection
    {
        private readonly HttpClient _httpClient;

        public LotsRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Lots>();
        }

        public async Task SaveLotsLocally(List<Lots> lots)
        {
            await InitializeAsync();
           

            foreach (var lot in lots)
            {
                var existing = await _db.Table<Lots>().Where(x => x.Id == lot.Id).FirstOrDefaultAsync();
                if (existing != null)
                {
                    lot.CreatedAt = existing.CreatedAt;
                    lot.UpdatedAt = DateTime.UtcNow;
                    await _db.UpdateAsync(lot);
                }
                else
                {
                    
                    lot.CreatedAt = DateTime.UtcNow;
                    lot.UpdatedAt = null;
                    await _db.InsertAsync(lot);
                }
            }
        }
        public async Task<List<Lots>> GetLocalLots()
        {
            await InitializeAsync();
            return await _db.Table<Lots>().ToListAsync();
        }

        public async Task<List<Lots>> GetLocalLotsById(Guid lotId)
        {
            await InitializeAsync();
            var localLots = await _db.Table<Lots>()
                                      .Where(l => l.Id == lotId)
                                      .ToListAsync();
            return localLots;
        }

        public async Task<List<Lots>> GetLocalLotsByFarmId(Guid FarmId)
        {
            await InitializeAsync();
            var localLots = await _db.Table<Lots>()
                                      .Where(l => l.FarmId == FarmId)
                                      .ToListAsync();
            return localLots;
        }

        public async Task<List<Lots>> DeletedLotsLocallyById(Guid lotId)
        {
            await InitializeAsync();

            
            var lote = await _db.Table<Lots>()
                                .Where(l => l.Id == lotId)
                                .FirstOrDefaultAsync();

            if (lote != null)
            {
                lote.DeletedAt = DateTime.Now;
                await _db.UpdateAsync(lote);
            }

           
            var activeLots = await _db.Table<Lots>()
                                      .Where(l => l.FarmId == lote.FarmId && l.DeletedAt == null)
                                      .ToListAsync();

            return activeLots;
        }

        public async Task DeleteAllLots()
        {
            await InitializeAsync();
            await _db.DeleteAllAsync<Lots>();
        }

    }
}
