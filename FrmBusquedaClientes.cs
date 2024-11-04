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
    public partial class FrmBusquedaClientes : Form
    {
        Conexion conexion;
        string usuarioSistema;
        public FrmBusquedaClientes(string usuarioSistema)
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
            this.usuarioSistema = usuarioSistema;
        }

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                this.mostrarFrmClientes(0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin btnNuevoCliente_Click
        public void mostrarFrmClientes(int function)
        {
            try
            {
                FrmClientes frm = new FrmClientes(this.usuarioSistema);
                frm.setFuncion(function);
                if (function == 1)
                {
                    frm.setCedulaConsultar(this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Cédula"].Value.ToString());
                }
                frm.ShowDialog();
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtCodigoBarra_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.BuscarCliente(this.txtCedula.Text.Trim());
            }
            catch (Exception ex)
            {

                throw ex; 
            }
        }//fin txtCodigoBarra_TextChanged
        public void BuscarCliente(string cedula)
        {
            try
            {
                this.dtgDatos.DataSource = this.conexion.BuscarClientePorCedula(cedula).Tables[0];
                this.dtgDatos.AutoResizeColumns();
                this.dtgDatos.ReadOnly = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método BuscarCliente

        private void dtgDatos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                this.mostrarFrmClientes(1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void EliminarCliente()
        {
            try
            {
                if (this.dtgDatos.Rows.Count > 0)
                {

                    if (this.dtgDatos.SelectedCells.Count > 0)
                    {
                        string cedula = this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Cédula"].Value.ToString();

                        if (MessageBox.Show("Desea eliminar el Cliente con cédula" + cedula, "Confimrar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.conexion.EliminarCliente(cedula);
              
                            MessageBox.Show("Se eliminó correctametne");
                        }
                    }
                    else
                    {
                        throw new Exception("Consulte los datos del cliente a eliminar");
                    }//
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método EliminarProducto

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.EliminarCliente();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
