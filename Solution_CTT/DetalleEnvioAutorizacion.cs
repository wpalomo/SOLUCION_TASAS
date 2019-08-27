using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT
{
    public class DetalleEnvioAutorizacion
    {
        public string IDFactura { get; set; }
        public string ClaveAcceso { get; set; }
        public string NumeroFactura { get; set; }
        public string FechaFactura { get; set; }
        public string Cliente { get; set; }
        public string CorreoCliente { get; set; }
        public string Mensaje { get; set; }
    }
}