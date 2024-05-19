using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmPrincipal : Form
    {
        private List<Articulo> listArticulos;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Cargar();
            cbxCampo.Items.Add("Codigo");
            cbxCampo.Items.Add("Categoria");
            cbxCampo.Items.Add("Precio");
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCampo.Text == "Codigo") 
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Igual");
                cbxCriterio.Items.Add("Empieza");
                cbxCriterio.Items.Add("Termina");

            }
            else if (cbxCampo.Text == "Categoria") 
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Igual");
                cbxCriterio.Items.Add("Empieza");
                cbxCriterio.Items.Add("Termina");

            }          
            else if (cbxCampo.Text == "Precio")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Igual");
                cbxCriterio.Items.Add("Mayor");
                cbxCriterio.Items.Add("Menor");

            }
        }   

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e) 
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            Cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (ArticuloSeleccionadoIvalido())
                return;

            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            DialogResult resultado = MessageBox.Show("¿Desea eliminarlo?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes) 
            {
                negocio.EliminarFisico(seleccionado.Id);
                MessageBox.Show("Eliminacion exitosa");
                Cargar();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(ArticuloSeleccionadoIvalido())
                return;
            
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            frmAltaArticulo Modificar = new frmAltaArticulo(seleccionado, 0);
            Modificar.ShowDialog();
            Cargar();         
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (ArticuloSeleccionadoIvalido())
                return;

            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            
            frmAltaArticulo VerDetalle = new frmAltaArticulo(seleccionado, 1);
            VerDetalle.ShowDialog();
            Cargar();
        }

        private void txbFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada = listArticulos;
            string filtro = txbFiltroRapido.Text; 

            if(filtro.Length > 1) 
            {
                listaFiltrada = listArticulos.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()));
                dgvArticulos.DataSource = listaFiltrada;
            }
            else
                dgvArticulos.DataSource = listaFiltrada;
        }

        private void btnFiltroAvanzado_Click(object sender, EventArgs e)
        {
            if(EsFiltroInvalido())
                return;

            ArticuloNegocio negocio = new ArticuloNegocio();
            string campo = cbxCampo.Text;
            string criterio = cbxCriterio.Text;
            string filtro = txbFiltroAvanzado.Text;

            listArticulos = negocio.FiltroAvanzado(campo, criterio, filtro);
            dgvArticulos.DataSource = listArticulos;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Cargar();
        }

        private bool EsFiltroInvalido() 
        {
            int bandera = 0;
            
            if(cbxCampo.Text == "") 
            {
                lblCampoValido.Visible = true;
                bandera = 1;
            }
            else
                lblCampoValido.Visible = false;
            if (cbxCriterio.Text == "")
            {
                lblCriterioValido.Visible = true;
                bandera = 1;
            }
            else
                 lblCriterioValido.Visible = false;
            
            if (txbFiltroAvanzado.Text == "")
            {
                lblFiltroValido.Visible = true;
                bandera = 1;
            }
            else
                lblFiltroValido.Visible = false;
            if (cbxCampo.Text == "Precio") 
            {
                if (HayLetras(txbFiltroAvanzado.Text) || txbFiltroAvanzado.Text == "") 
                {
                    lblFiltroValido.Visible = true;
                    bandera = 1;
                }
                else
                    lblFiltroValido.Visible = false;
            }             
            if (cbxCampo.Text == "Categoria") 
            {
                if (HayNumeros(txbFiltroAvanzado.Text) || txbFiltroAvanzado.Text == "")
                {
                    lblFiltroValido.Visible = true;
                    bandera = 1;
                }
                else
                    lblFiltroValido.Visible = false;
            }


            if (bandera == 1)
                return true;

            
            return false;
        }





        //help
        private void Cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listArticulos = negocio.Listar();
            //dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listArticulos;
            OcultarColumnas();
        }

        private void OcultarColumnas() 
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Marca"].Visible = false;
        }

        private bool HayNumeros(string texto) 
        {
            foreach (char c in texto)
                if (char.IsDigit(c))
                    return true;

            return false;
        }

        private bool HayLetras(string texto)
        {
            foreach (char c in texto)
                if (char.IsLetter(c))
                    return true;

            return false;
        }

        private bool ArticuloSeleccionadoIvalido() 
        {
            if (dgvArticulos.CurrentRow != null)
            {
                return false;
            }

            MessageBox.Show("Seleccione un articulo", "Articulo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

    }
}
