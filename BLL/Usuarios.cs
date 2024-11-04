using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Usuarios
    {
        public string login { get; set; }
        public string password { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string estado { get; set; }
        public bool ConfirmarPassword(string clave)
        {
            try
            {
                bool confirmado = false;
                if (this.password.Trim().Equals(clave))
                {
                    confirmado = true;
                }
                return confirmado;
            }
            catch (Exception ex)
            {
                throw ex;  
            }
        }


    }
}
