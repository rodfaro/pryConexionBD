using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pryGestionInventario
{
    internal class clsProductos
    {
        public int Codigo{ get; set; }
        public string Nombre { get; set; }
        public string Desc { get; set; }

        public decimal Precio{ get; set; }
        public int Stock{ get; set; }
        public int CategoriaId { get; set; }
    }
}
