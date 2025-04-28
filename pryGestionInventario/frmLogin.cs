using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryGestionInventario
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        clsConexionBD conexion = new clsConexionBD();
        int intentosRestantes = 3;

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            clsAdmins admin = new clsAdmins();
            admin.Usuario = txtUsuario.Text;
            admin.Passw = txtPassw.Text;

            bool resultado = conexion.VerificarAdministradores(admin);
            if (intentosRestantes > 0)
            {
                if (resultado == true)
                {
                    frmMain ventana = new frmMain();
                    this.Hide();
                    ventana.ShowDialog();
                    
                }
                else
                {
                    intentosRestantes--;
                    MessageBox.Show("Datos incorrectos, reintenta nuevamente, Intentos restantes: " + intentosRestantes.ToString());
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void txtPassw_TextChanged(object sender, EventArgs e)
        {
            if (txtUsuario.Text != "" && txtPassw.Text != "")
                btnIniciar.Enabled = true;

            else btnIniciar.Enabled = false;
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            if (txtUsuario.Text != "" && txtPassw.Text != "")
                btnIniciar.Enabled = true;

            else btnIniciar.Enabled = false;
        }
    }
}
