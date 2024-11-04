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
    public partial class FrmProductos : Form
    {
        private Conexion conexion;
        private Productos producto;
        private int function = 0;
        private Bitacora bitacora;
        public string usuario;
        private string codigoBarrasConsultar = null;
        public FrmProductos(string usuario)
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
            this.usuario = usuario;
        }
        //variable par controlar la lógica de objeto usuarios

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }// fin ptbSalir_Click

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btonAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.crearObjetoProducto();
                if (this.function == 0)
                {
                    this.AgregarProducto();
                    MessageBox.Show("Producto registrado Correctamente.", "Proceso aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }//fin if
                else
                {
                    if (MessageBox.Show("Desea aplicar los cambios", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.modificarProducto();
                        MessageBox.Show("Usuario Modificado Correctamente.", "Proceso aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//fin metodo btonAgregar_Click
         //metodo para crear el objeto Producto
        //#1
        public void validaciones()
        {
            try
            {
                if (this.txtCodigoBarra.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el código de barra en blanco");
                }
                if (this.txtDescripcion.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite la descripción en blanco");
                }
                if (this.txtPrecioVenta.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el precio de venta en blanco");
                }
                if (this.txtDescuento.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el descuento en blanco");
                }
                if (this.txtImpuesto.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el impuesto en blanco");
                }
                if (this.txtUnidadMedia.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite la unidad medida en blanco");
                }
                if (this.txtPrecioCompra.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite el precio de compra en blanco");
                }
                if (this.txtExistencias.Text.Trim().Equals(""))
                {
                    throw new Exception("No se permite las existencias en blanco medida en blanco");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //#2
        public void crearObjetoProducto()
        {
            try
            {
                this.validaciones();
                this.producto = new Productos();
                this.producto.codigoBarra = this.txtCodigoBarra.Text.Trim();
                this.producto.descripcion = this.txtDescripcion.Text.Trim();
                this.producto.precioVenta = Convert.ToDecimal(this.txtPrecioVenta.Text.Trim());
                this.producto.descuento = Convert.ToDecimal(this.txtDescuento.Text.Trim());
                this.producto.impuesto = Convert.ToDecimal(this.txtImpuesto.Text.Trim());
                this.producto.unidadMedida = Convert.ToInt32(this.txtUnidadMedia.Text.Trim());
                this.producto.precioCompra = Convert.ToDecimal(this.txtPrecioCompra.Text.Trim());
                this.producto.usuario = this.usuario;
                this.producto.existencia = Convert.ToInt32(this.txtExistencias.Text.Trim());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método crearObjetoProducto
        //

        //#4
        public void AgregarProducto()
        {
            try
            {
                this.conexion.agregarProducto(this.producto);
                this.bitacora = new Bitacora();
                this.bitacora.tabla = "Producto";
                this.bitacora.usuario = this.usuario;
                this.bitacora.maquina = Environment.MachineName;
                this.bitacora.fecha = DateTime.Now;
                this.bitacora.tipoMov = "Inserción";
                this.bitacora.registro = this.producto.codigoBarra;
                this.conexion.Auditoria(this.bitacora);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void setCodigoBarraConsultar(string valor)
        {
            this.codigoBarrasConsultar = valor;
        }//fin del método setLoginConsultar
        public void setFuncion(int valor)
        {
            this.function = valor;
        }//fin del método setFuncion

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            try
            {
                //logica de negocio= 1 es modificar
                if (this.function == 1)
                {
                    //el valor 1 representa la funcion de modificar
                    this.btnAgregar.Text = "Modificar";
                    //mostrar el login del usuario a modificar
                    this.txtCodigoBarra.Text =codigoBarrasConsultar;
                    //el login se convierte en solo lectura
                    this.txtCodigoBarra.ReadOnly = true;
                    //se llama al método consultar para mostrar los datos
                    this.consultarProducto();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//fin del método FrmProductos_Load
        public void consultarProducto()
        {
            try
            {
                //se reyenan los datos
                this.producto = this.conexion.BusquedaProductosCodigo(this.codigoBarrasConsultar);
                if (this.producto != null)
                {
                    this.txtCodigoBarra.Text = "" + this.producto.codigoBarra;
                    this.txtDescripcion.Text = "" + this.producto.descripcion;
                    this.txtPrecioVenta.Text = "" + this.producto.precioVenta;
                    this.txtDescuento.Text = "" + this.producto.descuento;
                    this.txtImpuesto.Text = "" + this.producto.impuesto;
                    this.txtUnidadMedia.Text = "" + this.producto.unidadMedida;
                    this.txtPrecioCompra.Text = "" + this.producto.precioCompra;
                    this.txtExistencias.Text = "" + this.producto.existencia;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método consultarProducto
        public void modificarProducto()
        {
            try
            {
                this.conexion.modificarProducto(this.producto);
                this.bitacora = new Bitacora();
                this.bitacora.tabla = "Producto";
                this.bitacora.usuario = "Admin";//aqui se emplea el método para conocer el usuario que interacitua con el sistema
                this.bitacora.maquina = Environment.MachineName;
                this.bitacora.fecha = DateTime.Now;
                this.bitacora.tipoMov = "Editar";
                this.bitacora.registro = this.producto.ToString();
                this.conexion.Auditoria(this.bitacora);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método modificarProducto
        public string getUsuario()
        {
            return this.usuario;
        }
        public void setUsuario(string user)
        {
            this.usuario = user;
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

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtUnidadMedia_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtExistencias_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtImpuesto_KeyPress(object sender, KeyPressEventArgs e)
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
            else if (e.KeyChar.ToString().Equals(","))
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
