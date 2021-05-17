using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.General
{
    public class PtoVta
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Sucursal { get; set; }

        public PtoVta() {
            this.Codigo = 0;
            this.Descripcion = "";
            this.Sucursal = 0;
        }
    }
}
