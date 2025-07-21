using KDP_EC.App.Infraestructure.API;
using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class RolRepository : SqLiteConnection
    {
        private readonly HttpClient _httpClient;
        public RolRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Rols>();
        }
        public async Task<List<Rols>> GetRoles()
        {
            var api = new ConsumoApi("https://localhost:7149/api/Rols/");
            try
            {
                var rolesResult = await api.GetAsync<List<Rols>>("getRols");
                return rolesResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener los roles: {ex.Message}");
                return new List<Rols>();
            }
        }
        public async Task SaveRolesLocally(List<Rols> roles)
        {
            await InitAsync();
            await _db.DeleteAllAsync<Rols>();
            await _db.InsertAllAsync(roles);
            await GetLocalRoles();
        }
        public async Task<List<Rols>> GetLocalRoles()
        {
            await InitAsync();
            return await _db.Table<Rols>().ToListAsync();
        }
    }
}
