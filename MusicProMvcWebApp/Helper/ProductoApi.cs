namespace MusicProMvcWebApp.Helper
{
    public class ProductoApi
    {
        public HttpClient Initial()
        {
            var product = new HttpClient();
            product.BaseAddress = new Uri("https://localhost:5001");
            return product;
        }
    }
}
