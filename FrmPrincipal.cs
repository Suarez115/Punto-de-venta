using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//importamos libreria para utilizar archivo de configuiracion
using Proyecto_Final_Lenguajes.Properties;
namespace Proyecto_Final_Lenguajes
{
    public partial class Frm_Principal : Form
    {
        
        public Frm_Principal()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Método encargado de mostrar la pantalla login
        /// </summary>
        private void mostrarPantallaLogin()
        {
            try
            {
                FrmLogin frm = new FrmLogin();
                //mostramos el formulario
                frm.ShowDialog();
                this.lblUser.Text = frm.getUsuario();
                //se liberan los recuersos
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método mostrar pantalla login


        //este método extrae el string de conexion
        public static string ObtenerStringConexion()
        {
            return Settings.Default.StringConexion;
        }

        private void Frm_Principal_Load(object sender, EventArgs e)
        {
            //se llama al formulario login
            this.mostrarPantallaLogin();
        }



        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Application.Exit();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            this.mostrarPantallaUsuarios();
        }
        //#1CRUD Usuario-método encargado de mostrar la pantalla buscar Usuarios
        public void mostrarPantallaUsuarios()
        {
            FrmBusquedaUsuarios frm = new FrmBusquedaUsuarios();
            //mostramos el frm
            frm.ShowDialog();

            frm.Close();
        }//fin del método para mostrar la pantalla usuarios

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mostrarPantallaLogin();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.mostrarFrmBusquedaProductos();
        }
        public void mostrarFrmBusquedaProductos()
        {
            FrmBusquedaProductos frm = new FrmBusquedaProductos(this.getUsuario());
            frm.ShowDialog();
            frm.Dispose();
        }//fin del metodo mostrarFrmProductos
        public void mostrarFrnProductos()
        {
            FrmProductos frm = new FrmProductos(this.getUsuario());
            frm.ShowDialog();
            frm.Dispose();
        }//fin metodo mostrarFrnProductos
        private void administrarProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostrarFrnProductos();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostrarPantallaFactura();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método Click iconButton2_Click
        private void mostrarPantallaFactura()
        {
            FrmBusquedaFacturas frm=new FrmBusquedaFacturas();
            frm.ShowDialog();
            frm.Dispose();
        }
        //método que muesta el formulario de para crear una factura
        public void mostrarFormularioFacturacion()
        {
            try
            {
                FrmFacturacion frm = new FrmFacturacion(this.getUsuario());
                frm.ShowDialog();
                frm.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostrarFormularioFacturacion();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostrarFrmBusquedaCliente();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin btnClientes_Click
        private void mostrarFrmBusquedaCliente()
        {
            try
            {
                FrmBusquedaClientes frm = new FrmBusquedaClientes(this.getUsuario());
                frm.ShowDialog();
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del metodo mostrarFrmBusquedaCliente
        private void mostrarFrmCliente()
        {
            try
            {
                FrmClientes frm = new FrmClientes(this.getUsuario());
                frm.ShowDialog();
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void administrarClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostrarFrmCliente();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cuentasPorCobrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mostrarFrmCuentasCobrar();
        }//fin del método cuentasPorCobrarToolStripMenuItem_Click
        public void mostrarFrmCuentasCobrar()
        {
            try
            {
                FrmBusquedaCuentasCobrar frm = new FrmBusquedaCuentasCobrar();
                frm.ShowDialog();
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//
        public string getUsuario()
        {
            return this.lblUser.Text.Trim();
        }
    }//fin de clase
}//fin del namespace

