using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using ApiRestaurante.Model.General;
using ApiRestaurante.Model.Restaurant;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestaurante.Controllers
{
    [Route("api/[controller]")]
    public class MesaController : Controller
    {
        private readonly MesaRepository _repository;
        public MesaController(MesaRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{numero}/{codPiso}")]
        public async Task<Mesa> BuscarDatos(int numero, int codPiso)
        {
            return await _repository.BuscarDatos(numero, codPiso);
        }

        [HttpGet("{codPiso}")]
        public async Task<List<Mesa>> GetLista(int codPiso)
        {
            return await _repository.GetLista(codPiso);
        }
    }
}
