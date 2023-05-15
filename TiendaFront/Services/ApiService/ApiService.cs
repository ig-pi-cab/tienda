//using SharedLibrary.Dtos;

//namespace TiendaFront.Services.ApiService
//{
//    public class ApiService
//    {
//        private readonly HttpClient _httpClient;

//        public ApiService()
//        {
//            _httpClient = new HttpClient();
//            _httpClient.BaseAddress = new Uri("http://your-api-base-url/");
//        }

//        public async Task<List<ProductoDto>> GetAllProductsAsync()
//        {
//            var response = await _httpClient.GetAsync("api/products");
//            response.EnsureSuccessStatusCode();

//            var products = await response.Content.ReadAsAsync<List<ProductoDto>>();
//            return products;
//        }
//    }
//}

