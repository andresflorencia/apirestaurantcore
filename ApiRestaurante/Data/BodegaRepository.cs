using ApiRestaurante.Model.General;
using ApiRestaurante.Model.Inventario;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class BodegaRepository
    {
        private readonly String _ConnectionString;
        public BodegaRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }
        public async Task<Bodega> BuscarDatos(int Codigo)
        {

            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Bodega]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "FILL"));
                    cmd.Parameters.Add(new SqlParameter("eCodigo", Codigo));
                    var response = new Bodega();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.Codigo = reader.IsDBNull(0) ? 0 : reader.GetInt16(0);
                            response.Descripcion = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.Sucursal = reader.IsDBNull(2) ? 0 : reader.GetInt16(2);
                            response.Estado = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<List<Bodega>> GetLista(int Sucursal = 0)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Bodega]", sql))
                {                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@aTipoAccion", Sucursal == 0 ? "COMBO" : "COMBO_FILL"));
                    cmd.Parameters.Add(new SqlParameter("@eSucursal", Sucursal));
                    var response = new List<Bodega>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Bodega miPto = new Bodega();
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
