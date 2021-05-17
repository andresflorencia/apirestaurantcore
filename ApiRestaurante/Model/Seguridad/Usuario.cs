using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Seguridad
{
    public class Usuario
    {
        public int codigo { get; set; }
        public int tipoUsuario { get; set; }
        public string accion { get; set; }
        public string login { get; set; }
        public string clave { get; set; }

        public Usuario() {
            this.codigo = 0;
            this.accion = "";
            this.login = "";
            this.clave = "";
        }
    }
}
