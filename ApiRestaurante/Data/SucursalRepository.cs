using ApiRestaurante.Model.General;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class SucursalRepository
    {
        private readonly String _ConnectionString;
        public SucursalRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<Sucursal> BuscarDatos(int Codigo)
        {

            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Sucursal]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "FILL"));
                    cmd.Parameters.Add(new SqlParameter("eCodigo", Codigo));
                    var response = new Sucursal();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.Codigo = reader.IsDBNull(0) ? 0 : reader.GetInt16(0);
                            response.Descripcion = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.Estado = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            response.Direccion = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            response.Telefono = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        }
                        return response;
                    }
                }
            }
        }
        
        public async Task<List<Sucursal>> GetLista()
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Sucursal]", sql))
                {                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", "COMBO"));
                    var response = new List<Sucursal>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Sucursal miPto = new Sucursal();
                            miPto.Codigo = reader.IsDBNull(0) ? 0 : reader.GetInt16(0);
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
