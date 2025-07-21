using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class BalanceCostCentersRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public BalanceCostCentersRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/BalanceCostCenters/");

        }

        public async Task<List<BalanceCostCenters>> ObtenerBalanceCC(Guid FarmId)
        {
            try
            {
                var balanceCostCentersResult = await _api.GetAsync<List<BalanceCostCenters>>($"getBalanceCostCentersByFarm?farmId={FarmId}");
                return balanceCostCentersResult ?? new List<BalanceCostCenters>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener el balance de centros de costo: {ex.Message}");
                return new List<BalanceCostCenters>();
            }
        }
    }
}
