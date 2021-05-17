using ApiRestaurante.Model.Restaurant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class MesaRepository
    {
        private readonly String _ConnectionString;
        private PisoRepository _reposiPiso;
        public MesaRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
            _reposiPiso = new PisoRepository(configuration);
        }

        public async Task<Mesa> BuscarDatos(int numero, int codPiso)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Mesa]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "POR_NUMERO"));
                    cmd.Parameters.Add(new SqlParameter("eNumero", numero));
                    cmd.Parameters.Add(new SqlParameter("ePiso", codPiso));
                    var response = new Mesa();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.numero = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            if ((reader.IsDBNull(1) ? 0 : reader.GetInt32(1)) > 0)
                                response.piso = await _reposiPiso.BuscarDatos(reader.GetInt32(1));
                            response.descripcion = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            response.numSillas = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            response.disponible = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                            response.estado = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<List<Mesa>> GetLista(int codPiso )
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Mesa]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "LISTA_MESAS"));
                    cmd.Parameters.Add(new SqlParameter("@ePiso", codPiso));
                    var response = new List<Mesa>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miM = new Mesa();
                            miM.numero = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miM.numSillas = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            miM.descripcion = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            miM.disponible = reader.IsDBNull(3) ? false : reader.GetBoolean(3);
                            miM.estado = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            miM.piso.codigo = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                            miM.piso.descripcion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            miM.cant_pedido = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                            miM.total_pedido = reader.IsDBNull(8) ? 0: Double.Parse(reader.GetDecimal(8).ToString());
                            miM.fecha_ingreso = reader.IsDBNull(9) ? new DateTime() : reader.GetDateTime(9);
                            response.Add(miM);
                        }
                        return response;
                    }
                }
            }
        }
    }
}
