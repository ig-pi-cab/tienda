using Microsoft.AspNetCore.Mvc;
using TiendaFront.Models;
using System.Linq;

namespace TiendaFront.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly Cart _cart;

        public ProductController(ProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }

        public IActionResult List()
        {
            var products = _productService.GetProducts();
            return View("Product", products);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            // Retrieve the product from the ProductService using the id
            var product = await _productService.GetProductById(id);

            // Add the product to the cart
            _cart.AddProduct(product);

            // Pass the updated cart item count to the view
            var cartItemCount = _cart.GetCartItemCount();

            return Json(new { cartItemCount });
        }


        public IActionResult Cart()
        {
            return View(_cart);
        }
    }
}
