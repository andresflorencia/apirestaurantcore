using ApiRestaurante.Model.Restaurant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class ClienteRepository
    {
        private readonly String _ConnectionString;

        public ClienteRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<List<Cliente>> BuscarDatos(string cedRuc)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Cliente]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "FILLCED_MOVIL"));
                    cmd.Parameters.Add(new SqlParameter("aCedRuc", cedRuc));
                    var result = new List<Cliente>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            var response = new Cliente();
                            response.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            response.cedRuc = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.nombre  = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            response.direccion = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            response.correo = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            response.telefono = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            result.Add(response);
                        }
                        return result;
                    }
                }
            }
        }

        public async Task<List<Cliente>> GetLista(string filtro, int maximoPagina)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Cliente]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "BUSCAR_MOVIL"));
                    cmd.Parameters.Add(new SqlParameter("@aNombre", filtro));
                    cmd.Parameters.Add(new SqlParameter("@eMaximoPagina", maximoPagina));
                    var response = new List<Cliente>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var miM = new Cliente();
                            miM.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miM.cedRuc = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            miM.nombre = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            miM.direccion = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            miM.correo = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            miM.telefono = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            response.Add(miM);
                        }
                        return response;
                    }
                }
            }
        }
    }
}
