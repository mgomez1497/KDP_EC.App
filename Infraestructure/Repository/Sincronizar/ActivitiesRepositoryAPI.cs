using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class ActivitiesRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public ActivitiesRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/Activities/");
        }
        public async Task<List<Activities>> ObtenerActividades()
        {
            try
            {
                var activityResults = await _api.GetAsync<List<Activities>>("getActivities");
                return activityResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las actividades: {ex.Message}");
                return new List<Activities>();
            }
        }
    }
}
