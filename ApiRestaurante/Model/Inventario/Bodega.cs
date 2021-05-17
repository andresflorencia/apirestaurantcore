using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Inventario
{
    public class Bodega
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Sucursal { get; set; }
        public string Estado { get; set; }

        public Bodega() {
            this.Codigo = 0;
            this.Descripcion = "";
            this.Sucursal = 0;
            this.Estado = "";
        }
    }
}
