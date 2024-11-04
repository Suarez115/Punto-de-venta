using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using BLL;
using DAL;
namespace Proyecto_Final_Lenguajes
{
    public partial class FrmUsuarios : Form
    {
        //variable para majenar la reerencia de la conexión
        private Conexion conexion;
        //variable para manejar la referencia al objeto
        private Usuarios usuario;
        //variable para controlar la logirar de registrar o modificar
        private int function = 0;
        private string loginConsultar = null;
        public FrmUsuarios()
        {
            InitializeComponent();
            //se crea la isntancia de la conexion
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
        }//fin constructor

        private void ptbSalir_Click(object sender, EventArgs e)
        {
           this.Close();
        }//fin del metodo salir
        public void RegistrarUsuario()
        {
            try
            {
                this.conexion.RegistrarUsuario(this.usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método RegistrarUsuario
        private void validacionesDatosRequeridos()
        {
            try
            {
                if (this.txtLogin.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el login en blanco");
                }
                if (this.txtContrasena.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite la contraseña en blanco");
                }
                if (this.txtEstado.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el Estado en blanco");
                }
                if (this.txtConfirmacion.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite la contraseña en blanco");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método validacionesDatosRequeridos
        private void CrearObjetoUsuario()
        {
            try
            {
                this.validacionesDatosRequeridos();
                this.usuario = new Usuarios();
                this.usuario.login = this.txtLogin.Text.Trim();
                this.usuario.password = this.txtContrasena.Text.Trim();
                this.usuario.estado = this.txtEstado.Text.Trim();
                if (!this.usuario.ConfirmarPassword(this.txtConfirmacion.Text.Trim()))
                {
                    throw new Exception("La confirmación de la contraseña es incorrecta");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método CrearObjetoUsuario

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.CrearObjetoUsuario();
                if (this.function == 0)
                {
                    this.RegistrarUsuario();
                    MessageBox.Show("Usuario registrado Correctamente.", "Proceso aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Desea aplicar los cambios","Confirmar",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        this.modificarUsuario();
                        MessageBox.Show("Usuario Modificado Correctamente.", "Proceso aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                this.Close();
            }//fin del try
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }//fin del método btnAgregar_Click
        public void setFuncion(int valor)
        {
            this.function = valor;
        }//fin del método setFuncion
        public void setLoginConsultar(string valor)
        {
            this.loginConsultar = valor;
        }//fin del método setLoginConsultar

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            try
            {
                //logica de negocio= 1 es modificar
                if (this.function==1)
                {
                    //el valor 1 representa la funcion de modificar
                    this.btnAgregar.Text = "Modificar";
                    //mostrar el login del usuario a modificar
                    this.txtLogin.Text = loginConsultar;
                    //el login se convierte en solo lectura
                    this.txtLogin.ReadOnly = true;
                    //se llama al método consultar para mostrar los datos
                    this.consultarUsuario();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//fin metodo FrmUsuarios_Load
        public void consultarUsuario()
        {
            try
            {
                //
                this.usuario = this.conexion.consutarUsuario(this.loginConsultar);
                if (this.usuario!=null)
                {
                    //se rellena el front end
                    this.txtLogin.Text = this.usuario.login;
                    this.txtContrasena.Text = this.usuario.password;
                    this.txtConfirmacion.Text = this.usuario.password;
                    this.txtEstado.Text = this.usuario.estado;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método consultarUsuario
        private void modificarUsuario()
        {
            try
            {
                this.conexion.modificarUsuario(this.usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método modificarUsuario

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

        private void txtEstado_KeyPress(object sender, KeyPressEventArgs e)
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
