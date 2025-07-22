using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class StageOfCultRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public StageOfCultRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/StageOfCult/");
        }
        public async Task<List<StageOfCult>> ObtenerEtapasDeCultivo()
        {
            try
            {
                var stageOfCultResults = await _api.GetAsync<List<StageOfCult>>("getStagesOfCult");
                return stageOfCultResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las etapas de cultivo: {ex.Message}");
                return new List<StageOfCult>();
            }
        }
    }
}
