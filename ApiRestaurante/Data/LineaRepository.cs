using ApiRestaurante.Model.Inventario;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class LineaRepository
    {
        private readonly String _ConnectionString;
        public LineaRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<List<Linea>> GetLista()
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Linea]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "COMBO"));
                    var response = new List<Linea>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Linea miPto = new Linea();
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
