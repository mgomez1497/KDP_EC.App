using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class Renewal_TypesRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public Renewal_TypesRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/Renewal_Types/");
        }

        public async Task<List<Renewal_Types>> ObtenerTiposRenovacion()
        {
            try
            {
                var renewalTypeResults = await _api.GetAsync<List<Renewal_Types>>("getRenewalTypes");
                return renewalTypeResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los tipos de renovación: {ex.Message}");
                return new List<Renewal_Types>();
            }
        }
    }
}
