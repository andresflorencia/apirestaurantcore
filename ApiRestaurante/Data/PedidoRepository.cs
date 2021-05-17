using ApiRestaurante.Model.Printer;
using ApiRestaurante.Model.Restaurant;
using ApiRestaurante.Model.Seguridad;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PruebaApiNetCore.Data;
using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Drawing;

namespace ApiRestaurante.Data
{
    public class PedidoRepository
    {
        private readonly String _ConnectionString;
        private DetPedidoRepository _reposiDetalle;
        private UsuarioRepository _reposiUsuario;
        private readonly IHostingEnvironment _hostingEnvironment;
        private ImpresoraRepository _reposiImpresora;
        private Impresora impresoraAnulacion;
        private PisoRepository _reposiPiso;
        public PedidoRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
            _reposiDetalle = new DetPedidoRepository(configuration);
            _reposiUsuario = new UsuarioRepository(configuration);
            _reposiImpresora = new ImpresoraRepository(configuration);
            _reposiPiso = new PisoRepository(configuration);
            _hostingEnvironment = hostingEnvironment;
            impresoraAnulacion = new Impresora();
        }
        public async Task<Pedido> BuscarDatos(string Accion, int CodPedido )
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Pedido]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", Accion));
                    cmd.Parameters.Add(new SqlParameter("eCodigo", CodPedido));
                    var response = new Pedido();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            response.numero = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            response.mesa.piso.codigo = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            response.mesa.numero = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            response.cliente = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            response.nombreClientePedido = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            response.total = reader.IsDBNull(6) ? 0 : Double.Parse(reader.GetDecimal(6).ToString());
                            response.observacion = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            response.estado = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            response.fecha = reader.IsDBNull(9) ? new DateTime() : reader.GetDateTime(9);
                            response.fechaEntrega = reader.IsDBNull(10) ? new DateTime() : reader.GetDateTime(10);
                            response.usuario.codigo = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                            response.cedRuc = reader.IsDBNull(12) ? "" : reader.GetString(12);
                            response.direccion = reader.IsDBNull(13) ? "" : reader.GetString(13);
                            response.correo = reader.IsDBNull(14) ? "" : reader.GetString(14);
                            response.telefono = reader.IsDBNull(15) ? "" : reader.GetString(15);
                            response.isLlevar = reader.IsDBNull(16) ? false : reader.GetBoolean(16);
                            response.listaDetalle = await _reposiDetalle.GetLista("LISTA_DETALLE", response.codigo);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<List<Pedido>> GetLista(string Accion , int Piso , int Mesa, bool isLlevar)
        { 
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Pedido]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", Accion));
                    cmd.Parameters.Add(new SqlParameter("@ePiso", Piso));
                    cmd.Parameters.Add(new SqlParameter("@eMesa", Mesa));
                    cmd.Parameters.Add(new SqlParameter("@bLlevar", isLlevar));
                    var response = new List<Pedido>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miDetalle = new Pedido();
                            miDetalle.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miDetalle.numero = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            miDetalle.mesa.piso.codigo = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            miDetalle.mesa.numero = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            miDetalle.cliente = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            miDetalle.nombreClientePedido = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            miDetalle.total = reader.IsDBNull(6) ? 0 : Double.Parse(reader.GetDecimal(6).ToString());
                            miDetalle.observacion = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            miDetalle.estado = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            miDetalle.fecha = reader.IsDBNull(9) ? new DateTime() : reader.GetDateTime(9);
                            miDetalle.fechaEntrega = reader.IsDBNull(10) ? new DateTime() : reader.GetDateTime(10);
                            miDetalle.usuario.codigo = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                            miDetalle.cedRuc = reader.IsDBNull(12) ? "" : reader.GetString(12);
                            miDetalle.direccion = reader.IsDBNull(13) ? "" : reader.GetString(13);
                            miDetalle.correo = reader.IsDBNull(14) ? "" : reader.GetString(14);
                            miDetalle.telefono = reader.IsDBNull(15) ? "" : reader.GetString(15);
                            miDetalle.isLlevar = reader.IsDBNull(16) ? false : reader.GetBoolean(16);
                            miDetalle.listaDetalle = await _reposiDetalle.GetLista("LISTA_DETALLE", miDetalle.codigo);
                            response.Add(miDetalle);
                        }
                        return response;
                    }
                }
            }
        }
        public async Task<List<Pedido>> GetListaImp(int CodPedido, string Estado, string CodigosItem, bool isAnulacion)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Pedido]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "LISTA_IMPRESION"));
                    cmd.Parameters.Add(new SqlParameter("@eCodigo", CodPedido));
                    cmd.Parameters.Add(new SqlParameter("@aEstado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@aCodigos", CodigosItem));
                    cmd.Parameters.Add(new SqlParameter("@bAnulacion", isAnulacion));
                    var response = new List<Pedido>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miDetalle = new Pedido();
                            miDetalle.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miDetalle.numero = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            miDetalle.mesa.piso.codigo = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            miDetalle.mesa.numero = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            miDetalle.cliente = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            miDetalle.nombreClientePedido = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            miDetalle.total = reader.IsDBNull(6) ? 0 : Double.Parse(reader.GetDecimal(6).ToString());
                            miDetalle.observacion = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            miDetalle.estado = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            miDetalle.fecha = reader.IsDBNull(9) ? new DateTime() : reader.GetDateTime(9);
                            miDetalle.fechaEntrega = reader.IsDBNull(10) ? new DateTime() : reader.GetDateTime(10);
                            miDetalle.usuario.codigo = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                            miDetalle.SubLinea.Codigo = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);

                            miDetalle.SubLinea.Descripcion = reader.IsDBNull(13) ? "" : reader.GetString(13);
                            miDetalle.Impresora.impresora = reader.IsDBNull(14) ? "" : reader.GetString(14);
                            miDetalle.Impresora.NumCopias = reader.IsDBNull(15) ? 1 : reader.GetInt32(15);
                            miDetalle.Impresora.NumColumImpresora = reader.IsDBNull(16) ? 40 : reader.GetInt32(16);

                            miDetalle.cedRuc = reader.IsDBNull(17) ? "" : reader.GetString(17);
                            miDetalle.direccion = reader.IsDBNull(18) ? "" : reader.GetString(18);
                            miDetalle.correo = reader.IsDBNull(19) ? "" : reader.GetString(19);
                            miDetalle.telefono = reader.IsDBNull(20) ? "" : reader.GetString(20);
                            miDetalle.isLlevar = reader.IsDBNull(21) ? false : reader.GetBoolean(21);

                            miDetalle.listaDetalle = await _reposiDetalle.GetListaImp("LISTA_IMPRESION", miDetalle.codigo, miDetalle.SubLinea.Codigo, Estado, CodigosItem, miDetalle.Impresora.impresora);
                            response.Add(miDetalle);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<ResultAPI> GrabaDatos(List<Pedido> ListaPedidos, int ePtoVta, bool ImprimirTicket = false,int TipoDoc= 1, bool NuevoFormatoImp=false)
        {
            CultureInfo c = new CultureInfo("en-US");
            var retorno = false;
            var result = new ResultAPI();
            int codMov = 0;
            List<int> ListImprimir = new List<int>();
            List<int> ListAnulado = new List<int>();
            List<string> ListCodItem = new List<string>();
            List<string> ListCodItemAnul = new List<string>();

            impresoraAnulacion = await _reposiImpresora.BuscarDatos(ePtoVta, 11);

            using (SqlConnection cnn = new SqlConnection(_ConnectionString))
            {
                await cnn.OpenAsync();

                SqlTransaction tran = cnn.BeginTransaction();

                try
                {
                    foreach (Pedido miPedido in ListaPedidos)
                    {
                        codMov = 0;
                        retorno = false;
                        using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Grab_Rest_Pedido]", cnn, tran))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.AddWithValue("@aTipoAccion", miPedido.accion);
                            cmd.Parameters.AddWithValue("@eCodigo", miPedido.codigo);
                            cmd.Parameters.AddWithValue("@eNumero", miPedido.numero);
                            cmd.Parameters.AddWithValue("@ePiso", miPedido.mesa.piso.codigo);
                            cmd.Parameters.AddWithValue("@eMesa", miPedido.mesa.numero);
                            cmd.Parameters.AddWithValue("@eCliente", miPedido.cliente);
                            cmd.Parameters.AddWithValue("@aNombreClientePedido", miPedido.nombreClientePedido);
                            cmd.Parameters.AddWithValue("@dTotal", miPedido.total.ToString(c));
                            cmd.Parameters.AddWithValue("@aObservacion", miPedido.observacion);
                            cmd.Parameters.AddWithValue("@aEstado", miPedido.estado);
                            cmd.Parameters.AddWithValue("@fFecha", miPedido.fecha);
                            cmd.Parameters.AddWithValue("@fFechaEntrega", miPedido.fecha);
                            cmd.Parameters.AddWithValue("@eUsuario", miPedido.usuario.codigo);
                            cmd.Parameters.AddWithValue("@aCedRuc", miPedido.cedRuc);
                            cmd.Parameters.AddWithValue("@aDireccion", miPedido.direccion);
                            cmd.Parameters.AddWithValue("@aCorreo", miPedido.correo);
                            cmd.Parameters.AddWithValue("@aTelefono", miPedido.telefono);
                            cmd.Parameters.AddWithValue("@bLlevar", miPedido.isLlevar);

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader.HasRows && await reader.ReadAsync())
                                {
                                    codMov = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                    if (miPedido.accion.Equals("Nuevo Registro"))
                                        miPedido.codigo = codMov;
                                    retorno = true;
                                }
                            }

                            string Codigos = "";
                            string CodigosAnul = "";
                            if (miPedido.listaDetalle.Count > 0 && retorno && miPedido.codigo > 0) {
                                foreach (Detalle_Pedido Linea in miPedido.listaDetalle) {
                                    if (miPedido.accion.Equals("Nuevo Registro")) {
                                        Linea.pedido = miPedido.codigo;
                                        if (Codigos.Length == 0)
                                            Codigos = Linea.itemUnidad.CodItem.ToString();
                                        else
                                            Codigos += "," + Linea.itemUnidad.CodItem.ToString();
                                    } else if (miPedido.accion.Equals("Modificar") && Linea.estado == "A") {
                                        if (CodigosAnul.Length == 0)
                                            CodigosAnul = Linea.itemUnidad.CodItem.ToString();
                                        else
                                            CodigosAnul += "," + Linea.itemUnidad.CodItem.ToString();
                                            
                                    }               
                                }
                                retorno = await _reposiDetalle.GrabaDatos(miPedido.listaDetalle, cnn, tran);
                            }

                            if (retorno && ImprimirTicket)
                            {
                                if (miPedido.accion.Equals("Nuevo Registro"))
                                {
                                    if (Codigos.Length > 0)
                                    {
                                        ListCodItem.Add(Codigos);
                                        ListImprimir.Add(miPedido.codigo);
                                    }
                                }
                                else if (miPedido.accion.Equals("Modificar"))
                                {
                                    if (CodigosAnul.Length > 0)
                                    {
                                        ListCodItemAnul.Add(CodigosAnul);
                                        ListAnulado.Add(miPedido.codigo);
                                    }
                                }
                            }

                            if (!retorno)
                            {
                                tran.Rollback();
                                result.estado = retorno;
                                result.message_error = "ERROR";
                                break;
                            }
                        }
                    }
                    if (retorno)
                    {
                        tran.Commit();
                        result.estado = retorno;
                        result.message_error = "OK";
                    }
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    result.message_error = e.Message;
                    result.estado = false;
                }

            }

            if (ListImprimir.Count > 0 && ListCodItem.Count > 0) {
                for (int iRow = 0; iRow < ListImprimir.Count; iRow++)
                {
                    if (!NuevoFormatoImp)
                        ImprimirTicketFormato(ListImprimir[iRow], true, "V", ListCodItem[iRow]);
                    else
                        ImprimirTicketFormato2(ListImprimir[iRow], true, "V", ListCodItem[iRow]);
                }
            }

            if (ListAnulado.Count > 0 && ListCodItemAnul.Count > 0) {                
                for (int iRow = 0; iRow < ListAnulado.Count; iRow++)
                {
                    if (!NuevoFormatoImp)
                    {
                        ImprimirTicketFormato(ListAnulado[iRow], true, "A", ListCodItemAnul[iRow]);
                        ImprimirTicketFormato(ListAnulado[iRow], true, "A", ListCodItemAnul[iRow], impresoraAnulacion.impresora, impresoraAnulacion.NumColumImpresora, impresoraAnulacion.NumCopias);
                    }
                    else {
                        ImprimirTicketFormato2(ListAnulado[iRow], true, "A", ListCodItemAnul[iRow]);
                        ImprimirTicketFormato2(ListAnulado[iRow], true, "A", ListCodItemAnul[iRow], impresoraAnulacion.impresora, impresoraAnulacion.NumColumImpresora, impresoraAnulacion.NumCopias);
                    }
                }
            }

            return result;
        }

        private async void ImprimirTicketFormato(int CodPedido, bool ImpCortePapel, string Estado, string CodigosItem, string namePrinter="", int numCol=40, int numCopy=1)
        {
            var ListPedido = new List<Pedido>(await GetListaImp(CodPedido, Estado, CodigosItem, namePrinter.Length == 0 ? false : true));
            
            try
            {
                foreach (Pedido miPedido in ListPedido)
                {
                    var ListaImpresion = new List<string>();
                    var miImpre = new Impresora();

                    if (Estado.Equals("A") && namePrinter.Length > 0)
                    {
                        miImpre.impresora = namePrinter;
                        miImpre.NumColumImpresora = numCol;
                        miImpre.NumCopias = numCopy;
                    }
                    else {
                        miImpre.impresora = miPedido.Impresora.impresora;
                        miImpre.NumColumImpresora = miPedido.Impresora.NumColumImpresora;
                        miImpre.NumCopias = miPedido.Impresora.NumCopias;
                    }

                    var impresora = new dllimpresion.ImpresoraFormato(miImpre.impresora, miImpre.NumColumImpresora, miImpre.NumCopias);

                    impresora.ConfiguraImpresora();
                    miPedido.usuario = await _reposiUsuario.GetUser(miPedido.usuario.codigo);
                    //ListaImpresion.Add(Estado.Equals("A")? "ANULACION ": "ORDEN PARA " + miPedido.SubLinea.Descripcion);
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(Estado.Equals("A") ? "ANULACION" : "ORDEN DE PEDIDO", 14, FontStyle.Bold));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("FECHA : " + miPedido.fecha.ToShortDateString() + " HORA: " + miPedido.fecha.ToShortTimeString(), 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("PEDIDO : " + miPedido.numero + "   " + (miPedido.isLlevar ? "PARA LLEVAR" : "MESA : " + miPedido.mesa.numero), 10, FontStyle.Bold));
                    if (miPedido.nombreClientePedido.Length > 0) impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("CLIENTE : " + miPedido.nombreClientePedido, 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("VENDEDOR: " + miPedido.usuario.login, 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(" ",10,FontStyle.Regular));
                    //ListaImpresion.Add("====================")
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("CANT.     DETALLE", 10, FontStyle.Regular));
                    //ListaImpresion.Add("CANT.       DETALLE");
                    foreach (Detalle_Pedido miLinea in miPedido.listaDetalle) {
                        //ListaImpresion.Add(miLinea.cantidad.ToString() + "         " + miLinea.itemUnidad.ItemNombre);
                        String linea = miLinea.cantidad.ToString() + "      " + miLinea.itemUnidad.ItemNombre;
                        String linea2 = linea.Length > 31 ? linea.Substring(31, linea.Length - 31) : "";
                        linea = linea.Length > 31 ? linea.Substring(0, 31) + "-" : linea;
                        impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(linea, 10, FontStyle.Regular));
                        if (linea2.Length > 0) impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(linea2, 10, FontStyle.Regular));
                        if (miLinea.observacion.Length > 0 && Estado == "V") impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("Obs: " + miLinea.observacion, 10, FontStyle.Regular));
                    }
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    //ListaImpresion.Add("");
                    impresora.Imprimir();

                    if (ImpCortePapel)
                        ImprimirCortePapel(miImpre.impresora, miImpre.NumColumImpresora);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void ImprimirTicketFormato2(int CodPedido, bool ImpCortePapel, string Estado, string CodigosItem, string namePrinter = "", int numCol = 40, int numCopy = 1)
        {
            var ListPedido = new List<Pedido>(await GetListaImp(CodPedido, Estado, CodigosItem, namePrinter.Length == 0 ? false : true));

            try
            {
                foreach (Pedido miPedido in ListPedido)
                {
                    var ListaImpresion = new List<string>();
                    var miImpre = new Impresora();

                    if (Estado.Equals("A") && namePrinter.Length > 0)
                    {
                        miImpre.impresora = namePrinter;
                        miImpre.NumColumImpresora = numCol;
                        miImpre.NumCopias = numCopy;
                    }
                    else
                    {
                        miImpre.impresora = miPedido.Impresora.impresora;
                        miImpre.NumColumImpresora = miPedido.Impresora.NumColumImpresora;
                        miImpre.NumCopias = miPedido.Impresora.NumCopias;
                    }

                    var impresora = new dllimpresion.ImpresoraFormato(miImpre.impresora, miImpre.NumColumImpresora, miImpre.NumCopias);

                    impresora.ConfiguraImpresora();
                    miPedido.usuario = await _reposiUsuario.GetUser(miPedido.usuario.codigo);
                    //ListaImpresion.Add(Estado.Equals("A")? "ANULACION ": "ORDEN PARA " + miPedido.SubLinea.Descripcion);
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(Estado.Equals("A") ? "ANULACION" : "ORDEN DE PEDIDO", 14, FontStyle.Bold));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("FECHA: " + miPedido.fecha.ToShortDateString() + " HORA: " + miPedido.fecha.ToShortTimeString(), 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("PEDIDO: " + miPedido.numero + "   " + (miPedido.isLlevar ? "PARA LLEVAR" : "MESA : " + miPedido.mesa.numero), 12, FontStyle.Bold));
                    if (miPedido.nombreClientePedido.Length > 0) impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("CLIENTE : " + miPedido.nombreClientePedido, 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("VENDEDOR: " + miPedido.usuario.login, 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(" ", 10, FontStyle.Regular));
                    //ListaImpresion.Add("====================")
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(("CODBAR").PadRight(4, ' ').PadLeft(8, ' ') + "       " + ("CANT").PadRight(4), 12, FontStyle.Regular));                    
                    //ListaImpresion.Add("CANT.       DETALLE");
                    foreach (Detalle_Pedido miLinea in miPedido.listaDetalle)
                    {
                        //impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(impresora.AgregarItem1(miLinea.itemUnidad.CodBar, "-", miLinea.cantidad.ToString("0.00"),12), 12, FontStyle.Bold));
                        impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato(miLinea.itemUnidad.CodBar.PadRight(4, '-').PadLeft(8, ' ') + "-------" + miLinea.cantidad.ToString("0").PadLeft(3, '-'), 12, FontStyle.Bold));
                        if (miLinea.observacion.Length > 0 && Estado == "V") impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("Obs: " + miLinea.observacion, 10, FontStyle.Bold));
                    }
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    //ListaImpresion.Add("");
                    impresora.Imprimir();

                    if (ImpCortePapel)
                        ImprimirCortePapel(miImpre.impresora, miImpre.NumColumImpresora);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultAPI> CambiarMesa(int Piso, int MesaAnt, int MesaNueva, int codUser, bool imprimirTicket)
        {
            var response = new ResultAPI();
            var user = new Usuario();
            var miPiso = new Piso();
            try
            {
                user = await _reposiUsuario.GetUser(codUser);
                miPiso = await _reposiPiso.BuscarDatos(Piso);
                using (SqlConnection sql = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Grab_Rest_Pedido]", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "CAMBIAR_MESA"));
                        cmd.Parameters.Add(new SqlParameter("@ePiso", Piso));
                        cmd.Parameters.Add(new SqlParameter("@eMesa", MesaAnt));
                        cmd.Parameters.Add(new SqlParameter("@eMesaNueva", MesaNueva));
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows && await reader.ReadAsync())
                            {
                                response.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            }

                            if (response.codigo == -1)
                                response.message_error = "Ocurrió un error al intentar cambiar de mesa.";
                            if (response.codigo == -2)
                                response.message_error = "La mesa ingresada no existe.";
                            if (response.codigo == -3)
                                response.message_error = "La mesa ingresada ya se encuentra ocupada.";
                            if (response.codigo > 0)
                            {
                                response.message_error = "OK";
                                response.estado = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response.message_error = e.Message;
            }

            if (response.estado && imprimirTicket)
            {
                ImprimirTicketCambioMesa(user.login, miPiso, MesaAnt, MesaNueva, true);
            }
            return response;
        }

        public async Task<List<Impresora>> GetImpresoraCambioMesa(int Piso, int Mesa)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Pedido]", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "IMPRESORAS_CAMBIOMESA"));
                        cmd.Parameters.Add(new SqlParameter("@ePiso", Piso));
                        cmd.Parameters.Add(new SqlParameter("@eMesa", Mesa));

                        var response = new List<Impresora>();
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var miImp = new Impresora();
                                    miImp.impresora = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                    miImp.NumCopias = reader.IsDBNull(1) ? 1 : reader.GetInt32(1);
                                    miImp.NumColumImpresora = reader.IsDBNull(2) ? 40 : reader.GetInt32(2);
                                    response.Add(miImp);
                                }
                            }
                            return response;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void ImprimirCortePapel(string namePrinter, int numColumn) {
            try
            {
                var Ticket = new LayerPrinter.CreaTicket(namePrinter, numColumn);
                Ticket.TextoCentro("");
                Ticket.TextoCentro("");
                Ticket.Imprimir();
                Ticket.CortaTicket();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void ImprimirTicketCambioMesa(String Usuario, Piso piso, int mesaAnt, int mesaNew, bool ImpCortePapel)
        {
            try
            {
                var listImpresora = new List<Impresora>();
                listImpresora = await GetImpresoraCambioMesa(piso.codigo, mesaNew);
                foreach (var miImpre in listImpresora)
                {
                    var ListaImpresion = new List<string>();
                    var impresora = new dllimpresion.ImpresoraFormato(miImpre.impresora, miImpre.NumColumImpresora, miImpre.NumCopias);

                    impresora.ConfiguraImpresora();

                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("  CAMBIO DE MESA", 14, FontStyle.Bold));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("FECHA : " + DateTime.Now.ToShortDateString() + "  HORA: " + DateTime.Now.ToShortTimeString(), 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("PISO  : " + piso.descripcion, 12, FontStyle.Bold));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("M. ANT: " + mesaAnt + "   " + "M. NUEVA: " + mesaNew, 12, FontStyle.Bold));

                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("USUARIO: " + Usuario,10,FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.ListImpresion.Add(new dllimpresion.ImpresoraFormato.TextoFormato("", 10, FontStyle.Regular));
                    impresora.Imprimir(true);

                    if (ImpCortePapel)
                        ImprimirCortePapel(miImpre.impresora, miImpre.NumColumImpresora);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
