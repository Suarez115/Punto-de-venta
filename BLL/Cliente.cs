using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Cliente
    {
        public string cedulaLegal { get; set; }
        public string tipoCedula { get; set; }
        public string nombreCompleto { get; set; }
        public string email { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
    }//fin de la clase
}
