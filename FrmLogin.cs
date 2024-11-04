using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
namespace Proyecto_Final_Lenguajes
{
    public partial class FrmLogin : Form
    {
        public Usuarios usuarios;
        //se crea una referencai para majenar la Dal
        private Conexion conexion;
        public FrmLogin()
        {
            InitializeComponent();
            this.usuarios = new Usuarios();
            //se instancia el objeto conexion
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());

        } 
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Usuarios temp = new Usuarios();
                //se rellenan los datos
                usuarios.login = this.txtLogin.Text.Trim();
                usuarios.password = this.txtPassword.Text.Trim();
              
                //se realiza el intento de autenticacion
                if (this.intentoAutenticacion(usuarios))
                {
                    this.Close();
                }
                else
                {
                    throw new Exception("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception)
            {

                throw;
            }
        }//fin metodo load
        private bool intentoAutenticacion(Usuarios temp)
        {
            bool autenticado = false;
            if (this.conexion.autenticacion(temp))
            {
                autenticado = true;
            }
            return autenticado;
        }//fin metodo intento autenticacion
        public string getUsuario()
        {
            return this.usuarios.login;
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }//fin clase
}//fin namespace
