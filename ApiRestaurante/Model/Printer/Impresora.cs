using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Model.Printer
{
    public class Impresora
    {
        //Private myConnectionString As String = ""
    //Private objDatosConfig As DatosConfig = New DatosConfig()

        public string impresora { get; set; }
        public int NumCopias { get; set; }
        public int NumColumImpresora { get; set; }
        public Impresora()
        {
            this.impresora = "";
            this.NumCopias = 0;
            this.NumColumImpresora = 0;
        }
    }
}
