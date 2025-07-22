using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class ProductivityReportRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public ProductivityReportRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/ProductivityReport/");
        }

        public async Task<List<ProductivityReport>> ObtenerProductivityReports(Guid FarmId)
        {
            try
            {
                var productivityReportsResult = await _api.GetAsync<List<ProductivityReport>>($"getProductivityReportByFarm_MultiYear?FarmId={FarmId}");
                return productivityReportsResult ?? new List<ProductivityReport>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los informes de productividad: {ex.Message}");
                return new List<ProductivityReport>();
            }
        }


    }
}
