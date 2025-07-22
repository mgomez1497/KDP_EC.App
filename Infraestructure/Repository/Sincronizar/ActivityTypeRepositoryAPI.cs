using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class ActivityTypeRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public ActivityTypeRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/ActivityType/");
        }
        public async Task<List<ActivityType>> ObtenerTiposActividad()
        {
            try
            {
                var activityTypeResults = await _api.GetAsync<List<ActivityType>>("getActivityTypes");
                return activityTypeResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los tipos de actividad: {ex.Message}");
                return new List<ActivityType>();
            }
        }
    }
}
