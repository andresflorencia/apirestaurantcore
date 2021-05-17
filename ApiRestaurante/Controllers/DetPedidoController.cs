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
    public class DetPedidoController : Controller
    {
        private readonly DetPedidoRepository _repository;
        public DetPedidoController(DetPedidoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{CodPedido}")]
        public async Task<List<Detalle_Pedido>> GetListaDetalle(int CodPedido)
        {
            return await _repository.GetLista("LISTA_DETALLE", CodPedido);
        }
    }
}
