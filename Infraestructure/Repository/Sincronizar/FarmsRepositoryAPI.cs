using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using KDP_EC.Core.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
     public class FarmsRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public FarmsRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/Farms/");
        }
        public async Task<List<FarmInfoViewModel>> ObtenerFincasporId(string Identification)
        {
            try
            {
                var farmsResult = await _api.GetAsync<List<FarmInfoViewModel>>($"getFarmsByPersonId?Identification={Identification}");
                return farmsResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las fincas: {ex.Message}");
                return new List<FarmInfoViewModel>();
            }
        }

        public async Task<List<Farms>> ObtenerFincasProductor(string Identification)
        {
            try
            {
                var farmResults = await _api.GetAsync<List<Farms>>($"getFarmbyIdentiAPI?identification={Identification}");
                return farmResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener las fincas: {ex.Message}");
                return new List<Farms>();
            }
        }

        public async Task<bool> SincronizarFinca(FarmUpdateViewModel updateModel)
        {
            try
            {
                var response = await _api.PostAsync<FarmUpdateViewModel, FarmsResponsesAPIViewModel>("UpdateFarmLocation", updateModel);
                if (response != null && response.Success)
                {
                    Console.WriteLine($"✅ {response.Message}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ {response?.Message ?? "Error desconocido al sincronizar ubicación."}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al sincronizar ubicación: {ex.Message}");
                return false;
            }
        }
    }
}
