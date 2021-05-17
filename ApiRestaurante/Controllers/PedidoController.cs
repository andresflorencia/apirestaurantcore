using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using ApiRestaurante.Model.Restaurant;
using Microsoft.AspNetCore.Mvc;
using PruebaApiNetCore.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestaurante.Controllers
{
    [Route("api/[controller]")]
    public class PedidoController : Controller
    {
        private readonly PedidoRepository _repository;
        public PedidoController(PedidoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{Accion}/{CodPedido}")]
        public async Task<Pedido> BuscarDatos(string Accion, int CodPedido)
        {
            return await _repository.BuscarDatos(Accion,CodPedido);
        }

        [HttpGet("{Accion}/{Piso}/{Mesa}/{isLlevar}")]
        public async Task<List<Pedido>> GetLista(string Accion, int Piso, int Mesa, bool isLlevar)
        {
            return await _repository.GetLista(Accion, Piso,Mesa, isLlevar);
        }
        //[FromBody]List<Pedido> ListaPedidos, 
        [HttpPost("{PtoVta}/{ImprimirTicket}/{TipoDoc}/{NuevoFormatoImp}")]
        public async Task<ResultAPI> GrabarDatos([FromBody]List<Pedido> ListaPedidos, int PtoVta, bool ImprimirTicket, int TipoDoc, bool NuevoFormatoImp)
        {
            //List<Pedido> ListaPedidos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pedido>>(json);
            return await _repository.GrabaDatos(ListaPedidos,PtoVta, ImprimirTicket, TipoDoc, NuevoFormatoImp);
        }

        [HttpGet("cambio/{piso}/{mesaAnt}/{mesaNew}/{codUser}/{imprimir}")]
        public async Task<ResultAPI> CambiarMesa(int piso, int mesaAnt, int mesaNew, int codUser, bool imprimir)
        {
            //List<Pedido> ListaPedidos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pedido>>(json);
            return await _repository.CambiarMesa(piso,mesaAnt,mesaNew, codUser, imprimir);
        }


        //[HttpGet("imprimir/{tipo}/{texto}/{autent}/{user}/{pass}")]
        //public string Imprimir(string tipo, string texto, bool autent, string user, string pass)
        //{
        //    //List<Pedido> ListaPedidos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pedido>>(json);
        //    return _repository.ImprimirPrueba(tipo, texto, autent, user, pass);
        //}

        //[HttpGet("{texto}")]
        //public string ImprimirExe(string texto)
        //{
        //    //List<Pedido> ListaPedidos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pedido>>(json);
        //    return _repository.ImprimirExe(texto);
        //}
    }
}
