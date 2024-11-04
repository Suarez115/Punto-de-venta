using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DetFactura
    {
        public int numFactura { get; set; }
        public int codInterno { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
        public decimal porImp { get; set; }
        public decimal porDescuento { get; set; }
    }//fin clase
}
