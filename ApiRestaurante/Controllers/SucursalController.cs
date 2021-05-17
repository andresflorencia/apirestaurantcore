using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using ApiRestaurante.Model.General;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestaurante.Controllers
{
    [Route("api/[controller]")]
    public class SucursalController : Controller
    {
        private readonly SucursalRepository _repository;
        public SucursalController(SucursalRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{codigo}")]
        public async Task<Sucursal> BuscarDatos(int codigo)
        {
            return await _repository.BuscarDatos(codigo);
        }

        [HttpGet()]
        public async Task<List<Sucursal>> GetLista()
        {
            return await _repository.GetLista();
        }
    }
}
