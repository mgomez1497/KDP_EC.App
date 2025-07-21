using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class VillagesRepositoryAPI
    {
        //private readonly ConsumoApi _api;

        //public VillagesRepositoryAPI()
        //{
        //    _api = new ConsumoApi("https://localhost:7149/api/Villages/");
        //}

        //public async Task<List<Villages>> ObtenerPueblos(int page = 1, int pageSize = 100)
        //{
        //    try
        //    {
        //        string endpoint = $"getVillages?page={page}&pageSize={pageSize}";
        //        var response = await _api.GetAsync<dynamic>(endpoint);

        //        if (response != null && response.data != null)
        //        {
        //            // Deserializar solo la propiedad "data" que contiene la lista
        //            var villagesJson = JsonConvert.SerializeObject(response.data);
        //            var villagesList = JsonConvert.DeserializeObject<List<Villages>>(villagesJson);
        //            return villagesList ?? new List<Villages>();
        //        }

        //        return new List<Villages>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"❌ Error al obtener los pueblos: {ex.Message}");
        //        return new List<Villages>();
        //    }
        //}
    }
}
