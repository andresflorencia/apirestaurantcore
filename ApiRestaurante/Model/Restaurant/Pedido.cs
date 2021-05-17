using ApiRestaurante.Model.Inventario;
using ApiRestaurante.Model.Printer;
using ApiRestaurante.Model.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Restaurant
{
    public class Pedido
    {
        public string accion { get; set; }
        public int codigo { get; set; }
        public int numero { get; set; }
        public Mesa mesa { get; set; }
        public int cliente { get; set; }
        public String nombreClientePedido { get; set; }
        public Double total { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaEntrega { get; set; }
        public Usuario   usuario { get; set; }
        public List<Detalle_Pedido> listaDetalle { get; set; }
        public Impresora Impresora { get; set; }
        public SubLinea SubLinea { get; set; }
        public String cedRuc { get; set; }
        public String direccion { get; set; }
        public String correo { get; set; }
        public String telefono { get; set; }
        public bool isLlevar { get; set; }

        public Pedido()
        {
            accion = "";
            codigo = 0;
            numero = 0;
            mesa = new Mesa();
            cliente = 0;
            nombreClientePedido = "";
            total = 0;
            observacion = "";
            estado = "";
            fecha = DateTime.Now;
            fechaEntrega = DateTime.Now;
            usuario = new Usuario();
            listaDetalle = new List<Detalle_Pedido>();
            Impresora = new Impresora();
            SubLinea = new SubLinea();
            cedRuc = "";
            direccion = "";
            correo = "";
            telefono = "";
            isLlevar = false;
        }
    }
}
