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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        clsConexionBD conexion = new clsConexionBD();
        DataTable dtGrilla = new DataTable();

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            clsConexionBD conexion = new clsConexionBD();

            conexion.ConectarBD();
            Listar();
            conexion.DesconectarBD();
            conexion.RellenarCmb(cmbCategorias);
        }

        #region Botones
        private void btnAplicar_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (txtId.Text != "")
            {
                id = Convert.ToInt32(txtId.Text);
            } else id = 0;

            clsProductos producto = new clsProductos();
            producto.Nombre = txtNombre.Text;
            producto.Desc = txtDesc.Text;
            producto.Precio = Convert.ToDecimal(txtPrecio.Text);
            producto.Stock = Convert.ToInt32(txtStock.Text);
            producto.CategoriaId = Convert.ToInt32(cmbCategorias.SelectedValue);

            if (id == 0 && txtId.Text == "")
            {
                conexion.AgregarProducto(producto);
                
            }
            else
            {
                producto.Codigo = id;
                conexion.EditarProducto(producto);
            } 
                
            VaciarInputs();
            Listar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            VaciarInputs();
        }
        #endregion

        #region Metodos
        private void Listar()
        {
            dtGrilla = conexion.ListarGrilla();

            dgvGrilla.DataSource = dtGrilla;

            // Configurar tamaño de los headers y columnas
            dgvGrilla.Columns["Codigo"].HeaderText = "Codigo";
            dgvGrilla.Columns["Nombre"].HeaderText = "Producto";
            dgvGrilla.Columns["Descripcion"].HeaderText = "Descripción";
            dgvGrilla.Columns["Precio"].HeaderText = "Precio ($)";
            dgvGrilla.Columns["Stock"].HeaderText = "Stock";
            dgvGrilla.Columns["Categorias"].HeaderText = "Categoría";

            // Ajustar ancho de columnass
            dgvGrilla.Columns["Codigo"].Width = 65;
            dgvGrilla.Columns["Nombre"].Width = 150;
            dgvGrilla.Columns["Descripcion"].Width = 250;
            dgvGrilla.Columns["Precio"].Width = 90;
            dgvGrilla.Columns["Stock"].Width = 55;
            dgvGrilla.Columns["Categorias"].Width = 100;

            // Cambiar alineación de celdas
            dgvGrilla.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvGrilla.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Cambiar color de fondo de header
            dgvGrilla.EnableHeadersVisualStyles = false;
            dgvGrilla.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGrilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGrilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

        }

        private void Eliminar()
        {
            int id = int.Parse(txtId.Text);
            DialogResult resultado = MessageBox.Show("¿Estás seguro que quieres eliminar este registro?", "Eliminar registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes && id > 0)
            {
                if (id > 0)
                {
                    conexion.EliminarProducto(id);
                    Listar();
                    VaciarInputs();
                }
            }
        }

        private void dgvGrilla_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int filasTotales = dgvGrilla.Rows.Count;

            if (e.RowIndex >= 0 && e.RowIndex <= filasTotales)
            {
                int codigoSeleccionado = Convert.ToInt32(dgvGrilla.Rows[e.RowIndex].Cells[0].Value);

                txtId.Text = codigoSeleccionado.ToString();
                txtNombre.Text = dgvGrilla.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtDesc.Text = dgvGrilla.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtPrecio.Text = dgvGrilla.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtStock.Text = dgvGrilla.Rows[e.RowIndex].Cells[4].Value.ToString();
                cmbCategorias.Text = dgvGrilla.Rows[e.RowIndex].Cells[5].Value.ToString();

            }
        }
        #endregion

        #region Controladores de INPUTS

        private void VaciarInputs()
        {
            txtId.Text = "";
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtStock.Text = string.Empty;
            cmbCategorias.SelectedIndex = 0;
            txtDesc.Text = string.Empty;
        }
        private void Controlador()
        {
            if (txtNombre.Text != string.Empty &&
                txtPrecio.Text != string.Empty &&
                txtStock.Text != string.Empty &&
                cmbCategorias.Text != string.Empty &&
                txtDesc.Text != string.Empty)
            {
                btnAplicar.Enabled = true;
            }
            else btnAplicar.Enabled = false;
        }
        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            Controlador();
        }

        private void cmbCategorias_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Controlador();
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            Controlador();
        }

        private void txtStock_TextChanged(object sender, EventArgs e)
        {
            Controlador();
        }

        private void txtNombre_TextChanged_1(object sender, EventArgs e)
        {
            Controlador();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            if (txtId.Text != "")
            {
                btnEliminar.Enabled = true;
            }
            else btnEliminar.Enabled = false;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            DataView dvGrilla = dtGrilla.DefaultView;
            dvGrilla.RowFilter = $"Nombre LIKE '%{txtBuscar.Text}%'";
        }
    }
    #endregion
}
