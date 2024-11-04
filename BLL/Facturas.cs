using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Facturas
    {
        public int numeroFactura { get; set; }
        public DateTime fecha { get; set; }
        public int codCliente { get; set; }
        public decimal subtotal { get; set; }
        public decimal montoDescuento { get; set; }
        public decimal montoImpuesto { get; set; }
        public decimal total { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public string tipoPago { get; set; }
        public string condicion { get; set; }
    }//fin clase
}
