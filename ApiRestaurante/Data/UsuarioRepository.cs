using ApiRestaurante.Model.Seguridad;
using Microsoft.Extensions.Configuration;
using PruebaApiNetCore.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestaurante.Data
{
    public class UsuarioRepository
    {
        private readonly String _ConnectionString;
        public UsuarioRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<int> GetCodigo(String user, string password)
        {
            var codUser = 0;
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[LoginUser]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Accion", "LOGIN"));
                    cmd.Parameters.Add(new SqlParameter("CodUsuario", 0));
                    cmd.Parameters.Add(new SqlParameter("LoginSQL", user));
                    cmd.Parameters.Add(new SqlParameter("Clave", password));
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            codUser = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        }
                        return codUser;
                    }
                }
            }
        }

        public async Task<Usuario> GetUser(int codigo)
        {           

            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sel_Usuario]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Accion", "Filtrado"));
                    cmd.Parameters.Add(new SqlParameter("Codigo", codigo));
                    var response = new Usuario();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            response.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            response.tipoUsuario = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            //response.nombre = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            response.login = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            response.clave = reader.IsDBNull(4) ? "" : reader.GetString(4);                            
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<List<Usuario>> GetLista(String Accion)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sel_Usuario]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Accion", Accion));
                    var response = new List<Usuario>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Usuario miUser = new Usuario();
                            miUser.codigo = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            miUser.login = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            response.Add(miUser);
                        }
                        return response;
                    }
                }
            }
        }

        public async Task<ResultAPI> VerificaUsuario(int codUser, string password)
        {
            using (SqlConnection sql = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[VerificaUserAutoriza]", sql))
                { //MOV_INV_BUSCAR                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Accion", "LOGIN"));
                    cmd.Parameters.Add(new SqlParameter("CodUsuario", codUser));
                    cmd.Parameters.Add(new SqlParameter("Clave", password));
                    var response = new ResultAPI();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            if ((reader.IsDBNull(0) ? 0 : reader.GetInt32(0)) > 0) {
                                response.estado = true;
                                response.message_error = "OK";
                            }
                        }
                        return response;
                    }
                }
            }
        }
    }
}
