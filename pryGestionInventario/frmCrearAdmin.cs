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
    public partial class frmCrearAdmin : Form
    {
        public frmCrearAdmin()
        {
            InitializeComponent();
        }

        clsConexionBD conexion = new clsConexionBD();

        private void btnCrear_Click(object sender, EventArgs e)
        {
            clsAdmins admin = new clsAdmins();
            admin.Usuario = txtUsuario.Text.Trim();
            admin.Passw = txtPassw.Text.Trim();

            conexion.CrearAdministrador(admin);
            VaciarInputs();
        }

        private void ControladorInputs()
        {
            if (txtPassw.Text != "" && txtUsuario.Text != "")
            {
                btnCrear.Enabled = true;
            }
            else btnCrear.Enabled = false;
        }
        
        private void VaciarInputs()
        {
            txtUsuario.Text = string.Empty;
            txtPassw.Text = string.Empty;
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            ControladorInputs();
        }

        private void txtPassw_TextChanged(object sender, EventArgs e)
        {
            ControladorInputs();
        }
    }
}
