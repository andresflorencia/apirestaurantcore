using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using ApiRestaurante.Model.Inventario;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestaurante.Controllers
{
    [Route("api/[controller]")]
    public class BodegaController : Controller
    {
        private readonly BodegaRepository _repository;
        public BodegaController(BodegaRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("fill/{codigo}")]
        public async Task<Bodega> BuscarDatos(int codigo)
        {
            return await _repository.BuscarDatos(codigo);
        }

        [HttpGet("combo/{Sucursal}")]
        public async Task<List<Bodega>> GetLista(int Sucursal)
        {
            return await _repository.GetLista(Sucursal);
        }
    }
}
