using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApiNetCore.Data
{
    public class ResultAPI
    {
        public int codigo { get; set; }
        public String message_error { get; set; }
        public bool estado { get; set; }
        public ResultAPI()
        {
            this.codigo = 0;
            this.message_error = "";
            this.estado = false;
        }
    }
}