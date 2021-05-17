using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.General
{
    public class Sucursal
    {
        public int Codigo {get; set;}
        public string Descripcion {get; set;}
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public Sucursal() {
            this.Codigo = 0;
            this.Descripcion = "";
            this.Estado = "";
            this.Direccion = "";
            this.Telefono = "";
        }
    }
}
