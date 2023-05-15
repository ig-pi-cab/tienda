namespace TiendaFront.Models
{

    public class Product
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }
        public DateTime fechaCreacion { get; set; }
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
    }
    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class Marca
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    

}
