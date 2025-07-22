using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class CitiesRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public CitiesRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/Cities/");
        }
        public async Task<List<Cities>> ObtenerCiudades()
        {
            try
            {
                var citiesResult = await _api.GetAsync<List<Cities>>("getCities");
                return citiesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las ciudades: {ex.Message}");
                return new List<Cities>();
            }
        }
    }
}
