using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class IncomesTypesRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public IncomesTypesRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/IncomesTypes/");
        }

        public async Task<List<IncomesTypes>>ObtenerTiposIngreso()
        {
            try
            {
                var incomeTypes = await _api.GetAsync<List<IncomesTypes>>("getIncomesTypes");
                return incomeTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los tipos de Ingresos: {ex.Message}");
                return new List<IncomesTypes>();
            }
        }
    }
}
