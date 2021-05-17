using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Inventario
{
    public class SubLinea
    {
        public int Codigo { get; set; }
    public string Descripcion { get; set; }
        public int Linea { get; set; }
        public string Foto { get; set; }
        public string Accion { get; set; }
        public int RetornoTransaccion { get; set; }
        public string DescripTransaccion { get; set; }

        public SubLinea() {
            this.Codigo = 0;
            this.Descripcion = "";
            this.Linea = 0;
            this.Foto = "";
            this.Accion = "";
            this.RetornoTransaccion = 0;
            this.DescripTransaccion = "";
        }
    }
}
