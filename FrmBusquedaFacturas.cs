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
    public partial class FrmBusquedaFacturas : Form
    {
        Conexion conexion;
        public FrmBusquedaFacturas()
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
        }

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();     
        }

        private void btnBuscarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                this.BusquedaFactura(Convert.ToInt32(this.txtCodigoBarra.Text.Trim())); 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//
        public void BusquedaFactura(int numeroFactura)
        {
            try
            {
                this.dtgDatos.DataSource = this.conexion.BuscarFactura(numeroFactura).Tables[0];
                this.dtgDatos.AutoResizeColumns();
                this.dtgDatos.ReadOnly = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método BusquedaFactura
        private void eliminarFactura()
        {
            try
            {
                if (this.dtgDatos.Rows.Count > 0)
                {
                    if (this.dtgDatos.SelectedCells.Count > 0)
                    {
                        int numeroFactura = Convert.ToInt32(this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["N° Factura"].Value.ToString());
                        if (MessageBox.Show("Desea eliminar la Factura numero " + numeroFactura, "Confimrar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.conexion.eliminarFactura(numeroFactura);
                            this.conexion.eliminarDetFactura(numeroFactura);
                            MessageBox.Show("Se eliminó correctametne");    
                        }
                    }
                    else
                    {
                        throw new Exception("Consulte los datos de la Factura a eliminar");
                    }
                }
                else
                {
                    throw new Exception("Seleccione la celda de la Factura que desea eliminar");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }// fin del método eliminarFactura

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.eliminarFactura();
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
