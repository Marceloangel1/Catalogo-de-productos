using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;





namespace presentacion
{
    public partial class frmAltaArticulo : Form
    {   
        private Catalogo catalogo = null;

        private OpenFileDialog archivo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Catalogo catalogo)
        {
            InitializeComponent();
            this.catalogo = catalogo;

            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {    
           
            CatalogoNegocio negocio = new CatalogoNegocio();
            try
            {
                if(catalogo == null)
                    catalogo = new Catalogo();

                catalogo.Codigo = txtCodigo.Text;
                catalogo.Nombre = txtNombre.Text;
                catalogo.Descripcion = txtDescripcion.Text;
                catalogo.ImagenUrl = txtImagenUrl.Text;
                catalogo.Precio = decimal.Parse(txtPrecio.Text);
                catalogo.Marca = (Marca) cboMarca.SelectedItem;
                catalogo.Categoria = (Categoria) cboCategoria.SelectedItem;

                if (catalogo.Id != 0) {
                    negocio.modificar(catalogo);
                    MessageBox.Show("Modificado Exitosamente");


                }

                else
                {

                    negocio.agregar(catalogo);
                    MessageBox.Show("Agregado Exitosamente");

                    if(archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                       
                            File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                    Close();
                }

               


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            

            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

               


                if (catalogo != null)
                {
                    txtCodigo.Text = catalogo.Codigo;
                    txtNombre.Text = catalogo.Nombre;
                    txtImagenUrl.Text = catalogo.ImagenUrl;
                    cargarImagen(catalogo.ImagenUrl);
                    txtDescripcion.Text = catalogo.Descripcion;
                    txtPrecio.Text = catalogo.Precio.ToString();
                    cboCategoria.SelectedValue = catalogo.Categoria.Id;
                    cboMarca.SelectedValue = catalogo.Marca.Id;


                        




                }
               
                
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            
            }

            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.Listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";






            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            


        }

        private void txturlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);

        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxCatalogo.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxCatalogo.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM= ");
            }

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
             archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
                
                
            }
           

        }
    }
}
