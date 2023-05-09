using Microsoft.AspNetCore.Mvc;
using MusicProMvcWebApp.Helper;
using MusicProMvcWebApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MusicProMvcWebApp.Controllers
{

    
    public class HomeController : Controller
    {

        ProductoApi _api = new ProductoApi();


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductoData> productos = new List<ProductoData>();
            HttpClient producto = _api.Initial();
            HttpResponseMessage res = await producto.GetAsync("api/productos");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                productos = JsonConvert.DeserializeObject<List<ProductoData>>(results);
            }
            return View(productos);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}