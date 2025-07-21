using KDP_EC.App.Infraestructure.API;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.Sincronizar
{
    public class ExpensesRepositoryAPI
    {
        private readonly ConsumoApi _api;
        public ExpensesRepositoryAPI()
        {
            _api = new ConsumoApi("https://localhost:7149/api/Expenses/");
        }
        public async Task<List<Expenses>> ObtenerGastosporFincaId(Guid FarmId)
        {
            try
            {
                var expensesResults = await _api.GetAsync<List<Expenses>>($"getExpensesByFarmId?FarmId={FarmId}");
                return expensesResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los gastos: {ex.Message}");
                return new List<Expenses>();
            }
        }

        public async Task<bool> PostExpensesAsync(List<Expenses> expenses)
        {
            try
            {
                var response = await _api.PostAsync<List<Expenses>, HttpResponseMessage>("postExpenses", expenses);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Gastos sincronizados correctamente.");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error en la sincronización de gastos: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar los gastos: {ex.Message}");
                return false;
            }
        }

    }
}
