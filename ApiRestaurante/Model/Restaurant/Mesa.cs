using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Restaurant
{
    public class Mesa
    {
        public string accion { get; set; }
        public int numero { get; set; }
        public Piso piso { get; set; }
        public string descripcion { get; set; }
        public int numSillas { get; set; }
        public bool disponible { get; set; }
        public string estado { get; set; }

        public int cant_pedido { get; set; }
        public double total_pedido { get; set; }
        public DateTime fecha_ingreso { get; set; }

        public Mesa() {
            this.accion = "";
            this.numero = 0;
            this.piso = new Piso();
            this.descripcion = "";
            this.numSillas = 0;
            this.disponible = false;
            this.estado = "";
            this.cant_pedido = 0;
            this.total_pedido = 0;
            this.fecha_ingreso = new DateTime();
        }
    }
}
