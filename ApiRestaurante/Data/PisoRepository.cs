using ApiRestaurante.Model.Restaurant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class PisoRepository
    {
        private readonly String _ConnectionString;
        public PisoRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<Piso> BuscarDatos(int Codigo)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Piso]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "FILL"));
                    cmd.Parameters.Add(new SqlParameter("eCodigo", Codigo));
                    var response = new Piso();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            response.descripcion = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.estado = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        }
                        return response;
                    }
                }
            }
        }
        public async Task<List<Piso>> GetLista()
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Rest_Piso]", sql))
                { 
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "COMBO"));
                    var response = new List<Piso>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Piso miPto = new Piso();
                            miPto.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miPto.descripcion = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.Add(miPto);
                        }
                        return response;
                    }
                }
            }
        }
    }
}
