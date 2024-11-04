using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Bitacora
    {
        public string tabla { get; set; }
        public string usuario { get; set; }
        public string maquina { get; set; }
        public DateTime fecha { get; set; }
        public string tipoMov { get; set; }
        public string registro { get; set; }
    }//fin clase
}
