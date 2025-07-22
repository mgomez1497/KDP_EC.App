using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class IncomesRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public IncomesRepositoryAPI()
        {
            _api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/Incomes/");
        }
        public async Task<List<Incomes>> ObtenerIngresos()
        {
            try
            {
                var incomeResults = await _api.GetAsync<List<Incomes>>("getIncomes");
                return incomeResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los ingresos: {ex.Message}");
                return new List<Incomes>();
            }
        }

        public async Task<List<Incomes>> ObtenerIngresosporFincaId(Guid FarmId)
        {
            try
            {
                var incomeResults = await _api.GetAsync<List<Incomes>>($"getIncomesByFarmId?FarmId={FarmId}");
                return incomeResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los ingresos por finca: {ex.Message}");
                return new List<Incomes>();
            }
        }

        public async Task<bool> PostIncomesAsync(List<Incomes> incomes)
        {
            try
            {
                var response = await _api.PostAsync<List<Incomes>, HttpResponseMessage>("CreateIncomes", incomes);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Ingresos sincronizados correctamente.");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error en la sincronización de ingresos: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar los ingresos: {ex.Message}");
                return false;
            }
        }
    }
}
