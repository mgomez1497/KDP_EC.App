using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class StatesRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public StatesRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/States/");
        }
        public async Task<List<States>> ObtenerEstados()
        {
            try
            {
                var statesResult = await _api.GetAsync<List<States>>("getStates");
                return statesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los estados: {ex.Message}");
                return new List<States>();
            }
        }
    }
}
