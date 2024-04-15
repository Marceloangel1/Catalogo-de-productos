using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;


namespace presentacion
{
    public partial class frmDetalle : Form
    {
        private Catalogo catalogo = new Catalogo();

        public frmDetalle(Catalogo catalogo)
        {
            InitializeComponent();
            this.catalogo = catalogo;

        }

        private void frmDetalle_Load(object sender, EventArgs e)
        {
            lblMarca.Text = catalogo.Marca.Descripcion;
            lblCategoria.Text = catalogo.Categoria.Descripcion;
            lblDescripcion.Text = catalogo.Descripcion;
            lblPrecio.Text = catalogo.Precio.ToString();
            lblNombre.Text = catalogo.Nombre;

            cargarImagen(catalogo.ImagenUrl);

        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {

                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
    }
}
