using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Inventario
{
    public class ItemUnidad
    {
        public string Accion{ get; set; }
        public int CodItem{ get; set; }
        public double Precio{ get; set; }
        public double PrecioLista{ get; set; }
        public double Costo{ get; set; }
        public double Stock{ get; set; }
        public bool IVA{ get; set; }
        public string ItemNombre{ get; set; }
        public int Unidad{ get; set; }
        public string UnidadDescripcion{ get; set; }
        public string UnidadSigla{ get; set; }
        public double Convertibilidad{ get; set; }
        public string CodBar{ get; set; }
        public string Tipo{ get; set; }
        public string EstadoBodega{ get; set; }
        public double Peso{ get; set; }
        public double MaxDescuento{ get; set; }
        public double CantConvertivilidad{ get; set; }
        public int CodItemSurtido{ get; set; }
        public int ReglaNum{ get; set; }
        public DateTime UltFechaCompra{ get; set; }
        public double CostoUltCompraIva{ get; set; }
        public double CostoUltCompra{ get; set; }
        public bool CantEntera{ get; set; }
        public double Convertibilidad1{ get; set; }
        public double PrecioListaConfig{ get; set; }
        public int MaxReglaPrecio{ get; set; }

        public ItemUnidad() {
            Accion = "";
            CodItem = 0;
            ItemNombre = "";
            Unidad = 0;
            UnidadDescripcion = "";
            Convertibilidad = 0;
            Precio = 0;
            Costo = 0;
            IVA = false;
            CodBar = "";
            Tipo = "";
            EstadoBodega = "";
            MaxDescuento = 0;
            CantConvertivilidad = 0;
            CodItemSurtido = 0;
            ReglaNum = 0;
            UltFechaCompra = DateTime.Now;
            CostoUltCompraIva = 0;
            CostoUltCompra = 0;
            CantEntera = false;
            Convertibilidad1 = 0;
            PrecioListaConfig = 0;
            MaxReglaPrecio = 0;
        }
    }
}
