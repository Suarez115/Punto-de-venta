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
using DAL;
using BLL;
namespace Proyecto_Final_Lenguajes
{
    public partial class FrmBusquedaUsuarios : Form
    {
        //variable para almacenar la referencia a la capa de aceso a datos
        private Conexion conexion;
        public FrmBusquedaUsuarios()
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
        }//fin constructor

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }//fin metodo salir

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.buscarUsuariosPorNombre(this.txtLogin.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//fin metodo textchanged
        //#2.CRUD Usuario
        private void buscarUsuariosPorNombre(string nombre)
        {
            try
            {
                this.dtgDatos.DataSource = this.conexion.BuscarUsuarios(nombre).Tables[0];
                this.dtgDatos.AutoResizeColumns();
                this.dtgDatos.ReadOnly = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método buscarUsuariosPorNombre 
        private void mostrarPantallaFrmUsuario(int funcion)
        {
            try
            {
                //se intancia el frm
                FrmUsuarios frm = new FrmUsuarios();
                frm.setFuncion(funcion);
                if (funcion==1)
                {
                    frm.setLoginConsultar(this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Login"].Value.ToString());

                }
                //se muetra al ventana
                frm.ShowDialog();
                // se liberan los recursos
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }//fin del método mostrarPantallaFrmUsuario

        private void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                //mostramos frm
                //valor 0 representa funcion de registrar
                this.mostrarPantallaFrmUsuario(0);
                //actualiza la lista
                this.buscarUsuariosPorNombre("");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método btnNuevoUsuario_Click

        private void dtgDatos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                this.mostrarPantallaFrmUsuario(1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// fin del método dtgDatos_CellMouseDoubleClick

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.eliminarUsuario();
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//fin del método eliminarToolStripMenuItem_Click
        private void eliminarUsuario()
        {
            try
            {
                if (this.dtgDatos.Rows.Count>0)
                {
                    if (this.dtgDatos.SelectedCells.Count>0)
                    {
                        string login = this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Login"].Value.ToString();
                        if (MessageBox.Show("Desea eliminar el usuario "+login,"Confimrar acción",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                        {
                            this.conexion.eliminarUsuario(login);
                            MessageBox.Show("Se eliminó correctametne");
                            this.buscarUsuariosPorNombre("");
                        }
                    }
                    else 
                    {
                        throw new Exception("Consulte los datos del usuario a eliminar");
                    }
                }
                else
                {
                    throw new Exception("Seleccione la celda del usario que desea eliminar");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }// fin del método eliminarUsuario

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
    }//fin de la clase
}//fin del namesapce
