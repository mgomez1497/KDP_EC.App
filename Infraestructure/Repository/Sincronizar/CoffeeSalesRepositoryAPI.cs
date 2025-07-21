using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class CoffeeSalesRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public CoffeeSalesRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/CoffeeSalesRep/");

            
        }

        public async Task<List<CoffeeSalesRep>> ObtenerVentasCafe(Guid FarmId)
        {
            try
            {
                var coffeeSalesResult = await _api.GetAsync<List<CoffeeSalesRep>>($"getCoffeeSalesReps?FarmId={FarmId}");
                return coffeeSalesResult ?? new List<CoffeeSalesRep>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las ventas de café: {ex.Message}");
                return new List<CoffeeSalesRep>();
            }
        }
    }
}
