using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class Lots_TypeRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public Lots_TypeRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/Lots_Type/");
        }

        public async Task<List<Lots_Type>> ObtenerTiposLotes()
        {
            try
            {
                var lotTypeResults = await _api.GetAsync<List<Lots_Type>>("getLotsType");
                return lotTypeResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los tipos de lotes: {ex.Message}");
                return new List<Lots_Type>();
            }
        }



    }
}
