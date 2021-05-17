using ApiRestaurante.Model.Restaurant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class DetPedidoRepository
    {
        private readonly String _ConnectionString;
        private ItemUnidadRepository _reposiItem;
        public DetPedidoRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
            _reposiItem = new ItemUnidadRepository(configuration);
        }

        public async Task<List<Detalle_Pedido>> GetLista(string Accion, int CodPedido, int CodSubLinea = 0)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_DetPedido]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", Accion));
                    cmd.Parameters.Add(new SqlParameter("@ePedido", CodPedido));
                    cmd.Parameters.Add(new SqlParameter("@eSubLinea", CodSubLinea));
                    var response = new List<Detalle_Pedido>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miDetalle = new Detalle_Pedido();
                            miDetalle.pedido = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            if ((reader.IsDBNull(1) ? 0 : reader.GetInt32(1)) > 0)
                                miDetalle.itemUnidad = await _reposiItem.BuscarDatos(reader.GetInt32(1).ToString(), "V", 1, 1, 1, 2, false, 1, 1);
                            miDetalle.cantidad = reader.IsDBNull(2) ? 0 : Double.Parse(reader.GetDecimal(2).ToString());
                            miDetalle.precio = reader.IsDBNull(3) ? 0 : Double.Parse(reader.GetDecimal(3).ToString());
                            miDetalle.estado = reader.IsDBNull(4)? "": reader.GetString(4);
                            miDetalle.idFac = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                            miDetalle.observacion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            response.Add(miDetalle);
                        }
                        return response;
                    }
                }
            }
        }
        public async Task<List<Detalle_Pedido>> GetListaImp(string Accion, int CodPedido, int CodSubLinea, string Estado, string CodigosItem, string NameImpresora="")
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_DetPedido]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", Accion));
                    cmd.Parameters.Add(new SqlParameter("@ePedido", CodPedido));
                    cmd.Parameters.Add(new SqlParameter("@eSubLinea", CodSubLinea));
                    cmd.Parameters.Add(new SqlParameter("@aEstado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@aCodigos", CodigosItem));
                    cmd.Parameters.Add(new SqlParameter("@aImpresora", NameImpresora));
                    var response = new List<Detalle_Pedido>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miDetalle = new Detalle_Pedido();
                            miDetalle.pedido = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miDetalle.itemUnidad.CodItem = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);                                
                            miDetalle.cantidad = reader.IsDBNull(2) ? 0 : Double.Parse(reader.GetDecimal(2).ToString());
                            miDetalle.precio = reader.IsDBNull(3) ? 0 : Double.Parse(reader.GetDecimal(3).ToString());
                            miDetalle.estado = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            miDetalle.idFac = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                            miDetalle.observacion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            miDetalle.itemUnidad.ItemNombre = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            miDetalle.itemUnidad.CodBar = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            response.Add(miDetalle);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<bool> GrabaDatos(List<Detalle_Pedido> lista, SqlConnection cnn, SqlTransaction tran)
        {
            CultureInfo c = new CultureInfo("en-US");
            var retorno = false;
            try
            {
                foreach (Detalle_Pedido miDetalle in lista)
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Grab_Rest_DetPedido]", cnn, tran))
                    { 
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@aTipoAccion", miDetalle.accion);
                        cmd.Parameters.AddWithValue("@ePedido", miDetalle.pedido);
                        cmd.Parameters.AddWithValue("@eItem", miDetalle.itemUnidad.CodItem);
                        cmd.Parameters.AddWithValue("@dCantidad", miDetalle.cantidad);
                        cmd.Parameters.AddWithValue("@dPrecio", miDetalle.precio);
                        cmd.Parameters.AddWithValue("@aEstado", miDetalle.estado);
                        cmd.Parameters.AddWithValue("@eIdFac", miDetalle.idFac == 0 ? 0: miDetalle.idFac);
                        cmd.Parameters.AddWithValue("@aObservacion", miDetalle.observacion);
                        //await cnn.OpenAsync();
                        int result = 0;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows && await reader.ReadAsync())
                            {
                                result = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                retorno = result > 0;
                            }
                        }
                    }
                    if (!retorno)
                        return false;
                }
            }
            catch (Exception e)
            {
                retorno = false;
            }
            return retorno;
        }

        public async Task<bool> EliminarDetPedido(int codPedido, SqlConnection cnn, SqlTransaction tran)
        {
            var retorno = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Elim_Rest_DetPedido]", cnn, tran))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@aTipoAccion", "ELIMINAR");
                    cmd.Parameters.AddWithValue("@ePedido", codPedido);
                    //await cnn.OpenAsync();
                    int result = 0;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            result = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            retorno = result > 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                retorno = false;
            }
            return retorno;
        }
    }
}
