using ApiRestaurante.Model.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Restaurant
{
    public class Detalle_Pedido
    {
        public string accion { get; set; }
        public int pedido { get; set; }
        public Double cantidad { get; set; }
        public Double precio { get; set; }
        public string estado { get; set; }
        public ItemUnidad itemUnidad { get; set; }
        public int idFac { get; set; }
        public string observacion { get; set; }

        public Detalle_Pedido()
        {
            this.accion = "";
            this.pedido = 0;
            this.cantidad = 0;
            this.precio = 0;
            this.estado = "";
            this.itemUnidad = new ItemUnidad();
            this.idFac = 0;
            this.observacion = "";
        }
    }
}
