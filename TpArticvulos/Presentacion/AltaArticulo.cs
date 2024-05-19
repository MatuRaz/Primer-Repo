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
using System.Configuration;
using System.IO;

namespace Presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo;

        private Articulo articuloDetalle;

        private OpenFileDialog archivo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo, int bandera)
        {
            InitializeComponent();          
            if (bandera == 0) 
            {
                this.articulo = articulo;
                Text = "Modificar";
            }
            else 
            {
                articuloDetalle = articulo;
                Text = "Detalle";
            }
                
                
        } 

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio negocioM = new MarcaNegocio();
            CategoriaNegocio negocioC = new CategoriaNegocio();
            cbxMarca.DataSource = negocioM.Listar();
            cbxMarca.ValueMember = "Id";
            cbxMarca.DisplayMember = "Descripcion";
            cbxCategoria.DataSource = negocioC.Listar();
            cbxCategoria.ValueMember = "Id";
            cbxCategoria.DisplayMember = "Descripcion";

            if(articulo != null) 
            {
                txbCodigo.Text = articulo.Codigo;
                txbNombre.Text = articulo.Nombre;
                txbDescripcion.Text = articulo.Descripcion;
                cbxMarca.Text = articulo.Marca.ToString();
                cbxCategoria.Text = articulo.Categoria.ToString();
                txbImagen.Text = articulo.ImagenUrl;
                txbPrecio.Text = articulo.Precio.ToString();
                CargarImagen(txbImagen.Text);               
            }
            else if(articuloDetalle != null) 
            {
                txbCodigo.Text = articuloDetalle.Codigo;
                txbNombre.Text = articuloDetalle.Nombre;
                txbDescripcion.Text = articuloDetalle.Descripcion;
                cbxMarca.Text = articuloDetalle.Marca.ToString();
                cbxCategoria.Text = articuloDetalle.Categoria.ToString();
                txbImagen.Text = articuloDetalle.ImagenUrl;
                txbPrecio.Text = articuloDetalle.Precio.ToString();
                CargarImagen(txbImagen.Text);

                txbCodigo.ReadOnly = true;
                txbNombre.ReadOnly = true;
                txbDescripcion.ReadOnly = true;
                cbxMarca.Enabled = false;
                cbxCategoria.Enabled = false;
                txbImagen.ReadOnly = true;
                txbPrecio.ReadOnly = true;

                btnAceptar.Visible = false;
                btnCancelar.Visible = false;
                btnAgregarImagenLocal.Visible = false;
                btnOk.Visible = true;
            }
                        
        }

        private void txbImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txbImagen.Text);
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
      
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (CargaArticuloInvalido())
                    return;

                if (this.articulo == null)
                    this.articulo = new Articulo();

                ArticuloNegocio negocio = new ArticuloNegocio();
                

                articulo.Codigo = txbCodigo.Text;
                articulo.Nombre = txbNombre.Text;    
                articulo.Descripcion = txbDescripcion.Text;
                articulo.Marca = new Marca();
                articulo.Marca.Id = int.Parse(cbxMarca.SelectedValue.ToString());
                articulo.Categoria = new Categoria();    
                articulo.Categoria.Id = int.Parse(cbxCategoria.SelectedValue.ToString());
                articulo.ImagenUrl = txbImagen.Text;
                articulo.Precio = decimal.Parse(txbPrecio.Text);
      

                if (articulo.Id == 0)
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                    Close();
                }
                else
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                    Close();
                }
                
                if (archivo != null && !(txbImagen.Text.ToLower().Contains("http"))) 
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Images-Folder"] + archivo.SafeFileName, true );
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregarImagenLocal_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = ("jpg|*.jpg;|png|*.png");
            archivo.ShowDialog();

            txbImagen.Text = archivo.FileName;
            pbxImagen.Load(archivo.FileName);
        }

        private void pbxImagen_Click(object sender, EventArgs e)
        {
            frmImagenGrande imagen = new frmImagenGrande(txbImagen.Text);
            imagen.ShowDialog();
        }


       
        
        
        //help

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {
               pbxImagen.Load("https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg");
            }
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

        private bool CargaArticuloInvalido() 
        {
            int bandera = 0;

            if(txbCodigo.Text == "" || txbCodigo.Text.Length > 50) 
            {
                lblCodigoValido.Visible = true;
                bandera = 1;
            }
            else
                lblCodigoValido.Visible = false;
            if (txbNombre.Text == "" || txbNombre.Text.Length > 50)
            {
                lblNombreValido.Visible = true;
                bandera = 1;
            }
            else
                lblNombreValido.Visible = false;
            if (txbDescripcion.Text == "" || txbDescripcion.Text.Length > 150) 
            {
                lblDescripcionValida.Visible = true;
                bandera = 1;
            }
            else
                lblDescripcionValida.Visible = false;
            if(txbImagen.Text == "" || txbImagen.Text.Length < 10 || txbImagen.Text.Length > 1000) 
            {
                lblImagenValida.Visible = true;
                bandera = 1;
            }
            else
                lblImagenValida.Visible = false;
            if(txbPrecio.Text == "" || txbPrecio.Text.Length > 15) 
            {
                lblPrecioValido.Visible = true;
                bandera = 1;
            }
            else
                lblPrecioValido.Visible= false;
            if (HayLetras(txbPrecio.Text)) 
            {
                lblPrecionSionLetras.Visible = true;
                bandera = 1;
            }
            else
                lblPrecionSionLetras.Visible = false;



            if (bandera == 0)
                return false;

            return true;
        }


    }
}
