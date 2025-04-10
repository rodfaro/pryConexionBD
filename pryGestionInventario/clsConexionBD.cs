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
        string cadenaConexion = "Server=localhost;Database=Ventas;Trusted_Connection=True;";

        //conector
        SqlConnection conexion;

        //comando
        SqlCommand comandoBaseDatos;

        public string nombreBaseDeDatos;


        public void ConectarBD()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
                conexion.Open();
            }
            catch (Exception error)
            {
                MessageBox.Show("Tiene un errorcito - " + error.Message);
            }
            finally
            {
                conexion.Close();
            }

        }



        public void RellanarGrilla(DataGridView Grilla)
        {
            try
            {
                conexion.Open();
                string query = "SELECT * FROM clientes";

                SqlCommand comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                Grilla.Rows.Clear();
                Grilla.DataSource = dt;

      

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);;
            }
            finally
            {

                conexion.Close();

            }
        }

    }
}
