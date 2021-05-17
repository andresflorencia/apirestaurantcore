using ApiRestaurante.Model.Printer;
using ApiRestaurante.Model.Restaurant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class ImpresoraRepository
    {
        private readonly String _ConnectionString;

        public ImpresoraRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<Impresora> BuscarDatos(int PtoVta, int TipoDoc = 1)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_ConfigImpresion]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "POR_TIPODOC"));
                    cmd.Parameters.Add(new SqlParameter("@ePtoVta", PtoVta));
                    cmd.Parameters.Add(new SqlParameter("@eTipoDoc", TipoDoc));
                    var response = new Impresora();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {                            
                            response.impresora = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            response.NumCopias = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                            response.NumColumImpresora = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        }
                        return response;
                    }
                }
            }
        }
    }
}
