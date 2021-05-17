using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Inventario
{
    public class Linea
    {
        public int Codigo{ get; set; }
        public string Descripcion{ get; set; }
        public double Comision{ get; set; }
        public string Ruc{ get; set; }
        public string Nombre{ get; set; }
        public int CodPtoVta{ get; set; }
        public string RutaFirma{ get; set; }
        public string DireccionEmpresa{ get; set; }
        public string DireccionSucursal{ get; set; }
        public string NumResoContEspecial{ get; set; }
        public bool ObligadoContabilidad{ get; set; }
        public string NombreComercialEmpresa{ get; set; }
        public string ClaveFirma{ get; set; }

        public Linea() {
            this.Codigo = 0;
            this.Descripcion = "";
            this.Comision = 0;
            this.Ruc = "";
            this.Nombre = "";
            this.CodPtoVta = 0;
            this.RutaFirma = "";
            this.DireccionEmpresa = "";
            this.DireccionSucursal = "";
            this.NumResoContEspecial = "";
            this.ObligadoContabilidad = false;
            this.NombreComercialEmpresa = "";
            this.ClaveFirma = "";
         }
    }
}
