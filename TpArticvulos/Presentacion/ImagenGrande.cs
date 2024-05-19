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
    public partial class frmImagenGrande : Form
    {

        private string imagen;
        public frmImagenGrande()
        {
            InitializeComponent();
        }
        public frmImagenGrande(string imagen)
        {
            InitializeComponent();
            this.imagen = imagen;
        }

        private void frmImagenGrande_Load(object sender, EventArgs e)
        {
            CargarImagen(imagen);   
        }

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
    }
}
