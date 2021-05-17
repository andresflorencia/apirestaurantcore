using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Restaurant
{
    public class Piso
    {
        public string accion { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }

        public Piso() {
            this.accion = "";
            this.codigo = 0;
            this.descripcion = "";
            this.estado = "";
        }
    }
}
