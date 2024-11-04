using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Driver para SQL Server
using System.Data;
using System.Data.SqlClient;
using BLL;
namespace DAL
{
    public class Conexion
    {
        //Objetos que interactual con la BD
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter dataAdapter;
        private SqlDataReader dataReader;
        //contenedor de datos
        private DataSet dataSet;   
        //variable para almacenar el string de conexion
        private string strConexion;
        //constructor de la clase
        public Conexion(string strCnx)
        {
            this.strConexion = strCnx;
        }//fin constructor

        //metodo para realizar un intento de autenticacion
        public bool autenticacion(Usuarios usuario)
        {
            try
            {
                bool autorizado = false;
                //se crea una instancia de la conexion
                this.connection = new SqlConnection(this.strConexion);
                //se intenta abrir la conexion
                this.connection.Open();
                //se instancia el comando
                this.command = new SqlCommand();
                //se asigna la conexión
                this.command.Connection = this.connection;
                //se indica el tipo de comando
                this.command.CommandType = System.Data.CommandType.StoredProcedure;
                //se asigna el nombre del procedimiento almacenado
                this.command.CommandText = "[Sp_Cns_Usuario]";
                //asigna el valir de los aprámetros
                this.command.Parameters.AddWithValue("@login",usuario.login);
                this.command.Parameters.AddWithValue("@password",usuario.password);
                // realiza lectura de los datos del usuario
                this.dataReader = this.command.ExecuteReader();
                if (this.dataReader.Read())
                {
                    autorizado = true;
                }//fin if
                return autorizado;
            }//fin try
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método autenticacion
        public DataSet BuscarUsuarios(string login)
        {
            try
            {
                //se instancia una conexion
                this.connection = new SqlConnection(this.strConexion);
                //se intenta abrir la conexión
                this.connection.Open();
                //se instancia el comando 
                this.command = new SqlCommand();
                //se asigna la conección al comando
                this.command.Connection = this.connection;
                //se indica el tipo de comando
                this.command.CommandType = CommandType.StoredProcedure;
                //se indica el nombre del procedimiento
                this.command.CommandText = "[Sp_Cns_UsuariosPorNombre]";
                //asignamos el valor del parámetro del procedimiento
                this.command.Parameters.AddWithValue("@login",login);
                //se intancia un adaptador
                this.dataAdapter = new SqlDataAdapter();
                //se instancia un dataSet para guardar los datos
                this.dataAdapter.SelectCommand = this.command;
                this.dataSet = new DataSet();
                //se llena el data set con los datos del comando
                this.dataAdapter.Fill(this.dataSet);
                //se cierran los recursos
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
                this.dataAdapter.Dispose();
                //se retorna el dataset
                return this.dataSet;
            }//fin try
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método para buscar usuarios
        public void RegistrarUsuario(Usuarios usuario)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Ins_Usuarios]";
                //Se asignan los valores a cada parámetro
                this.command.Parameters.AddWithValue("@login",usuario.login);
                this.command.Parameters.AddWithValue("@password",usuario.password);
                this.command.Parameters.AddWithValue("@estado",usuario.estado);
                this.command.ExecuteNonQuery();
                //Cierre de conexión
                this.connection.Close();
                //Liberamos los recursos
                this.connection.Dispose();
                this.command.Dispose();
            }//fin del try
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método RegistrarUsuario
        //método para mostrar los datos de un usuario
        public Usuarios consutarUsuario(string login)
        {
            try
            {
                Usuarios temp = null;
                //se instancia la conexion
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Cns_Datos_Usuario]";
                //se asignan los parámetros
                this.command.Parameters.AddWithValue("@login",login);
                //se ejecuta el comando de lectura
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    //se crea una instancia del objeto usuario
                    temp = new Usuarios();
                    //se rellena el objeto
                    temp.login = this.dataReader.GetValue(0).ToString();
                    temp.password = this.dataReader.GetValue(1).ToString();
                    temp.estado = this.dataReader.GetValue(3).ToString();
                    temp.fechaRegistro = Convert.ToDateTime(this.dataReader.GetValue(2));
                    //cerramos conexion
                    this.connection.Close();
                    //liberamos recursos
                    this.connection.Dispose();
                    this.command.Dispose();
                    this.dataReader = null;
                    //se retorna el objeto
                    return temp;
                }

                return temp;
            }//fin try
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método consutarUsuario
        public void modificarUsuario(Usuarios usuario)
        {
            try
            {
                //se instancia la conexión
                this.connection = new SqlConnection(this.strConexion);
                //se abre la conexión
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_upd_Usuario]";
                //se asignan los parámetros
                this.command.Parameters.AddWithValue("@login", usuario.login);
                this.command.Parameters.AddWithValue("@password",usuario.password);
                this.command.Parameters.AddWithValue("@estado",usuario.estado);
                //se ejecuta el comando
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Método encargado de eliminar los datos de usuario//
        public void eliminarUsuario(string login)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Del_Usuario]";
                this.command.Parameters.AddWithValue("@login", login);
                this.command.ExecuteNonQuery();
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método eliminarUsuario
        //Módulo Productos//
        public DataSet BuscarProductos(int @codigobarra)
        {
            try
            {
                //se instancia una conexion
                this.connection = new SqlConnection(this.strConexion);
                //se intenta abrir la conexión
                this.connection.Open();
                //se instancia el comando 
                this.command = new SqlCommand();
                //se asigna la conección al comando
                this.command.Connection = this.connection;
                //se indica el tipo de comando
                this.command.CommandType = CommandType.StoredProcedure;
                //se indica el nombre del procedimiento
                this.command.CommandText = "[Sp_Cns_productosPorNombre]";
                //asignamos el valor del parámetro del procedimiento
                this.command.Parameters.AddWithValue("@codigobarra", @codigobarra);
                //se intancia un adaptador
                this.dataAdapter = new SqlDataAdapter();
                //se instancia un dataSet para guardar los datos
                this.dataAdapter.SelectCommand = this.command;
                this.dataSet = new DataSet();
                //se llena el data set con los datos del comando
                this.dataAdapter.Fill(this.dataSet);
                //se cierran los recursos
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
                this.dataAdapter.Dispose();
                //se retorna el dataset
                return this.dataSet;
            }//fin del try
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método BuscarProductos
        public void agregarProducto(Productos producto)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Ins_Producto]";
                //Se asignan los valores a cada parámetro
                this.command.Parameters.AddWithValue("@codigoBarra", producto.codigoBarra);
                this.command.Parameters.AddWithValue("@descripcion", producto.descripcion);
                this.command.Parameters.AddWithValue("@precioVenta", producto.precioVenta);
                this.command.Parameters.AddWithValue("@descuento", producto.descuento);
                this.command.Parameters.AddWithValue("@impuesto", producto.impuesto);
                this.command.Parameters.AddWithValue("@unidadMedia", producto.unidadMedida);
                this.command.Parameters.AddWithValue("@precioCompra", producto.precioCompra);
                this.command.Parameters.AddWithValue("@usuario ", producto.usuario);
                this.command.Parameters.AddWithValue("@existencia", producto.existencia);
                this.command.ExecuteNonQuery();
                //Cierre de conexión
                this.connection.Close();
                //Liberamos los recursos
                this.connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método agregarProducto
        //método para traer el objeto producto de la bd con el codigo de barra
        public Productos BusquedaProductosCodigo(string codigoBarra)
        {
            try
            {
                Productos producto = null;
                this.connection = new SqlConnection(this.strConexion);
                //se abre la conexión
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Cns_productosDatos]";

                this.command.Parameters.AddWithValue("@codigobarra", codigoBarra);
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    producto = new Productos();
                    producto.codigoBarra =this.dataReader.GetValue(0).ToString();
                    producto.descripcion = this.dataReader.GetValue(1).ToString();
                    producto.precioVenta = Convert.ToDecimal(this.dataReader.GetValue(2));
                    producto.descuento = Convert.ToDecimal(this.dataReader.GetValue(3));
                    producto.impuesto = Convert.ToDecimal(this.dataReader.GetValue(4));
                    producto.unidadMedida = Convert.ToInt32(this.dataReader.GetValue(5));
                    producto.precioCompra = Convert.ToDecimal(this.dataReader.GetValue(6));
                    producto.usuario = "Admin";//debe arreglarse
                    producto.existencia = Convert.ToInt32(this.dataReader.GetValue(8));
                    //cerramos conexion
                    this.connection.Close();
                    //liberamos recursos
                    this.connection.Dispose();
                    this.command.Dispose();
                    this.dataReader = null;
                }//fin if
                 //se retorna el objeto
                return producto;
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método BusquedaProductosCodigo
        public void modificarProducto(Productos producto)
        {
            try
            {
                //se instancia la conexión
                this.connection = new SqlConnection(this.strConexion);
                //se abre la conexión
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Upd_Producto]";
                //se asignan los parámetros
                this.command.Parameters.AddWithValue("@codigoBarra", producto.codigoBarra);
                this.command.Parameters.AddWithValue("@descripcion",producto.descripcion);
                this.command.Parameters.AddWithValue("@precioVenta", producto.precioVenta);
                this.command.Parameters.AddWithValue("@descuento", producto.descuento);
                this.command.Parameters.AddWithValue("@impuesto", producto.impuesto);
                this.command.Parameters.AddWithValue("@unidadMedia", producto.unidadMedida);
                this.command.Parameters.AddWithValue("@precioCompra", producto.precioCompra);
                this.command.Parameters.AddWithValue("@usuario", producto.usuario);
                this.command.Parameters.AddWithValue("@existencia", producto.existencia);
                //se ejecuta el comando
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Método encargado de eliminar los datos de usuario//
        public void EliminarProducto(int codigoBarra)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText= "Sp_Del_Producto";
                this.command.Parameters.AddWithValue("@codigoBarra", codigoBarra);
                this.command.ExecuteNonQuery();
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método EliminarProducto
        public void crearDetFactura(DetFactura detFactura)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Ins_DetFactura]";
                this.command.Parameters.AddWithValue("@numFactura",detFactura.numFactura);
                this.command.Parameters.AddWithValue("@codigoInterno", detFactura.codInterno);
                this.command.Parameters.AddWithValue("@cantidad", detFactura.cantidad);
                this.command.Parameters.AddWithValue("@precioUnitario", detFactura.precioUnitario);
                this.command.Parameters.AddWithValue("@subtotal", detFactura.subtotal);
                this.command.Parameters.AddWithValue("@porImp", detFactura.porImp);
                this.command.Parameters.AddWithValue("@porDescuento", detFactura.porDescuento);
                this.command.ExecuteNonQuery();
                //Cierre de conexión
                this.connection.Close();
                //Liberamos los recursos
                this.connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }//fin del método crearDetFactura
        public int consultarCodigoInternoProd(string codigoBarra)
        {
            try
            {
                int codigoInterno = 0;
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Cns_CodInterno]";
                this.command.Parameters.AddWithValue("@codigobarra", codigoBarra);
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    codigoInterno = Convert.ToInt32(this.dataReader.GetValue(0).ToString());
                }
                //cerramos conexion
                this.connection.Close();
                //liberamos recursos
                this.connection.Dispose();
                this.command.Dispose();
                this.dataReader = null;
                return codigoInterno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método consultarCodigoInternoProd
        //método para agregar una nueva Factura
        public void AgregarFactura(Facturas facturas)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_CrearFactura]";
                this.command.Parameters.AddWithValue("@numeroFactura",facturas.numeroFactura);
                this.command.ExecuteNonQuery();
                //Cierre de conexión
                this.connection.Close();
                //Liberamos los recursos
                this.connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método agregarFactura
        public int consultarNumeroFactura()
        {
            try
            {
                int numeroFactura = 0;
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Cns_NFactura]";
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    numeroFactura = Convert.ToInt32(this.dataReader.GetValue(0).ToString());
                }
                //cerramos conexion
                this.connection.Close();
                //liberamos recursos
                this.connection.Dispose();
                this.command.Dispose();
                this.dataReader = null;
                return numeroFactura;
            }
            catch (Exception)
            {

                throw;
            }
        }//fin del método consultarNumeroFactura
        public void ActualizarExistencias(string codigoBarra,int cantidad)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText= "[Sp_Disminuir_Existencias]";
                this.command.Parameters.AddWithValue("@CodigoBarra",codigoBarra);
                this.command.Parameters.AddWithValue("@cantidad",cantidad);
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin método ActualizarExistencias
        public void CompletarFactura(Facturas facturas)
        {
            try
            {
                this.connection = new SqlConnection(strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText="[Sp_CompletarFactura]";
                this.command.Parameters.AddWithValue("@numeroFactura",facturas.numeroFactura);
                this.command.Parameters.AddWithValue("@codCliente", facturas.codCliente);
                this.command.Parameters.AddWithValue("@subtotal", facturas.subtotal);
                this.command.Parameters.AddWithValue("@montoDescuento", facturas.montoDescuento);
                this.command.Parameters.AddWithValue("@montoImpuesto", facturas.montoImpuesto);
                this.command.Parameters.AddWithValue("@total",facturas.total);
                this.command.Parameters.AddWithValue("@estado", facturas.estado);
                this.command.Parameters.AddWithValue("@usuario",facturas.usuario);
                this.command.Parameters.AddWithValue("@tipoPago", facturas.tipoPago);
                this.command.Parameters.AddWithValue("@condicion", facturas.condicion);
                //se ejecuta el comando
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método CompletarFactura
        //método para agregar una nueva cuenta por cobrar
        public void AgregarCuentaCobrar(CuentaPorCobrar cuentaPorCobrar)
        {
            try
            {
                this.connection = new SqlConnection(strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[AgregarCuentaCobrar]";
                this.command.Parameters.AddWithValue("@numFactura",cuentaPorCobrar.numFactura);
                this.command.Parameters.AddWithValue("@codCliente", cuentaPorCobrar.codCliente);
                this.command.Parameters.AddWithValue("@fechaFactura", cuentaPorCobrar.fechaFactura);
                this.command.Parameters.AddWithValue("@montoFactura", cuentaPorCobrar.montoFactura);
                this.command.Parameters.AddWithValue("@usuario", cuentaPorCobrar.usuario);
                this.command.Parameters.AddWithValue("@estado", cuentaPorCobrar.estado);
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método
        public CuentaPorCobrar EditarCuentaCobrar(int cedula)
        {
            try
            {
                CuentaPorCobrar cuentaPorCobrar = null;
                this.connection = new SqlConnection(this.strConexion);
                //se abre la conexión
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "";

                this.command.Parameters.AddWithValue("@codCliente", cedula);
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    cuentaPorCobrar = new CuentaPorCobrar();
                    cuentaPorCobrar.numFactura = Convert.ToInt32(this.dataReader.GetValue(0).ToString());
                    cuentaPorCobrar.codCliente =Convert.ToInt32(this.dataReader.GetValue(1).ToString());
                    cuentaPorCobrar.fechaFactura = Convert.ToDateTime(this.dataReader.GetValue(2));
                    cuentaPorCobrar.fechaRegistro = Convert.ToDateTime(this.dataReader.GetValue(3));
                    cuentaPorCobrar.montoFactura = Convert.ToDecimal(this.dataReader.GetValue(4));
                    cuentaPorCobrar.estado = this.dataReader.GetValue(5).ToString();

                    //cerramos conexion
                    this.connection.Close();
                    //liberamos recursos
                    this.connection.Dispose();
                    this.command.Dispose();
                    this.dataReader = null;
                }//fin if
                 //se retorna el objeto
                return cuentaPorCobrar;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método EditarCuentaCobrar
        public DataSet ConsultarCuentaCobrar(int cedula)
        {
            try
            {
                //se instancia una conexion
                this.connection = new SqlConnection(this.strConexion);
                //se intenta abrir la conexión
                this.connection.Open();
                //se instancia el comando 
                this.command = new SqlCommand();
                //se asigna la conección al comando
                this.command.Connection = this.connection;
                //se indica el tipo de comando
                this.command.CommandType = CommandType.StoredProcedure;
                //se indica el nombre del procedimiento
                this.command.CommandText = "[Cns_CuentaCobrar]";
                //asignamos el valor del parámetro del procedimiento
                this.command.Parameters.AddWithValue("@codCliente", cedula);
                //se intancia un adaptador
                this.dataAdapter = new SqlDataAdapter();
                //se instancia un dataSet para guardar los datos
                this.dataAdapter.SelectCommand = this.command;
                this.dataSet = new DataSet();
                //se llena el data set con los datos del comando
                this.dataAdapter.Fill(this.dataSet);
                //se cierran los recursos
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
                this.dataAdapter.Dispose();
                //se retorna el dataset
                return this.dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método ConsultarCuentaCobrar
        public void EliminarCuentaCobrar(int numeroFactura)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Eliminar_CuentaCobra]";
                this.command.Parameters.AddWithValue("@numeroFactura",numeroFactura);
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }//EliminarCuentaCobrar
        public void Auditoria(Bitacora bitacora)
        {
            try
            {
                this.connection = new SqlConnection(strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Auditoria]";
                this.command.Parameters.AddWithValue("@tabla", bitacora.tabla);
                this.command.Parameters.AddWithValue("@usuario", bitacora.usuario);
                this.command.Parameters.AddWithValue("@maquina", bitacora.maquina);
                this.command.Parameters.AddWithValue("@fecha", bitacora.fecha);
                this.command.Parameters.AddWithValue("@tipoMov", bitacora.tipoMov);
                this.command.Parameters.AddWithValue("@registro", bitacora.registro);
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin metodo Auditoria
        public DataSet BuscarFactura(int numeroFactura)
        {
            try
            {
                //se instancia una conexion
                this.connection = new SqlConnection(this.strConexion);
                //se intenta abrir la conexión
                this.connection.Open();
                //se instancia el comando 
                this.command = new SqlCommand();
                //se asigna la conección al comando
                this.command.Connection = this.connection;
                //se indica el tipo de comando
                this.command.CommandType = CommandType.StoredProcedure;
                //se indica el nombre del procedimiento
                this.command.CommandText = "[Sp_Cns_Factura]";
                //asignamos el valor del parámetro del procedimiento
                this.command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                //se intancia un adaptador
                this.dataAdapter = new SqlDataAdapter();
                //se instancia un dataSet para guardar los datos
                this.dataAdapter.SelectCommand = this.command;
                this.dataSet = new DataSet();
                //se llena el data set con los datos del comando
                this.dataAdapter.Fill(this.dataSet);
                //se cierran los recursos
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
                this.dataAdapter.Dispose();
                //se retorna el dataset
                return this.dataSet;
            }//fin try
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método BuscarFactura
        public void eliminarFactura(int numeroFactura)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Del_Factua]";
                this.command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception EX)
            {
                throw;
            }
        }//fin método Eliminar factura
        public void eliminarDetFactura(int numeroFactura)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Del_DetFactura]";
                this.command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception EX)
            {
                throw;
            }
        }//fin del método eliminarDetFactura
        public void AgregarCliente(Cliente cliente)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Ins_Cliente]";
                //Se asignan los valores a cada parámetro
                this.command.Parameters.AddWithValue("@cedulaLegal", cliente.cedulaLegal);
                this.command.Parameters.AddWithValue("@tipoCedula", cliente.tipoCedula);
                this.command.Parameters.AddWithValue("@nombreCompleto",cliente.nombreCompleto);
                this.command.Parameters.AddWithValue("@email", cliente.email);
                this.command.Parameters.AddWithValue("@estado", cliente.estado);
                this.command.Parameters.AddWithValue("@usuario", cliente.usuario);
                this.command.ExecuteNonQuery();
                //Cierre de conexión
                this.connection.Close();
                //Liberamos los recursos
                this.connection.Dispose();
                this.command.Dispose();
            }//fin del try
            catch (Exception ex)
            {

                throw ex;
            }
        }//fin del método AgregarCliente
        public DataSet BuscarClientePorCedula(string cedula)
        {
            try
            {
                //se instancia una conexion
                this.connection = new SqlConnection(this.strConexion);
                //se intenta abrir la conexión
                this.connection.Open();
                //se instancia el comando 
                this.command = new SqlCommand();
                //se asigna la conección al comando
                this.command.Connection = this.connection;
                //se indica el tipo de comando
                this.command.CommandType = CommandType.StoredProcedure;
                //se indica el nombre del procedimiento
                this.command.CommandText = "[Sp_Cns_Cliente]";
                //asignamos el valor del parámetro del procedimiento
                this.command.Parameters.AddWithValue("@cedulaLegal", cedula);
                //se intancia un adaptador
                this.dataAdapter = new SqlDataAdapter();
                //se instancia un dataSet para guardar los datos
                this.dataAdapter.SelectCommand = this.command;
                this.dataSet = new DataSet();
                //se llena el data set con los datos del comando
                this.dataAdapter.Fill(this.dataSet);
                //se cierran los recursos
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
                this.dataAdapter.Dispose();
                //se retorna el dataset
                return this.dataSet;
            }//fin try
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método BuscarClientePorCedula
        public Cliente ConsultarCliente(string cedula)
        {
            try
            {
                Cliente temp = null;
                //se instancia la conexion
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Cns_Cliente]";
                //se asignan los parámetros
                this.command.Parameters.AddWithValue("@cedulaLegal", cedula);
                //se ejecuta el comando de lectura
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    //se crea una instancia del objeto usuario
                    temp = new Cliente();
                    //se rellena el objeto
                    temp.cedulaLegal = this.dataReader.GetValue(0).ToString();
                    temp.tipoCedula = this.dataReader.GetValue(1).ToString();
                    temp.nombreCompleto = this.dataReader.GetValue(2).ToString();
                    temp.email= this.dataReader.GetValue(3).ToString();
                    temp.estado= this.dataReader.GetValue(4).ToString();
                    //cerramos conexion
                    this.connection.Close();
                    //liberamos recursos
                    this.connection.Dispose();
                    this.command.Dispose();
                    this.dataReader = null;
                    //se retorna el objeto
                    return temp;
                }
                return temp;
            }//fin try
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método ConsultarCliente
        public void EliminarCliente(string cedula)
        {
            try
            {
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Del_Cliente]";
                this.command.Parameters.AddWithValue("@CedulaLegal", cedula);
                this.command.ExecuteNonQuery();
                this.connection.Close();
                this.connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método EliminarCliente
        public void EditarCliente(Cliente cliente)
        {
            try
            {
                //se instancia la conexión
                this.connection = new SqlConnection(this.strConexion);
                //se abre la conexión
                this.connection.Open();
                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_Upd_Cliente]";
                //se asignan los parámetros
                this.command.Parameters.AddWithValue("@cedulaLegal", cliente.cedulaLegal);
                this.command.Parameters.AddWithValue("@tipoCedula", cliente.tipoCedula);
                this.command.Parameters.AddWithValue("@nombreCompleto", cliente.nombreCompleto);
                this.command.Parameters.AddWithValue("@email", cliente.email);
                this.command.Parameters.AddWithValue("@estado", cliente.estado);
  
                //se ejecuta el comando
                this.command.ExecuteNonQuery();
                this.command.Dispose();
                this.connection.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//fin del método para editar un cliente
        public int ContarFacturas()
        {
            try
            {
                int numeroFactura = 0;
                this.connection = new SqlConnection(this.strConexion);
                this.connection.Open();

                this.command = new SqlCommand();
                this.command.Connection = this.connection;
                this.command.CommandType = CommandType.StoredProcedure;
                this.command.CommandText = "[Sp_CantidadFacturas]";
                this.dataReader = this.command.ExecuteReader();
                //pregunta si tiene datos
                if (this.dataReader.Read())
                {
                    numeroFactura = Convert.ToInt32(this.dataReader.GetValue(0).ToString());
                }
                //cerramos conexion
                this.connection.Close();
                //liberamos recursos
                this.connection.Dispose();
                this.command.Dispose();
                this.dataReader = null;
                return numeroFactura;
            }
            catch (Exception)
            {

                throw;
            }
        }//fin del método 
    }//fin clase Conexión
}// fin del namespace
