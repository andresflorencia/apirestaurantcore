using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using ApiRestaurante.Model.Seguridad;
using Microsoft.AspNetCore.Mvc;
using PruebaApiNetCore.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestaurante.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _repository;
        public UsuarioController(UsuarioRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        [HttpGet("login/{user}/{password}")]
        public async Task<Usuario> IniciarSesion(String user, String password)
        {
            var codUser = await _repository.GetCodigo(user, password);
            return await _repository.GetUser(codUser);
        }


        [HttpGet("{Accion}")]
        public async Task<List<Usuario>> GetLista(String Accion)
        {
            return await _repository.GetLista(Accion);
        }

        [HttpGet("{codUser}/{password}")]
        public async Task<ResultAPI> VerificaUsuario(int codUser,String password)
        {
            return await _repository.VerificaUsuario(codUser,password);
        }
    }
}
