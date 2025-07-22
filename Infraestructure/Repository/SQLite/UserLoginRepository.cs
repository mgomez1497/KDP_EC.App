using KDP_EC.App.Infraestructure.API;
using KDP_EC.App.Infraestructure.Database;
using KDP_EC.Core.Models;
using KDP_EC.Core.ModelView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Infraestructure.Repository.SQLite
{
    public class UserLoginRepository : SqLiteConnection
    {

        private readonly HttpClient _httpClient;
        public UserLoginRepository()
        {
            _httpClient = new HttpClient();
        }


        public async Task InitializeAsync()
        {
            await base.InitAsync();
            await _db.CreateTableAsync<Users>();
        }

        

        public async Task<LoginResponse> LoginUsuario(string username, string password)
        {
            var api = new ConsumoApi("http://sandboxapiec.localpartners.ch/api/Account/");

            var loginData = new Users
            {
                UserName = username,
                Password = password
            };

            try
            {
                var loginResult = await api.PostAsync<Users, LoginResponse>("login", loginData);
                return loginResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al hacer login: {ex.Message}");
                return null;
            }
        }

        public async Task<List<UserInfoViewModel?>> GetUserInfoAsync(string token, string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                throw new HttpRequestException("Error al obtener la informacion del usuario");

            }

            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var apiUrl = $"http://sandboxapiec.localpartners.ch/api/Account/userinfo?UserId={userId}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Error al obtener la informacion del usuario");

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<UserInfoViewModel>>(responseString);

            return result ?? new List<UserInfoViewModel>();
        }

        public async Task LogoutAsync()
        {
            
            if (SecureStorage.Default != null)
            {
                SecureStorage.Default.Remove("access_token");
                SecureStorage.Default.Remove("user_id");
            }

            await InitializeAsync();

            
            await _db.DeleteAllAsync<UserInfoViewModel>();
            await _db.DeleteAllAsync<Lots>();
            await _db.DeleteAllAsync<Farms>();
            await _db.DeleteAllAsync<Villages>();
            await _db.DeleteAllAsync<Chains>();
            await _db.DeleteAllAsync<Countries>();
            await _db.DeleteAllAsync<Company>();
            await _db.DeleteAllAsync<LotsRepository>();
            await _db.DeleteAllAsync<Lots_Varietys>();
            
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            Console.WriteLine("Sesión cerrada correctamente.");
        }

    }
}
