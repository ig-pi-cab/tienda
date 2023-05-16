//using TiendaFront.Models;

//namespace TiendaFront.Services.ShoppingCart
//{
//    public class ShoppingCartService
//    {
//        private List<CartItem> items;

//        public ShoppingCartService()
//        {
//            items = new List<CartItem>();
//        }

//        public void AddItem(Product product, int quantity)
//        {
//            var existingItem = items.FirstOrDefault(item => item.Product.id == product.id);

//            if (existingItem != null)
//            {
//                existingItem.Quantity += quantity;
//            }
//            else
//            {
//                var newItem = new CartItem { Product = product, Quantity = quantity };
//                items.Add(newItem);
//            }
//        }

//        public void RemoveItem(Product product)
//        {
//            var itemToRemove = items.FirstOrDefault(item => item.Product.id == product.id);
//            if (itemToRemove != null)
//            {
//                items.Remove(itemToRemove);
//            }
//        }

//        public void UpdateQuantity(Product product, int quantity)
//        {
//            var itemToUpdate = items.FirstOrDefault(item => item.Product.id == product.id);
//            if (itemToUpdate != null)
//            {
//                itemToUpdate.Quantity = quantity;
//            }
//        }

//        public void ClearCart()
//        {
//            items.Clear();
//        }

//        public List<CartItem> GetItems()
//        {
//            return items;
//        }

//        public double GetTotal()
//        {
//            double total = 0;
//            foreach (var item in items)
//            {
//                total += item.Product.precio * item.Quantity;
//            }
//            return total;
//        }
//    }

//    public class CartItem
//    {
//        public Product Product { get; set; }
//        public int Quantity { get; set; }
//    }
//}
