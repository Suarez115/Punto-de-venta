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
    public partial class FrmClientes : Form
    {
        Conexion conexion;
        private int function = 0;
        string usuarioSistema;
        private string cedulaConsultar = null;
        Cliente cliente;
        public FrmClientes(string usuarioSistema)
        {
            InitializeComponent();
            this.conexion = new Conexion(Frm_Principal.ObtenerStringConexion());
            this.usuarioSistema = usuarioSistema;
        }

        private void ptbSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                crearObjetoCliente();
                if (this.function == 0)
                {
                    this.AgregarCliente();
                    MessageBox.Show("Cliente registrado Correctamente.", "Proceso aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
                else 
                {
                    if (MessageBox.Show("Desea aplicar los cambios", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.ModificarCliente();
                        MessageBox.Show("Cliente Modificado Correctamente.", "Proceso aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void setCedulaConsultar(string cedula)
        {
            this.cedulaConsultar = cedula;
        }//fin método setCedulaConsultar
        public void setFuncion(int valor)
        {
            this.function = valor;
        }//fin del método setFuncion

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.function==1)
                {
                    this.btnAgregar.Text = "Modificar";
                    this.txtCedula.Text = this.cedulaConsultar;
                    this.txtCedula.ReadOnly = true;
                    this.consultarCliente();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método consultarCliente
        public void consultarCliente()
        {
            try
            {
                this.cliente = this.conexion.ConsultarCliente(this.cedulaConsultar);
                if (this.cliente!=null)
                {
                    this.txtCedula.Text = this.cliente.cedulaLegal;
                    this.txtTipoCedula.Text = this.cliente.tipoCedula;
                    this.txtNombreCompleto.Text = this.cliente.nombreCompleto;
                    this.txtEmail.Text = this.cliente.email;
                    this.txtEstado.Text = this.cliente.estado;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método consultarCliente
        public void validaciones()
        {
            try
            {
                if (this.txtCedula.Text.Trim().Equals(""))
                {
                    throw new Exception("Error, no se permite el campo cédula en blanco");
                }
                if (this.txtTipoCedula.Text.Trim().Equals(""))
                {
                    throw new Exception("Error, no se permite el campo tipo cédula en blanco");
                }
                if (this.txtNombreCompleto.Text.Trim().Equals(""))
                {
                    throw new Exception("Error, no se permite el campo cliente en blanco");
                }
                if (this.txtEmail.Text.Trim().Equals(""))
                {
                    throw new Exception("Error, no se permite el campo email en blanco");
                }
                if (this.txtEstado.Text.Trim().Equals(""))
                {
                    throw new Exception("Error, no se permite el campo estado en blanco");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método validaciones
        public void crearObjetoCliente()
        {
            try
            {
                this.validaciones();
                this.cliente = new Cliente();
                this.cliente.cedulaLegal = this.txtCedula.Text.Trim();
                this.cliente.tipoCedula = this.txtTipoCedula.Text.Trim();
                this.cliente.nombreCompleto = this.txtNombreCompleto.Text.Trim();
                this.cliente.email = this.txtEmail.Text.Trim();
                this.cliente.estado = this.txtEstado.Text.Trim();
                this.cliente.usuario = this.usuarioSistema;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método crear objeto Cliente
        public void AgregarCliente()
        {
            try
            {
                this.conexion.AgregarCliente(this.cliente);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método agregar Cliente
        public void ModificarCliente()
        {
            try
            {
                this.conexion.EditarCliente(this.cliente);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método ModificarCliente

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

        private void txtTipoCedula_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtNombreCompleto_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
