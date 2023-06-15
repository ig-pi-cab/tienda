namespace TiendaFront.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; }

        public Cart()
        {
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        public int GetCartItemCount()
        {
            return Products.Count;
        }
        public decimal GetTotalPrice()
        {
            return (decimal)Products.Sum(p => p.precio);
        }

    }
}
