using Microsoft.AspNetCore.Mvc;

namespace TiendaFront.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> List()
        {
            var products = await _productService.GetProducts();
            return View("Product", products);
        }
    }

}
