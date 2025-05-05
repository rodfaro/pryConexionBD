using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Windows.Forms;
using System.Data;

namespace pryGestionInventario
{
    internal class clsConexionBD
    {
        //cadena de conexion
        //string cadenaConexion = "Server=localhost;Database=Ventas;Trusted_Connection=True;";

        SqlConnection conexion = new SqlConnection("Server=localhost;Database=Comercio;Trusted_Connection=True;");

        public void ConectarBD()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Tiene un errorcito - " + error.Message);
            }

        }

        public void DesconectarBD()
        {
            try
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Tiene un errorcito - " + error.Message);
            }

        }


        public void AgregarProducto(clsProductos producto)
        {
            try
            {

                string query = $@"INSERT INTO Productos(Nombre, Descripcion, Precio, Stock, CategoriaId)
                                   VALUES (@Nombre, @Descripcion, @Precio, @Stock, @CategoriaId)";

                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
                comando.Parameters.AddWithValue("@Descripcion", producto.Desc);
                comando.Parameters.AddWithValue("@Precio", producto.Precio);
                comando.Parameters.AddWithValue("@Stock", producto.Stock);
                comando.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);

                comando.ExecuteNonQuery();
                //MessageBox.Show("Producto agregado a la base de datos", "Base de Datos");

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: as" + ex.Message);;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void EditarProducto(clsProductos producto)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string query = $@"UPDATE Productos SET
                                Nombre = @Nombre,
                                Descripcion = @Descripcion,
                                Precio = @Precio,
                                Stock = @Stock,
                                CategoriaId = @CategoriaId
                            WHERE Codigo = @Codigo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Codigo", producto.Codigo);
                    comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", producto.Desc);
                    comando.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
                    comando.Parameters.AddWithValue("@Precio", producto.Precio);
                    comando.Parameters.AddWithValue("@Stock", producto.Stock);

                    comando.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public void EliminarProducto(int id)
        {
            try
            {
                string query = "DELETE FROM Productos WHERE Codigo = @Codigo";

                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
                
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Codigo", id);

                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message); ;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public DataTable ListarGrilla()
        {
            DataTable dt = new DataTable();

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string query = $@"SELECT p.Codigo, p.Nombre, p.Descripcion, p.Precio, p.Stock, c.Nombre AS Categorias 
                                FROM Productos p
                                JOIN Categorias c ON p.CategoriaId = c.Id";

                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);

                adaptador.Fill(dt);    
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message); ;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                } 
            }
            return dt;
        }

        public void RellenarCmb(ComboBox cmbox)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
                string query = "SELECT Id, Nombre FROM Categorias";

                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                cmbox.Items.Clear();
                cmbox.DataSource = dt;
                cmbox.DisplayMember = "Nombre";
                cmbox.ValueMember = "Id";

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message); ;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        public bool VerificarAdministradores(clsAdmins admin)
        {
            string query = "SELECT COUNT(*) FROM Administradores WHERE Usuario COLLATE Latin1_General_BIN = @Usuario AND Passw COLLATE Latin1_General_BIN = @Passw";
            bool usuarioCorrecto = false;

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@Usuario", admin.Usuario);
                    command.Parameters.AddWithValue("@Passw", admin.Passw);

                    int matchs = (int)command.ExecuteScalar();

                    if (matchs != 0)
                    {
                        usuarioCorrecto = true;
                    }
                    else
                    {
                        usuarioCorrecto = false;

                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: "+ ex.Message);;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return usuarioCorrecto;
        }

        public void CrearAdministrador(clsAdmins admin)
        {
            string query = $@"INSERT INTO Administradores(Usuario, Passw)
                                VALUES (@Usuario, @Passw)";
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@Usuario", admin.Usuario);
                    command.Parameters.AddWithValue("@Passw", admin.Passw);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Nuevo administrador CREADO Correctamente", "Sistema Admin", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }
    }
}
