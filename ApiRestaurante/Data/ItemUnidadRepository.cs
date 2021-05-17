using ApiRestaurante.Model.Inventario;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class ItemUnidadRepository
    {
        private readonly String _ConnectionString;
        public ItemUnidadRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }
        public async Task<ItemUnidad> BuscarDatos(string CodBar, string Tipo, int Bodega, int ListaPrecio, int Sucursal, int opc, bool DiaPromo, int ReglaCliente, int ListaPrecioCompara, bool soloServ = false)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_ItemUnidad]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "FILL_CODBAR"));
                    cmd.Parameters.Add(new SqlParameter("aTipo", Tipo));                    
                    if (opc == 2)
                        cmd.Parameters.Add(new SqlParameter("eItem", Int32.Parse(CodBar)));
                    else
                        cmd.Parameters.Add(new SqlParameter("aCodBar", CodBar));
                    cmd.Parameters.Add(new SqlParameter("eBodega", Bodega));
                    cmd.Parameters.Add(new SqlParameter("eListPrecio", ListaPrecio));
                    cmd.Parameters.Add(new SqlParameter("eSucursal", Sucursal));
                    cmd.Parameters.Add(new SqlParameter("bDiaPromo", DiaPromo));
                    cmd.Parameters.Add(new SqlParameter("eReglaCliente", ReglaCliente));
                    cmd.Parameters.Add(new SqlParameter("eListaPrecioCompara", ListaPrecioCompara));
                    cmd.Parameters.Add(new SqlParameter("bSoloServicios", soloServ));
                    var miItemUnidad = new ItemUnidad();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            miItemUnidad.CodItem = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miItemUnidad.ItemNombre = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            miItemUnidad.Unidad = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            miItemUnidad.UnidadDescripcion = reader.IsDBNull(3) ? "": reader.GetString(3);
                            miItemUnidad.UnidadSigla = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            miItemUnidad.Convertibilidad = reader.IsDBNull(5) ? 0 : Double.Parse(reader.GetSqlSingle(5).ToString());
                            //miItemUnidad.Operacion = IIf(reader.IsDBNull(6), "", reader(6))
                            miItemUnidad.Tipo = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            miItemUnidad.CodBar = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            miItemUnidad.Costo = reader.IsDBNull(9) ? 0 : Double.Parse(reader.GetSqlSingle(9).ToString());
                            miItemUnidad.Stock = reader.IsDBNull(10) ? 0 : Double.Parse(reader.GetDecimal(10).ToString());
                            miItemUnidad.Precio = reader.IsDBNull(11) ? 0 : Double.Parse(reader.GetDecimal(11).ToString());
                            miItemUnidad.IVA = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                            miItemUnidad.EstadoBodega = reader.IsDBNull(13) ? "" : reader.GetString(13);
                            miItemUnidad.Peso = reader.IsDBNull(14) ? 0 : Double.Parse(reader.GetDecimal(14).ToString());
                            miItemUnidad.MaxDescuento = reader.IsDBNull(15) ? 0 : Double.Parse(reader.GetDecimal(15).ToString());
                            miItemUnidad.CodItemSurtido = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                            miItemUnidad.UltFechaCompra = reader.IsDBNull(17) ? DateTime.Now : reader.GetDateTime(17);
                            miItemUnidad.CostoUltCompraIva = reader.IsDBNull(18) ? 0 : Double.Parse(reader.GetDecimal(18).ToString());
                            miItemUnidad.CostoUltCompra = reader.IsDBNull(19) ? 0 : Double.Parse(reader.GetSqlSingle(19).ToString());
                            miItemUnidad.CantEntera = reader.IsDBNull(20) ? false : reader.GetBoolean(20);
                            //Me.Item.CantUnidadesPorCaja = IIf(reader.IsDBNull(21), 0, reader(21)) '----ANDRÉS 15 / 08 / 2019
                            miItemUnidad.PrecioLista = reader.IsDBNull(22) ? 0 : Double.Parse(reader.GetDecimal(22).ToString());
                            miItemUnidad.PrecioListaConfig = reader.IsDBNull(23) ? 0 : Double.Parse(reader.GetDecimal(23).ToString());
                        }
                        return miItemUnidad;
                    }
                }
            }
        }

        public async Task<List<ItemUnidad>> BuscarDatos(string TipoAccion, string Descripcion, int CodLinea, int Bodega, int Sucursal)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_ItemUnidad]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", TipoAccion));
                    cmd.Parameters.Add(new SqlParameter("@aTipo", "V"));
                    cmd.Parameters.Add(new SqlParameter("@aDescripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@eBodega", Bodega));
                    cmd.Parameters.Add(new SqlParameter("@eListPrecio", 1));
                    cmd.Parameters.Add(new SqlParameter("@eSucursal", Sucursal));
                    cmd.Parameters.Add(new SqlParameter("@bDiaPromo", false));
                    cmd.Parameters.Add(new SqlParameter("@eReglaCliente", 1));
                    cmd.Parameters.Add(new SqlParameter("@eMaximoPagina", 20));
                    cmd.Parameters.Add(new SqlParameter("@bSoloServicios", false));
                    var response = new List<ItemUnidad>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miM = new ItemUnidad();
                            miM.CodBar = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            miM.CodItem = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            miM.ItemNombre = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            miM.Precio = reader.IsDBNull(6) ? 0 : Double.Parse(reader.GetDecimal(6).ToString());
                            response.Add(miM);
                        }
                        return response;
                    }
                }
            }
        }

        private async Task<List<int>> BuscarCodigos(bool porCodigo, int CodLinea, string Filtrado = "") {
            var sentencia = "";
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                if (porCodigo)
                    sentencia = "select top 10 I.eCodigo from Inv_Item I with (nolock)  where I.aEstado='V' AND I.eLinea= " + CodLinea + " order by I.eCodigo desc";
                else
                    sentencia = "select top 20 I.eCodigo from Inv_Item I with (nolock)  where I.aEstado='V' AND I.eLinea= " + CodLinea + " and I.aDescripcion like '%" + Filtrado + "%' or isnull(I.aCodBar,'')='" + Filtrado + "' order by I.aCodBar desc";

                using (SqlCommand cmd = new SqlCommand(sentencia, sql))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 0;
                    var response = new List<int>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miCodigo =  reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            if (miCodigo>0)
                                response.Add(miCodigo);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<List<ItemUnidad>> GetListaItems(bool porCodigo, int CodLinea, string Filtrado = "", int Bodega = 1, int Sucursal = 1)
        {
            List<ItemUnidad> listRetorno = new List<ItemUnidad>();
            List<int> listCodigos = await BuscarCodigos(porCodigo, CodLinea, Filtrado);

            if (listCodigos.Count > 0) {
                foreach (int c in listCodigos) {
                    var miItem = await BuscarDatos(c.ToString(), "V", Bodega, 1, Sucursal, 2, false, 1, 1);
                    if (miItem != null)
                        listRetorno.Add(miItem);
                }
            }
            return listRetorno;
        }

    }
}
