using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class CostCenterRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public CostCenterRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/CostCenter/");
        }
        public async Task<List<CostCenter>> ObtenerCentrosDeCosto()
        {
            try
            {
                var costCenterResults = await _api.GetAsync<List<CostCenter>>("getCostCenters");
                return costCenterResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los centros de costo: {ex.Message}");
                return new List<CostCenter>();
            }
        }
    }
}
