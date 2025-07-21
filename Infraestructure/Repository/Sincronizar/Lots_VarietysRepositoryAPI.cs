using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class Lots_VarietysRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public Lots_VarietysRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/Lots_Varietys/");
        }
        public async Task<List<Lots_Varietys>> ObtenerVariedadesLotes()
        {
            try
            {
                var lotVarietyResults = await _api.GetAsync<List<Lots_Varietys>>("getLotsVarietys");
                return lotVarietyResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las variedades de lotes: {ex.Message}");
                return new List<Lots_Varietys>();
            }
        }
    }
}
