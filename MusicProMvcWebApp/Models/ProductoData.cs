using System.Text.RegularExpressions;
using MusicProMvcWebApp.Models;

namespace MusicProMvcWebApp.Models
{
    public class ProductoData:BaseEntityData

    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int MarcaId { get; set; }
        public MarcaData Marca { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaData Categoria { get; set; }
    }
}
