using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CuentaPorCobrar
    {
        public int numFactura { get; set; }
        public int codCliente { get; set; }
        public DateTime fechaFactura { get; set; }
        public DateTime fechaRegistro { get; set; }
        public decimal montoFactura { get; set; }
        public string usuario { get; set; }
        public string estado { get; set; }
    }//fin clase
}
