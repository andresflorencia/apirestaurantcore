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
    public class ClienteController : Controller
    {
        private readonly ClienteRepository _repository;
        public ClienteController(ClienteRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("fill/{cedRuc}")]
        public async Task<List<Cliente>> BuscarDatos(string cedRuc)
        {
            return await _repository.BuscarDatos(cedRuc);
        }

        [HttpGet("lista/{filtro}/{maximo}")]
        public async Task<List<Cliente>> GetLista(string filtro, int maximo)
        {
            return await _repository.GetLista(filtro,maximo);
        }
    }
}
