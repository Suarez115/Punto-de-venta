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
    public partial class FrmBusquedaCuentasCobrar : Form
    {
        Conexion conexion;
        private Bitacora bitacora;
        public FrmBusquedaCuentasCobrar()
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
        }
        public void buscarCuentaCobrar(int cedula)
        {
            try
            {
                this.dtgDatos.DataSource = this.conexion.ConsultarCuentaCobrar(cedula).Tables[0];
                this.dtgDatos.AutoResizeColumns();
                this.dtgDatos.ReadOnly = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método buscarCuentaCobrar
        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }//fin del método  ptbSalir_Click

        private void btonBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.buscarCuentaCobrar(Convert.ToInt32(this.txtCedula.Text.Trim()));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtCedula_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.eliminarCuentaCobrar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void eliminarCuentaCobrar()
        {
            try
            {
                if (this.dtgDatos.Rows.Count > 0)
                {
                    if (this.dtgDatos.SelectedCells.Count > 0)
                    {
                        string numeroFactura = this.dtgDatos.Rows[this.dtgDatos.SelectedCells[0].RowIndex].Cells["Número Factura"].Value.ToString();
                        if (MessageBox.Show("Desea eliminar la cuenta por cobrar de la factua N°" + numeroFactura, "Confimrar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.conexion.EliminarCuentaCobrar (Convert.ToInt32(numeroFactura));
                            this.bitacora = new Bitacora();
                            this.bitacora.tabla = "cuentaPorCobrar";
                            this.bitacora.usuario = "Admin";//aqui se emplea el método para conocer el usuario que interacitua con el sistema
                            this.bitacora.maquina = Environment.MachineName;
                            this.bitacora.fecha = DateTime.Now;
                            this.bitacora.tipoMov = "Eliminacion";
                            this.bitacora.registro = numeroFactura;
                            this.conexion.Auditoria(this.bitacora);
                            MessageBox.Show("Se eliminó correctametne");
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
