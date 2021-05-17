using ApiRestaurante.Model.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Restaurant
{
    public class UsuarioPiso
    {
        public Usuario Usuario { get; set; }
        public Piso Piso { get; set; }
        public string Accion { get; set; }

        public UsuarioPiso() {
            this.Usuario = new Usuario();
            this.Piso = new Piso();
            this.Accion = "";
        }
    }
}
