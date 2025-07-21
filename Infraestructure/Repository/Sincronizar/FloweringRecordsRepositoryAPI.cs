using KDP_EC.App.Infraestructure.API;
using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class FloweringRecordsRepositoryAPI
    {
        private readonly ConsumoApi _api;

        public FloweringRecordsRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/FloweringRecords/");
        }

        public async Task<List<FloweringRecords>> ObtenerFloracionesporUserId(Guid UserId)
        {
            try
            {
                var floweringResults = await _api.GetAsync<List<FloweringRecords>>($"GetFloweringRecordsByUserId?UserId={UserId}");
                return floweringResults;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los lotes: {ex.Message}");
                return new List<FloweringRecords>();
            }
        }
        public async Task<bool> sincronizarFloraciones(List<FloweringRecords> floweringRecords)
        {
            try
            {

                var response = await _api.PostAsync<List<FloweringRecords>, HttpResponseMessage>("createFloweringRecord", floweringRecords);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al sincronizar las floraciones: {ex.Message}");
                return false;
            }
        }

        public async Task<HttpResponseMessage> SincronizarConRespuesta(List<FloweringRecords> floweringRecords)
        {
            try
            {
                return await _api.PostAsync<List<FloweringRecords>, HttpResponseMessage>(
                    "createFloweringRecord", floweringRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción al enviar floraciones: {ex.Message}");
                throw;
            }
        }
    }
}
