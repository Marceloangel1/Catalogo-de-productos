using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;


namespace presentacion
{
    public partial class Form1 : Form
    {
        private List<Catalogo> listaCatalogo;
        private readonly object listaFiltrada;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");
        }

        private void cargar()
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            try
            {
                listaCatalogo = negocio.listar();

                dgvCatalogo.DataSource = listaCatalogo;
                ocultarColumnas();

                cargarImagen(listaCatalogo[0].ImagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()

        {
            dgvCatalogo.Columns["ImagenUrl"].Visible = false;
            dgvCatalogo.Columns["Id"].Visible = false;
        }


        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                Catalogo seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);

            }


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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            Catalogo seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            Catalogo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Seguro queres eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un Campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un Criterio para filtrar.");
                return true;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {

                {
                    if (string.IsNullOrEmpty(txtfiltroAvanzado.Text))
                    {
                        MessageBox.Show("Tienes que  cargar el filtro para numéricos...");
                        return true;
                    }

                    if (!(soloNumeros(txtfiltroAvanzado.Text)))


                    {
                        MessageBox.Show("Ingrese solo numeros por favor...");
                        return true;
                    }




                }

            }


            return false;
        }
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtfiltroAvanzado.Text;
                dgvCatalogo.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {

            List<Catalogo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaCatalogo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaCatalogo;
            }




            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnas();
        }


        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a:");
                cboCriterio.Items.Add("Menor a:");
                cboCriterio.Items.Add("Igual a:");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con:");
                cboCriterio.Items.Add("Termina con:");
                cboCriterio.Items.Add("Contiene:");
            }

        }

        public void btnDetalle_Click(object sender, EventArgs e)
        {


            Catalogo seleccionado = (Catalogo)dgvCatalogo.CurrentRow.DataBoundItem;
            frmDetalle detalle = new frmDetalle(seleccionado);
            detalle.ShowDialog();



        }
    }
    
}