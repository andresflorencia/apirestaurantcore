using ApiRestaurante.Model.Restaurant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class UsuarioPisoRepository
    {
        private readonly String _ConnectionString;
        private UsuarioRepository _reposiUsuario;
        private PisoRepository _reposiPiso;
        public UsuarioPisoRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
            _reposiUsuario = new UsuarioRepository(configuration);
            _reposiPiso = new PisoRepository(configuration);
        }
        public async Task<UsuarioPiso> BuscarDatos(int codUser)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_Bus_Mot_UsuarioPabellon]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("aTipoAccion", "POR_USUARIO"));
                    cmd.Parameters.Add(new SqlParameter("eUsuario", codUser));
                    var response = new UsuarioPiso();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            if ((reader.IsDBNull(1) ? 0 : reader.GetInt32(1)) > 0)
                                response.Usuario = await _reposiUsuario.GetUser(reader.GetInt32(1));
                            if ((reader.IsDBNull(2) ? 0 : reader.GetInt32(2)) > 0)
                                response.Piso = await _reposiPiso.BuscarDatos(reader.GetInt32(2));
                        }
                        return response;
                    }
                }
            }
        }
    }
}
