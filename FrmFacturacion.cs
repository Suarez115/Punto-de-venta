using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//importación de las librerias
using BLL;
using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
namespace Proyecto_Final_Lenguajes
{
    public partial class FrmFacturacion : Form
    {
        const string usuario = "ucrlenguajes2022@gmail.com";
        const string password = "universidad2022";
        Conexion conexion;
        Productos producto;
        Bitacora bitacora;
        DetFactura detFactura;
        Facturas factura;
        CuentaPorCobrar cuentaPorCobrar;
        Cliente cliente;
        string usuarioSistema;
        public decimal totalPagar;
        //
        decimal subTotalFactura = 0;//esta variable es el monto a pagar sin descuento ni impuestos.
        decimal montoDescuentoFactura = 0;//esta variable guarda el monto total de descuento de todos los productos comprados.
        decimal montoImpuestoFactura = 0;//esta variable garda el monto total de impuesto de  los productos comprados
        decimal totalPagarFactura = 0; //esta variable guarda el monto total a pagar deduciendo los impuestos y el descuento
        string pdf="*****************Big Food Services****************"+"\n"+
                   "Factura por la compra de: "+"\n";
        public FrmFacturacion(string usuarioSistema)
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
            this.comboTipoPago.Items.Add("Tipo Pago");
            this.comboTipoPago.Items.Add("Efectivo");
            this.comboTipoPago.Items.Add("Tarjeta");
            this.comboTipoPago.Items.Add("SINPE");
            this.comboTipoPago.Items.Add("Credito");
            this.comboTipoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboTipoPago.Text = "Tipo Pago";
            this.usuarioSistema = usuarioSistema;
            this.CrearCarpetaFacturas();
        }// 

        private void FrmFacturacion_Load(object sender, EventArgs e)
        {
           
        }

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }//

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            
        }//
        public void llenarObjetoProducto(int codigo)
        {
            try
            {
                 this.producto = this.conexion.BusquedaProductosCodigo((txtCodigo.Text.Trim()));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método llenarObjetoProducto
        public string validaciones()
        {
            try
            {
                if (this.txtCedulaCliente.Text.Trim().Equals(""))
                {
                return ("Digite la cédula del cliente");
                }
                if (this.txtCodigo.Text.Trim().Equals(""))
                {
                    return ("Digite el codigo para agregar el producto");
                }
                if (this.txtCantidad.Text.Trim().Equals(""))
                {
                    return ("Digite una cantidad para el producto");
                }
                return "Correcto";
            }
            catch (Exception ex)
            {   
                throw ex;
            }        
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.producto = this.conexion.BusquedaProductosCodigo(this.txtCodigo.Text.Trim());
                if (this.producto == null)
                {
                    MessageBox.Show("El producto digitado no se encuentra en el sistema.");
                }
                else
                {
                    if (this.producto.existencia > 0)
                    {
                        if (this.validaciones() != "Correcto")
                        {
                            MessageBox.Show(this.validaciones());
                        }
                        else
                        {
                            this.llenarObjetoProducto(Convert.ToInt32(this.txtCodigo.Text.Trim()));
                            DataGridViewRow file = new DataGridViewRow();
                            file.CreateCells(dtgDatosFactura);
                            file.Cells[0].Value = txtCodigo.Text.Trim();
                            file.Cells[1].Value = producto.descripcion;
                            file.Cells[2].Value = producto.precioVenta;
                            file.Cells[3].Value = this.txtCantidad.Text.Trim();
                            dtgDatosFactura.Rows.Add(file);
                            int nuevaExistencia = this.producto.existencia - Convert.ToInt32(this.txtCantidad.Text.Trim());
                            this.conexion.ActualizarExistencias(this.producto.codigoBarra,nuevaExistencia);
                            this.obtenerTotal();
                        }
                    }
                    else
                    {
                        MessageBox.Show("El producto digitado no se encuentra Agotado");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void obtenerTotal()
        {
            txtTotalPagar.Text=totalPagar.ToString();
            foreach (DataGridViewRow row in dtgDatosFactura.Rows)
            {
                totalPagar += Convert.ToDecimal(row.Cells["Precio"].Value.ToString()) * Convert.ToDecimal(row.Cells["Cantidad"].Value.ToString());
            }//fin del foreach
            this.txtTotalPagar.Text = totalPagar.ToString();
            totalPagar = 0;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult eliminar = MessageBox.Show("¿Desea Eliminar Producto?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (eliminar == DialogResult.Yes)
                {
                    Productos temp = this.conexion.BusquedaProductosCodigo(this.dtgDatosFactura.Rows[this.dtgDatosFactura.SelectedCells[0].RowIndex].Cells["CodigoBarra"].Value.ToString());
                    int nuevaExistencia = temp.existencia + Convert.ToInt32(this.dtgDatosFactura.Rows[this.dtgDatosFactura.SelectedCells[0].RowIndex].Cells["Cantidad"].Value.ToString());
                    this.conexion.ActualizarExistencias(temp.codigoBarra,nuevaExistencia);
                    dtgDatosFactura.Rows.Remove(dtgDatosFactura.CurrentRow);
                    this.obtenerTotal();
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                this.InsertarDetalleFactura();
                this.totalPagar = 0;
                this.subTotalFactura = 0;
                this.montoDescuentoFactura = 0;
                this.montoImpuestoFactura = 0;
                this.totalPagarFactura = 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void EnvioCorreo(StringBuilder mensaje,DateTime fechaEnvio,string emisor,string receptor,string asunto,out string error,string numeroFactura)
        {
            error = "";
            try
            {
                mensaje.Append(Environment.NewLine);
                mensaje.Append(string.Format("Este correo ha sido enviado el dia {0:dd/MM/yyyy} a las {0:HH:mm:ss} Hrs: /n/n", fechaEnvio));
                mensaje.Append(Environment.NewLine);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emisor);
                mail.To.Add(receptor);
                mail.Subject = asunto;
                mail.Body = mensaje.ToString();
                string path = @"C:\Facturas\factura" + numeroFactura + ".pdf";
                mail.Attachments.Add(new Attachment(path));
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(usuario, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                error = "Email Enviado Correctamente";
                MessageBox.Show(error);
            }
            catch (Exception ex)
            {
                error = "Error: " + ex.Message;
                MessageBox.Show(error);
                return;
            }
        }//fin métodoEnvioCorreo
        public void InsertarDetalleFactura()
        {
            try
            {
                if (this.comboTipoPago.SelectedItem.ToString().Equals("Tipo Pago"))
                {
                    MessageBox.Show("Por favor elija un tipo de pago");
                }
                else 
                {
                    this.factura = new Facturas();
                    if (this.conexion.ContarFacturas() == 0)
                    {
                        this.factura.numeroFactura = 1;
                    }
                    else
                    {
                        this.factura.numeroFactura = this.conexion.consultarNumeroFactura() + 1;
                    }
                    this.detFactura = new DetFactura();
                    this.conexion.AgregarFactura(factura);
                    foreach (DataGridViewRow row in dtgDatosFactura.Rows)
                    {
                        this.detFactura.numFactura = factura.numeroFactura;
                        this.detFactura.codInterno = this.conexion.consultarCodigoInternoProd(Convert.ToString(row.Cells["CodigoBarra"].Value.ToString()));//aqui va el método que me devuelve el codigo interno por codigo de barra
                        this.detFactura.cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value.ToString());
                        this.detFactura.precioUnitario = this.conexion.BusquedaProductosCodigo(Convert.ToString(row.Cells["CodigoBarra"].Value.ToString())).precioVenta;
                        this.detFactura.subtotal = (this.detFactura.precioUnitario) * (this.detFactura.cantidad);
                        this.detFactura.porImp = (this.conexion.BusquedaProductosCodigo(Convert.ToString(row.Cells["CodigoBarra"].Value.ToString())).impuesto) * this.detFactura.subtotal;
                        this.detFactura.porDescuento = ((this.conexion.BusquedaProductosCodigo(Convert.ToString(row.Cells["CodigoBarra"].Value.ToString())).descuento) * detFactura.cantidad);
                        this.montoImpuestoFactura += this.detFactura.porImp;
                        this.montoDescuentoFactura += this.detFactura.porDescuento;
                        this.subTotalFactura += this.detFactura.subtotal;
                        this.conexion.crearDetFactura(detFactura);
                        this.pdf += "Producto: " + Convert.ToString(row.Cells["Descripción"].Value.ToString()) + " Cantidad: " + this.detFactura.cantidad + " Subtotal " + this.detFactura.subtotal + " Impuesto " + this.detFactura.porImp + " Descuento " + this.detFactura.porDescuento + "\n";
                    }//fin del foreach
                    this.totalPagarFactura = (this.subTotalFactura - this.montoDescuentoFactura) + this.montoImpuestoFactura;
                    this.factura.codCliente = Convert.ToInt32(this.txtCedulaCliente.Text.Trim());
                    this.factura.subtotal = this.subTotalFactura;
                    this.factura.montoDescuento = this.montoDescuentoFactura;
                    this.factura.montoImpuesto = this.montoImpuestoFactura;
                    this.factura.total = this.totalPagarFactura;
                    if (this.comboTipoPago.SelectedItem.ToString().Equals("Efectivo") ||
                        this.comboTipoPago.SelectedItem.ToString().Equals("Tarjeta") ||
                        this.comboTipoPago.SelectedItem.ToString().Equals("SINPE"))
                    {
                        this.factura.condicion = "Contado";
                    }
                    else
                    {
                        this.factura.condicion = "Credito";
                    }
                    this.factura.estado = "F";
                    this.factura.usuario = this.usuarioSistema;
                    this.factura.tipoPago = this.comboTipoPago.SelectedItem.ToString();
                    this.conexion.CompletarFactura(this.factura);
                    if (this.comboTipoPago.SelectedItem.ToString().Equals("Credito"))
                    {
                        this.cuentaPorCobrar = new CuentaPorCobrar();
                        this.cuentaPorCobrar.numFactura = this.factura.numeroFactura;
                        this.cuentaPorCobrar.codCliente = Convert.ToInt32(this.txtCedulaCliente.Text.Trim());
                        this.cuentaPorCobrar.fechaFactura = DateTime.Now;
                        this.cuentaPorCobrar.montoFactura = this.factura.total;
                        this.cuentaPorCobrar.usuario = this.usuarioSistema;
                        this.cuentaPorCobrar.estado = "A";
                        this.conexion.AgregarCuentaCobrar(cuentaPorCobrar);
                        this.bitacora = new Bitacora();
                        this.bitacora.tabla = "cuentaPorCobrar";
                        this.bitacora.usuario = this.usuarioSistema;
                        this.bitacora.maquina = Environment.MachineName;
                        this.bitacora.fecha = DateTime.Now;
                        this.bitacora.tipoMov = "Inserción";
                        this.bitacora.registro = "" + this.factura.numeroFactura;
                        this.conexion.Auditoria(this.bitacora);
                    }
                    this.bitacora = new Bitacora();
                    this.bitacora.tabla = "Factura";
                    this.bitacora.usuario = this.usuarioSistema;
                    this.bitacora.maquina = Environment.MachineName;
                    this.bitacora.fecha = DateTime.Now;
                    this.bitacora.tipoMov = "Inserción";
                    this.bitacora.registro = "" + this.factura.numeroFactura;
                    this.conexion.Auditoria(this.bitacora);
                    this.pdf += "Total A pagar: " + this.factura.total;
                    this.FinalizarEnvio("" + factura.numeroFactura);
                    this.limpiarPantalla();
                    this.pdf = "*****************Big Food Services****************" + "\n" +
                     "Factura por la compra de: " + "\n";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método InsertarDetalleFactura
        public void limpiarPantalla()
        {
            this.dtgDatosFactura.Rows.Clear();
            this.txtCedulaCliente.Text = "";
            this.txtCantidad.Text = ""; 
            this.txtCodigo.Text = "";
            this.txtTotalPagar.Text = "";
        }
        public void ValidarFacturacion()
        {
            try
            {
                
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void CrearPdf(string pdf,string numeroFactura)
        {
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(@"C:\Facturas\factura" + numeroFactura+".pdf", FileMode.Create));
            doc.Open();
            Paragraph title = new Paragraph();
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLUE);
            title.Add("Factura Big Food Services");
            doc.Add(title);
            doc.Add(new Paragraph(pdf));
            doc.Close();
           
        }//
        public void FinalizarEnvio(string numeroFactura)
        {
            if (this.ValidarCliente(this.txtCedulaCliente.Text.Trim())!=null)
            {
                string error = "";
                StringBuilder mensaje = new StringBuilder();
                mensaje.Append("");
                this.CrearPdf(this.pdf, numeroFactura);
                EnvioCorreo(mensaje, DateTime.Now, "ucrlenguajes2022@gmail.com",this.ValidarCliente(this.txtCedulaCliente.Text.Trim()).email, "Factura Electrónica", out error, numeroFactura);
                MessageBox.Show("Se Completó la Factura correctamente");
            }
        }
        public void CrearCarpetaFacturas()
        {
            string folderPath = @"C:\Facturas";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine(folderPath);
            }
        }
        public Cliente ValidarCliente(string cedula)
        {
            try
            {
                this.cliente = this.conexion.ConsultarCliente(this.txtCedulaCliente.Text.Trim());
                if (this.cliente != null)
                {
                    return this.cliente;
                }
                else
                {
                    MessageBox.Show("La cédula del cliente digitada no se encuentra en el sistema.");
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCedulaCliente_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtPago_KeyPress(object sender, KeyPressEventArgs e)
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }//fin clase
}//fin namespace
