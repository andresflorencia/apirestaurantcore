using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Drawing;

namespace ApiRestaurante.Model.Printer
{
    public class ImpresoraFormato
    {
        public string Impresora;
        public int NumColumn;
        public int NumCopias;
        public List<string> ListImpresion;
        private PrinterSettings prtSettings = new PrinterSettings();
        private PrintDocument prtDoc  = new PrintDocument();

        public ImpresoraFormato(string Impresora, int NumColumn, int NumCopias)
        {
            this.Impresora = Impresora;
            this.NumColumn = NumColumn;
            this.NumCopias = NumCopias;
            this.ListImpresion = new List<string>();
        }

        public void ConfiguraImpresora()
        {
            if (prtSettings == null)
            {
                prtSettings = new PrinterSettings();
            }
            prtSettings.PrinterName = this.Impresora;
            prtSettings.Copies = short.Parse(this.NumCopias.ToString());
            //prtDoc.PrintPage += new PrintPageEventHandler(this.print_PrintPage);
            prtDoc.PrinterSettings = prtSettings;

        }

        private void print_PrintPage(object sender, PrintPageEventArgs e) {

            Single xPos = 1;

            Font prFont = new Font("Arial", 14, FontStyle.Bold);
            Font prFontDetalle = new Font("Arial", 10, FontStyle.Regular);

            Single yPos = prFont.GetHeight(e.Graphics);


            for (int i = 0; i < ListImpresion.Count; i++) {
                if (i == 0) {
                    e.Graphics.DrawString(ListImpresion[i], prFont, Brushes.Black, xPos, yPos);
                    yPos += prFont.GetHeight(e.Graphics);
                }
                else {
                    e.Graphics.DrawString(ListImpresion[i], prFontDetalle, Brushes.Black, xPos, yPos);
                    yPos += prFontDetalle.GetHeight(e.Graphics);
                }
            }
            e.HasMorePages = false;
        }

        public void Imprimir()
        {
            try
            {
                if (!prtDoc.PrinterSettings.IsValid)
                {
                    throw new Exception("Impresora no válida: " + prtDoc.PrinterSettings.PrinterName);
                }
                else
                {
                    prtDoc.PrintPage += new PrintPageEventHandler(print_PrintPage);
                    prtDoc.Print();
                }
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
