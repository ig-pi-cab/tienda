using MusicProMvcWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProMvcWebApp.Models
{
    public class MarcaData: BaseEntityData
    {
        public string Nombre { get; set; }
        public ICollection<ProductoData> Productos { get; set; }
    }
}
