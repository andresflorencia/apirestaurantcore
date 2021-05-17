using ApiRestaurante.Model.General;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class PtoVtaRepository
    {
        private readonly String _ConnectionString;
        public PtoVtaRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<PtoVta> BuscarDatos(int Codigo)
        {

            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_PtoVta]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "FILL"));
                    cmd.Parameters.Add(new SqlParameter("eCodigo", Codigo));
                    var response = new PtoVta();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.Codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            response.Descripcion = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.Sucursal = reader.IsDBNull(2) ? 0 : reader.GetInt16(2);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<List<PtoVta>> GetLista(int Sucursal = 0)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_PtoVta]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", Sucursal ==0?"COMBO":"COMBO_FILL"));
                    cmd.Parameters.Add(new SqlParameter("@eSucursal", Sucursal));
                    var response = new List<PtoVta>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            PtoVta miPto = new PtoVta();
                            miPto.Codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miPto.Descripcion = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.Add(miPto);
                        }
                        return response;
                    }
                }
            }
        }
    }
}
