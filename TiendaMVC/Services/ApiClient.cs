using Core.Entities;
using System.Net.Http.Formatting;

namespace TiendaMVC.Services
{
    public class ApiClient
    {
       
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Producto>> GetMyModelsAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:5001/api/productos");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<IEnumerable<Producto>>();
        }
    }
}
