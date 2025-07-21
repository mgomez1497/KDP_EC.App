using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class LotsRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public LotsRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/Lots/");
        }

        public async Task<List<Lots>> ObtenerLotesporFarmId(Guid farmId)
        {
            try
            {
                var lotResults = await _api.GetAsync<List<Lots>>($"getLotsByFarmIdApi?FarmId={farmId}");
                return lotResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los lotes: {ex.Message}");
                return new List<Lots>();
            }
        }

        public async Task<bool> SincronizarLotes(List<Lots> lots)
        {
            try
            {
                var response = await _api.PostAsync<List<Lots>, HttpResponseMessage>("createLots", lots);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al sincronizar los lotes: {ex.Message}");
                return false;
            }
        }

    }
}
