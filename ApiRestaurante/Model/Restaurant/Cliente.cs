using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Restaurant
{
    public class Cliente
    {
        public int codigo { get; set; }
        public String cedRuc { get; set; }
        public String nombre { get; set; }
        public String direccion { get; set; }
        public String correo { get; set; }
        public String telefono { get; set; }

        public Cliente() {
            this.codigo = 0;
            this.cedRuc = "";
            this.nombre = "";
            this.direccion = "";
            this.correo = "";
            this.telefono = "";
        }
    }
}
