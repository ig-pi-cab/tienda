using Microsoft.AspNetCore.Mvc;
using TiendaFront.Models;
using TiendaFront.Services.ShoppingCart;

namespace TiendaFront.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ProductService _productService;
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ProductService productService, ShoppingCartService shoppingCartService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var cartItems = _shoppingCartService.GetItems();
            var total = _shoppingCartService.GetTotal();
            // Pass the cart items and total to the view
            return View(new ShoppingCartViewModel { CartItems = cartItems, Total = total });
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _shoppingCartService.AddItem(product, quantity);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _shoppingCartService.RemoveItem(product);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _shoppingCartService.UpdateQuantity(product, quantity);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _shoppingCartService.ClearCart();
            return RedirectToAction("Index");
        }
    }

    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public double Total { get; set; }
    }
}
