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
    public class ItemUnidadController : Controller
    {
        private readonly ItemUnidadRepository _repository;
        public ItemUnidadController(ItemUnidadRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        [HttpGet("{TipoAccion}/{Descripcion}/{CodLinea}/{porCodigo}/{Bodega}/{Sucursal}")]
        public async Task<List<ItemUnidad>> GetLista(string TipoAccion, string Descripcion, int CodLinea, bool porCodigo, int Bodega, int Sucursal)
        {
            var lista = new List<ItemUnidad>();
            if (TipoAccion.Equals("LISTA_ITEM"))
                lista = await _repository.GetListaItems(porCodigo, CodLinea, Descripcion, Bodega, Sucursal);
            else if (TipoAccion.Equals("DESCRIP_ITEM_SIN_BODEGAS"))
                lista = await _repository.BuscarDatos(TipoAccion, Descripcion, CodLinea, Bodega, Sucursal);
            return lista;
        }
    }
}
