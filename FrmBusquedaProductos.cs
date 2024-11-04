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
    public partial class FrmBusquedaProductos : Form
    {
        private Conexion conexion;
        private Bitacora bitacora;
        private string usuario;
        public FrmBusquedaProductos(string usuario)
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
            this.usuario = usuario;
        }

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            this.mostrarFrmProductos(0);
        }
        private void mostrarFrmProductos(int function)
        {
            try
            {
                //se instancia el frm
                FrmProductos frm = new FrmProductos(this.usuario);
                frm.setFuncion(function);
                if (function==1)
                {
                   frm.setCodigoBarraConsultar(this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Código de Barra"].Value.ToString());
                }
                //SE MUESTRA EL FRM
                frm.ShowDialog();
                //se liberan  los recuros
                frm.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del metodo mostrarFrmProductos

        private void txtCodigoBarra_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCodigoBarra.Text.Trim()!="")
                {
                    this.BuscarProductoCodigoBarra(Convert.ToInt32(txtCodigoBarra.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//fin del método txtCodigoBarra_TextChanged
        private void BuscarProductoCodigoBarra(int codigobarra)
        {
            try
            {
                this.dtgDatos.DataSource = this.conexion.BuscarProductos(codigobarra).Tables[0];
                this.dtgDatos.AutoResizeColumns();
                this.dtgDatos.ReadOnly = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//

        private void dtgDatos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                this.mostrarFrmProductos(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//
        public void EliminarProducto()
        {
            try
            {
                if (this.dtgDatos.Rows.Count>0)
                {

                    if (this.dtgDatos.SelectedCells.Count > 0)
                    {
                        int codigoBarra = Convert.ToInt32(this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Código de Barra"].Value.ToString());

                        if (MessageBox.Show("Desea eliminar el producto con código de barra" + codigoBarra, "Confimrar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.conexion.EliminarProducto(codigoBarra);
                            this.bitacora = new Bitacora();
                            this.bitacora.tabla = "Producto";
                            this.bitacora.usuario = this.usuario;
                            this.bitacora.maquina = Environment.MachineName;
                            this.bitacora.fecha = DateTime.Now;
                            this.bitacora.tipoMov = "Eliminación";
                            this.bitacora.registro = ""+codigoBarra;
                            this.conexion.Auditoria(this.bitacora);
                            MessageBox.Show("Se eliminó correctametne");
                        }
                    }
                    else
                    {
                        throw new Exception("Consulte los datos del Producto a eliminar");
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
                this.EliminarProducto();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtCodigoBarra_KeyPress(object sender, KeyPressEventArgs e)
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
