using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContracts
{
    public class DolarResponse
    {
        public string Version { get; set; }
        public string Autor { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Unidad_medida { get; set; }
        public List<Serie> Serie { get; set; }
    }

    public class Serie
    {
        public DateTime Fecha { get; set; }
        public double Valor { get; set; }
    }
}
