using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using ApiRestaurante.Model.Restaurant;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestaurante.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioPisoController : Controller
    {
        private readonly UsuarioPisoRepository _repository;
        public UsuarioPisoController(UsuarioPisoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{codigo}")]
        public async Task<UsuarioPiso> BuscarDatos(int codigo)
        {
            return await _repository.BuscarDatos(codigo);
        }
    }
}
